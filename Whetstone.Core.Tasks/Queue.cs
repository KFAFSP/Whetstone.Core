using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Represents a waiting queue.
    /// </summary>
    /// <remarks>
    /// Essentially implements a mutex lock.
    /// </remarks>
    [PublicAPI]
    public sealed class Queue : Disposable, ISynchronizationSource
    {
        sealed class Handle : SynchronizationHandle
        {
            public static Handle CreateHead()
            {
                var handle = new Handle();
                handle.Dispose();
                return handle;
            }

            #region SynchronizationHandle overrides
            /// <inheritdoc />
            protected override void Release()
            {
                Released.TryEnd();
            }
            #endregion

            public void Fault()
            {
                Released.Dispose();
                Dispose();
            }

            public Era Released { get; } = new Era();
        }

        [NotNull]
        Handle FHead;
        [NotNull]
        Handle FTail;

        /// <summary>
        /// Create a new <see cref="Queue"/>.
        /// </summary>
        public Queue()
        {
            FHead = Handle.CreateHead();
            FTail = FHead;
        }

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                FHead.Fault();
            }

            base.Dispose(ADisposing);
        }
        #endregion

        #region ISynchronizationSource
        /// <inheritdoc />
        public async Task<SynchronizationHandle> WaitAsync(CancellationToken ACancel)
        {
            ThrowIfDisposed();
            ACancel.ThrowIfCancellationRequested();

            var handle = new Handle();
            var predecessor = Interlocked.Exchange(ref FTail, handle);

            try
            {
                await predecessor.Released.WaitAsync(ACancel);
            }
            catch (OperationCanceledException)
            {
                // NOTE: This continuation runs synchronously at the parent task.
#pragma warning disable 4014
                predecessor.Released.WaitAsync()
                    .ContinueWith(
                        A => handle.Dispose(),
                        TaskContinuationOptions.ExecuteSynchronously
                    );
#pragma warning restore 4014
                throw;
            }
            catch (ObjectDisposedException)
            {
                handle.Fault();
                throw;
            }

            FHead = handle;
            return handle;
        }
        #endregion

        /// <summary>
        /// Get a value indicating whether the queue is empty.
        /// </summary>
        public bool IsEmpty => FHead.IsReleased;
    }
}
