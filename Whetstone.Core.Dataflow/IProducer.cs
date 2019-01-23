using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Whetstone.Core.Dataflow
{
    /// <summary>
    /// Interface for a dataflow node that produces items.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <remarks>
    /// A producer can be instructed to produce an item asynchronously. It is not required to track
    /// the items that it produced, and does not need to buffer canceled results.
    /// </remarks>
    [PublicAPI]
    public interface IProducer<T>
    {
        /// <summary>
        /// Produce an item.
        /// </summary>
        /// <param name="ACancel">The <see cref="CancellationToken"/>.</param>
        /// <returns>An awaitable <see cref="Task{T}"/> that returns the item.</returns>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        Task<T> Produce(CancellationToken ACancel);
    }
}
