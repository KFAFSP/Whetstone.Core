using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Whetstone.Core.Dataflow
{
    /// <summary>
    /// Interface for a dataflow node that processes items.
    /// </summary>
    /// <typeparam name="TIn">The input item type.</typeparam>
    /// <typeparam name="TOut">The output item type.</typeparam>
    /// <remarks>
    /// A processor is an unbuffered mix of <see cref="IConsumer{T}"/> and
    /// <see cref="IProducer{T}"/> at the same time, as it can be instructed to consume an item,
    /// yielding a processed one as a result of that operation. It is not required to track
    /// the items that it consumed, and does not need to buffer neither input nor output.
    /// </remarks>
    [PublicAPI]
    public interface IProcessor<TIn, TOut>
    {
        /// <summary>
        /// Process an item.
        /// </summary>
        /// <param name="AItem">The item.</param>
        /// <param name="ACancel">The <see cref="CancellationToken"/>.</param>
        /// <returns>An awaitable <see cref="Task{T}"/> that returns the processed item.</returns>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        Task<TOut> Process(TIn AItem, CancellationToken ACancel);
    }
}
