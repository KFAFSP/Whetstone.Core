using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Represents a repeatable trigger.
    /// </summary>
    /// <remarks>
    /// If the firing of the trigger coincides with the production of a value, use an
    /// <see cref="Event{TData}"/> instead.
    /// </remarks>
    [PublicAPI]
    public sealed class Trigger : Disposable, IAwaitable
    {
        [NotNull]
        Era FCurrent = new Era();

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
        /// Fire the trigger once.
        /// </summary>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        public void Fire()
        {
            ThrowIfDisposed();

            var ok = Interlocked.Exchange(ref FCurrent, new Era()).TryEnd();
            Ensure.That(ok, "Era contested.");
        }

        #region IAwaitable
        /// <inheritdoc />
        public Task WaitAsync(CancellationToken ACancel) => FCurrent.WaitAsync();
        #endregion
    }
}
