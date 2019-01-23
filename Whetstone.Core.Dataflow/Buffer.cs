using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Dataflow
{
    /// <summary>
    /// Base class for implementing a dataflow node that buffers items.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <remarks>
    /// <para>
    /// A buffer is both a <see cref="IConsumer{T}"/> and a <see cref="IProducer{T}"/> at the same
    /// time. Depending on the implementation, the buffer is allowed to complete both requests in
    /// any order an without any guarantee of the completion time. Buffers are expected to buffer
    /// all of the items that were pushed to them and return them later when pulled.
    /// </para>
    /// <para>
    /// This interface does not make any requirements about the order of the items in the buffer
    /// with regards to the order of push operations. Buffers could be implemented as queues, bags,
    /// and many more.
    /// </para>
    /// </remarks>
    [PublicAPI]
    public abstract class Buffer<T> : Disposable, IConsumer<T>, IProducer<T>
    {
        #region Pulling
        /// <summary>
        /// Try to pull an item from the buffer.
        /// </summary>
        /// <param name="AItem">The pulled item.</param>
        /// <returns>
        /// <see langword="true"/> if an item was pulled; otherwise <see langword="false"/>.
        /// </returns>
        public virtual bool TryPull(out T AItem)
        {
            AItem = default;

            if (IsDisposed)
            {
                return false;
            }

            T[] result = new T[1];

            if (!TryPull(result))
            {
                return false;
            }

            AItem = result[0];
            return true;
        }

        /// <summary>
        /// Pull an item from the buffer.
        /// </summary>
        /// <returns>An awaitable <see cref="Task{T}"/> that returns the item.</returns>
        /// <param name="ACancel">The <see cref="CancellationToken"/>.</param>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        public virtual Task<T> Pull(CancellationToken ACancel)
        {
            ThrowIfDisposed();

            T[] result = new T[1];

            return Pull(result, ACancel)
                .ContinueWith(
                    X => result[0],
                    ACancel,
                    TaskContinuationOptions.OnlyOnRanToCompletion
                    | TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Current
                );
        }

        /// <summary>
        /// Try to pull multiple items from the buffer.
        /// </summary>
        /// <param name="ABuffer">The buffer to fill.</param>
        /// <returns>
        /// <see langword="true"/> if all the items were pulled; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ABuffer"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// In contrast to repeatedly calling <see cref="TryPull(out T)"/>, this method is supposed
        /// to pull all the requested items in a batch, as if there were no other pull operations
        /// happening concurrently. Depending on the implementation, this might mean that there is
        /// no difference at all.
        /// </remarks>
        public abstract bool TryPull([NotNull] T[] ABuffer);

        /// <summary>
        /// Pull multiple items from the buffer.
        /// </summary>
        /// <param name="ABuffer">The buffer to fill.</param>
        /// <param name="ACancel">The <see cref="CancellationToken"/>.</param>
        /// <returns>
        /// An awaitable <see cref="Task"/> that completes when <paramref name="ABuffer"/> was
        /// filled.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ABuffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        /// <remarks>
        /// In contrast to repeatedly calling <see cref="Pull()"/>, this method is supposed to pull
        /// all the requested items in a batch, as if there were no other pull operations happening
        /// concurrently. Depending on the implementation, this might mean that there is no
        /// difference at all.
        /// </remarks>
        public abstract Task Pull([NotNull] T[] ABuffer, CancellationToken ACancel);
        #endregion

        #region Pushing
        /// <summary>
        /// Try to push an item to the buffer.
        /// </summary>
        /// <param name="AItem">The item.</param>
        /// <returns>
        /// <see langword="true"/> if the item was pushed; otherwise <see langword="false"/>.
        /// </returns>
        public virtual bool TryPush(T AItem)
        {
            if (IsDisposed)
            {
                return false;
            }

            T[] values = { AItem };

            return TryPush(values);
        }

        /// <summary>
        /// Push an item to the buffer.
        /// </summary>
        /// <param name="AItem">The item.</param>
        /// <param name="ACancel">The <see cref="CancellationToken"/>.</param>
        /// <returns>
        /// An awaitable <see cref="Task"/> that completes when the item was pushed.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        public virtual Task Push(T AItem, CancellationToken ACancel)
        {
            ThrowIfDisposed();

            T[] values = { AItem };

            return Push(values, ACancel);
        }

        /// <summary>
        /// Try to push multiple items to the buffer.
        /// </summary>
        /// <param name="AItems">The items to push.</param>
        /// <returns>
        /// <see langword="true"/> if all items were pushed; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AItems"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// In contrast to repeatedly calling <see cref="TryPush(T)"/>, this method is supposed to
        /// push all the requested items in a batch, as if there were no other push operations
        /// happening concurrently. Depending on the implementation, this might mean that there is
        /// no difference at all.
        /// </remarks>
        public abstract bool TryPush([NotNull] params T[] AItems);

        /// <summary>
        /// Push multiple items to the buffer.
        /// </summary>
        /// <param name="AItems">The items to push.</param>
        /// <param name="ACancel">The <see cref="CancellationToken"/>.</param>
        /// <returns>
        /// An awaitable <see cref="Task"/> that completes when all items were pushed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AItems"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        /// <remarks>
        /// In contrast to repeatedly calling <see cref="Push(T)"/>, this method is supposed to
        /// push all the requested items in a batch, as if there were no other push operations
        /// happening concurrently. Depending on the implementation, this might mean that there is
        /// no difference at all.
        /// </remarks>
        public abstract Task Push([NotNull] T[] AItems, CancellationToken ACancel);
        #endregion

        #region IConsumer<T>
        Task IConsumer<T>.Consume(T AItem, CancellationToken ACancel) => Push(AItem, ACancel);
        #endregion

        #region IProducer<T>
        Task<T> IProducer<T>.Produce(CancellationToken ACancel) => Pull(ACancel);
        #endregion

        /// <summary>
        /// Get the number of available items.
        /// </summary>
        public abstract int Available { get; }
    }
}
