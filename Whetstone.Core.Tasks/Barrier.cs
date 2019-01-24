using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Provides a way to synchronize a known number of participants.
    /// </summary>
    [PublicAPI]
    public sealed class Barrier : Disposable, IAwaitable
    {
        int FWaitingFor;
        [NotNull]
        readonly Trigger FRelease = new Trigger();

        /// <summary>
        /// Create a new <see cref="Barrier"/>.
        /// </summary>
        /// <param name="ACount">The number of participants.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ACount"/> is not positive.
        /// </exception>
        public Barrier(int ACount)
        {
            Require.Positive(ACount, nameof(ACount));

            FWaitingFor = ACount;
        }

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                FRelease.Dispose();
            }

            base.Dispose(ADisposing);
        }
        #endregion

        /// <summary>
        /// Try to skip the barrier.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the barrier was skipped; otherwise <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Skipping the barrier means signalling it if that would unblock all awaiters; otherwise
        /// doing nothing. If the barrier is waiting for more than 1 task, it will not be signalled
        /// and instead left in the same state as it was before.
        /// </remarks>
        public bool TrySkip()
        {
            if (IsDisposed)
            {
                // Cannot skip disposed barriers.
                return false;
            }

            // Attempt to do the last decrement operation.
            // NOTE: Exception cannot be thrown.
            // ReSharper disable once ExceptionNotDocumented
            var result = Interlocked.CompareExchange(ref FWaitingFor, 0, 1);
            if (result != 1)
            {
                // There are more tasks left to await.
                return false;
            }

            // We were the final task.
            FRelease.Fire();
            // NOTE: Exception cannot be thrown.
            // ReSharper disable once ExceptionNotDocumented
            Interlocked.Increment(ref FWaitingFor);
            return true;
        }

        #region IAwaitable
        /// <inheritdoc />
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        public async Task WaitAsync(CancellationToken ACancel)
        {
            ThrowIfDisposed();
            ACancel.ThrowIfCancellationRequested();

            var remaining = Interlocked.Decrement(ref FWaitingFor);
            try
            {
                if (remaining == 0)
                {
                    FRelease.Fire();
                }
                else
                {
                    await FRelease.WaitAsync(ACancel);
                }
            }
            finally
            {
                // NOTE: Exception cannot be thrown.
                // ReSharper disable once ExceptionNotDocumented
                Interlocked.Increment(ref FWaitingFor);
            }
        }
        #endregion

        /// <summary>
        /// Get the number of participants this barrier is waiting for.
        /// </summary>
        public int WaitingFor => FWaitingFor;
    }
}
