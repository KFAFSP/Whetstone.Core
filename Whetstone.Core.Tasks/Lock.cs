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
            readonly Lock FParent;

            public Handle([NotNull] Lock AParent)
            {
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
        int FStrength;

        [NotNull]
        readonly Queue FQueue = new Queue();
        [CanBeNull]
        SynchronizationHandle FHead;

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

        void Strengthen()
        {
            Interlocked.Increment(ref FStrength);
        }
        void Weaken()
        {
            if (Interlocked.Decrement(ref FStrength) == 0)
            {
                FOwner = TaskContext.C_NoId;
                FHead?.Dispose();
            }
        }

        #region ISynchronizationSource
        /// <inheritdoc />
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        public async Task<SynchronizationHandle> WaitAsync(CancellationToken ACancel)
        {
            ThrowIfDisposed();
            ACancel.ThrowIfCancellationRequested();

            var id = TaskContext.CurrentId;

            Strengthen();
            // NOTE: Exception cannot be thrown.
            // ReSharper disable once ExceptionNotDocumented
            var previous = Interlocked.CompareExchange(
                ref FOwner,
                id,
                TaskContext.C_NoId
            );

            if (previous == id)
            {
                return new Handle(this);
            }

            if (previous != TaskContext.C_NoId)
            {
                Weaken();
                FHead = await FQueue.WaitAsync(ACancel);
                Strengthen();
                FOwner = id;
            }
            else
            {
                FHead = await FQueue.WaitAsync(ACancel);
            }

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
