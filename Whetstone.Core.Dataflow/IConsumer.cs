using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Whetstone.Core.Dataflow
{
    /// <summary>
    /// Interface for a dataflow node that consumes items.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <remarks>
    /// A consumer can be instructed to consume an item asynchronously. It is not required to track
    /// the items that it consumed, and does not need to buffer any.
    /// </remarks>
    [PublicAPI]
    public interface IConsumer<T>
    {
        /// <summary>
        /// Consume an item.
        /// </summary>
        /// <param name="AItem">The item.</param>
        /// <param name="ACancel">The <see cref="CancellationToken"/>.</param>
        /// <returns>
        /// An awaitable <see cref="Task"/> that completes when the item was consumed.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        Task Consume(T AItem, CancellationToken ACancel);
    }
}
