using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Represents an era that can be ended exactly once.
    /// </summary>
    /// <remarks>
    /// If the end of the era coincides with the production of a value, use a
    /// <see cref="Future{TValue}"/> instead.
    /// </remarks>
    [PublicAPI]
    public sealed class Era : Disposable, IAwaitable
    {
        const string C_AlreadyEnded = @"Era has already ended.";

        /// <summary>
        /// Get a new <see cref="Era"/> that has ended.
        /// </summary>
        /// <returns>A new ended <see cref="Era"/>.</returns>
        [NotNull]
        public static Era Ended()
        {
            var era = new Era();
            var ok = era.TryEnd();
            Ensure.That(ok, "Era contested.");

            return era;
        }

        [NotNull]
        readonly TaskCompletionSource<Void> FSource = new TaskCompletionSource<Void>();

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                // If the era did not end yet, fault it.
                FSource.TrySetException(new ObjectDisposedException(typeof(Era).Name));
            }

            base.Dispose(ADisposing);
        }
        #endregion

        /// <summary>
        /// Attempt to end the era.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the era was ended, <see langword="false"/> if it had already
        /// ended or is disposed.
        /// </returns>
        public bool TryEnd() => FSource.TrySetResult(default);
        /// <summary>
        /// End the era.
        /// </summary>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        /// <exception cref="InvalidOperationException">The era has already ended.</exception>
        /// <remarks>
        /// Use this method in contexts where there is no contention on the era.
        /// </remarks>
        public void End()
        {
            if (TryEnd()) return;

            ThrowIfDisposed();
            throw new InvalidOperationException(C_AlreadyEnded);
        }

        #region IAwaitable
        /// <inheritdoc />
        public Task WaitAsync(CancellationToken ACancel) => FSource.Task.OrCanceledBy(ACancel);
        #endregion

        /// <summary>
        /// Get a value indicating whether the era has ended.
        /// </summary>
        public bool HasEnded => FSource.Task.IsCompleted;
    }
}
