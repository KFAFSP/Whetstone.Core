using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Represents a quota that limits entry to a section.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Essentially implements a semaphore.
    /// </para>
    /// </remarks>
    [PublicAPI]
    public sealed class Quota : Disposable, ISynchronizationSource
    {
        sealed class Handle : SynchronizationHandle
        {
            [NotNull]
            readonly Quota FParent;

            public Handle([NotNull] Quota AParent)
            {
                Ensure.NotNull(AParent, nameof(AParent));

                FParent = AParent;
            }

            #region SynchronizationHandle overrides
            /// <inheritdoc />
            protected override void Release()
            {
                FParent.Release();
            }
            #endregion
        }

        [NotNull]
        readonly Turnstile FWaiting = new Turnstile();
        long FBalance;
        long FActive;

        /// <summary>
        /// Create a new <see cref="Quota"/>.
        /// </summary>
        /// <param name="AQuota">The quota.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AQuota"/> is 0 or less.
        /// </exception>
        public Quota(long AQuota)
        {
            Require.Positive(AQuota, nameof(AQuota));

            FBalance = AQuota;
        }

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                // Fault all remaining awaiters.
                FWaiting.Dispose();
            }

            base.Dispose(ADisposing);
        }
        #endregion

        /// <summary>
        /// Decrement the active count and increment the balance.
        /// </summary>
        void Release()
        {
            // Exit the section.
            Interlocked.Decrement(ref FActive);

            // Increase the balance.
            if (Interlocked.Increment(ref FBalance) <= 0)
            {
                // Release the next reservation.
                FWaiting.Turn();
            }
        }

        #region ISynchronizationSource
        /// <inheritdoc />
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        public async Task<SynchronizationHandle> WaitAsync(CancellationToken ACancel)
        {
            ThrowIfDisposed();
            ACancel.ThrowIfCancellationRequested();

            // Decrement the balance.
            if (Interlocked.Decrement(ref FBalance) < 0)
            {
                // Reservation was made, await our turn.
                try
                {
                    await FWaiting.WaitAsync(ACancel);
                }
                catch (OperationCanceledException)
                {
                    // Waiting was canceled.
                    // Increment the balance again to release the reservation.
                    // NOTE: Reference is never null.
                    // ReSharper disable once ExceptionNotDocumented
                    Interlocked.Increment(ref FBalance);
                    throw;
                }
            }

            // Enter the section.
            // NOTE: Reference is never null.
            // ReSharper disable once ExceptionNotDocumented
            Interlocked.Increment(ref FActive);
            return new Handle(this);
        }
        #endregion

        /// <summary>
        /// Get the current quota balance.
        /// </summary>
        /// <remarks>
        /// The balance is the difference between available quota and made reservations. If it is
        /// negative, some reservation is still waiting.
        /// </remarks>
        public long Balance => FBalance;
        /// <summary>
        /// Get the number of tasks currently in the monitored section.
        /// </summary>
        public long Active => FActive;
    }
}
