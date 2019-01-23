using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Asynchronous re-entrant <see langword="lock"/> implementation.
    /// </summary>
    [PublicAPI]
    public sealed class Lock : Disposable, ISynchronizationSource
    {
        sealed class Handle : SynchronizationHandle
        {
            [NotNull]
            readonly Lock FParent;

            public Handle([NotNull] Lock AParent)
            {
                Ensure.NotNull(AParent, nameof(AParent));

                FParent = AParent;
                Id = AParent.FOwner;
            }

            #region SynchronizationHandle overrides
            /// <inheritdoc />
            protected override void Release()
            {
                Ensure.That(
                    FParent.FOwner == Id,
                    @"Expired handle.",
                    @"The handle might have been duplicated and released elsewhere."
                );

                FParent.Weaken();
            }
            #endregion

            public long Id { get; }
        }

        long FOwner = TaskContext.C_NoId;
        long FStrength;

        [NotNull]
        readonly Queue FQueue = new Queue();
        [NotNull]
        SynchronizationHandle FHead = SynchronizationHandle.Released;

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                FQueue.Dispose();
            }

            base.Dispose(ADisposing);
        }
        #endregion

        /// <summary>
        /// Strengthen the lock.
        /// </summary>
        void Strengthen()
        {
            Interlocked.Increment(ref FStrength);
        }
        /// <summary>
        /// Weaken the lock, and release it if strength reached zero.
        /// </summary>
        void Weaken()
        {
            var strength = Interlocked.Decrement(ref FStrength);
            Ensure.That(
                strength >= 0,
                @"Unbalanced Strengthen/Weaken."
            );

            if (strength == 0)
            {
                FOwner = TaskContext.C_NoId;
                FHead.Dispose();
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

            var id = TaskContext.CurrentId;

            // Strengthen the lock to prevent release (temporarily).
            Strengthen();

            if (FOwner == id)
            {
                // Re-enter the lock.
                return new Handle(this);
            }

            // The lock is either unacquired or acquired by some other context.
            Task<SynchronizationHandle> turn;
            try
            {
                // Acquire an awaitable to await our turn.
                turn = FQueue.WaitAsync(ACancel);
            }
            finally
            {
                // Weak the lock to allow release again.
                Weaken();
            }

            // Await our turn.
            FHead = await turn;

            // We now have exclusive access.
            FOwner = id;
            Strengthen();
            return new Handle(this);
        }
        #endregion

        /// <summary>
        /// Get a value indicating whether this lock is acquired.
        /// </summary>
        public bool IsAcquired => FOwner != TaskContext.C_NoId;
        /// <summary>
        /// Get a value indicating whether this lock is acquired by the current context.
        /// </summary>
        public bool IsAcquiredHere => FOwner == TaskContext.CurrentId;
    }
}
