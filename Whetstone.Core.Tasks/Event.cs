using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Represents a recurring event.
    /// </summary>
    /// <remarks>
    /// Essentially a <see cref="Trigger"/> that also produces data upon firing.
    /// </remarks>
    [PublicAPI]
    public sealed class Event<TData> : Disposable, IAwaitable<TData>
    {
        [NotNull]
        Future<TData> FCurrent = new Future<TData>();

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                FCurrent.Dispose();
            }

            base.Dispose(ADisposing);
        }
        #endregion

        /// <summary>
        /// Fire the event once.
        /// </summary>
        /// <param name="AData">The event data.</param>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        public void Post(TData AData)
        {
            ThrowIfDisposed();

            var ok = Interlocked.Exchange(ref FCurrent, new Future<TData>())
                .TryPost(AData);
            Ensure.That(ok, "Future contested.");
        }

        #region IAwaitable
        /// <inheritdoc />
        public Task<TData> WaitAsync(CancellationToken ACancel) => FCurrent.WaitAsync();
        #endregion
    }
}