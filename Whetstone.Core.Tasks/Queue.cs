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
    /// <para>
    /// Essentially implements a mutex lock.
    /// </para>
    /// <para>
    /// In a <see cref="Queue"/>, the order of the awaiters is strictly preserved, but turns may
    /// still be ceded to successors using cancellation.
    /// </para>
    /// <para>
    /// All awaiters advance automatically in the <see cref="Queue"/> as soon as their predecessor
    /// finishes their turn. Any awaiter that reaches the head automatically begins their turn and
    /// unblocks. If this is not desired, and the release of the head should be controlled manually
    /// instead, use a <see cref="Turnstile"/>.
    /// </para>
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
                Turn.TryEnd();
            }
            #endregion

            public void Fault()
            {
                Turn.Dispose();
                Dispose();
            }

            [NotNull]
            public Era Turn { get; } = new Era();
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
                // Fault all remaining awaiters.
                FHead.Fault();
            }

            base.Dispose(ADisposing);
        }
        #endregion

        /// <summary>
        /// Try to skip to the head of the queue.
        /// </summary>
        /// <param name="AHandle">
        /// The output <see cref="SynchronizationHandle"/> for the skipped turn.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if skipping was successful and <paramref name="AHandle"/> is
        /// live; otherwise <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Skipping means entering an empty queue to immediately start a turn. Internally, the
        /// caller enters the queue regardless of state but cancels any wait immediately, reporting
        /// failure instead.
        /// </para>
        /// <para>
        /// When the result is <see langword="true"/>, <paramref name="AHandle"/> is acquired and
        /// live and must be disposed!
        /// </para>
        /// </remarks>
        public bool TrySkip(out SynchronizationHandle AHandle)
        {
            AHandle = null;

            if (IsDisposed)
            {
                // Cannot skip a disposed queue.
                return false;
            }

            // Make a handle to end this turn.
            var handle = new Handle();
            // Update the tail to reserve our position whilst getting our immediate predecessor.
            // NOTE: Exception cannot be thrown.
            // ReSharper disable once ExceptionNotDocumented
            var predecessor = Interlocked.Exchange(ref FTail, handle);

            if (predecessor.IsReleased)
            {
                // Successfully skipped to the head of the queue.
                AHandle = handle;
                return true;
            }

            // Immediately bypass to cede our turn.
#pragma warning disable 4014
            predecessor.Turn.WaitAsync()
                .ContinueWith(
                    A =>
                    {
                        if (A.IsFaulted)
                        {
                            handle.Fault();
                        }
                        else
                        {
                            handle.Dispose();
                        }
                    },
                    TaskContinuationOptions.ExecuteSynchronously
                );
#pragma warning restore 4014
            return false;
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

            // Make a handle to end this turn.
            var handle = new Handle();
            // Update the tail to reserve our position whilst getting our immediate predecessor.
            // NOTE: Exception cannot be thrown.
            // ReSharper disable once ExceptionNotDocumented
            var predecessor = Interlocked.Exchange(ref FTail, handle);

            try
            {
                // Wait for our predecessor to finish their turn.
                await predecessor.Turn.WaitAsync(ACancel);
            }
            catch (OperationCanceledException)
            {
                // Waiting was canceled. Bypass our turn for all successors.
                // NOTE: This continuation runs synchronously at the parent task.
#pragma warning disable 4014
                predecessor.Turn.WaitAsync()
                    .ContinueWith(
                        A =>
                        {
                            if (A.IsFaulted)
                            {
                                handle.Fault();
                            }
                            else
                            {
                                handle.Dispose();
                            }
                        },
                        TaskContinuationOptions.ExecuteSynchronously
                    );
#pragma warning restore 4014
                throw;
            }
            catch (ObjectDisposedException)
            {
                // The queue was disposed. Propagate to all successors.
                handle.Fault();
                throw;
            }

            // Our turn is now, update the head.
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
