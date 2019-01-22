using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Represents a turnstile that releases awaiters one by one.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In a <see cref="Turnstile"/>, the order of the awaiters is strictly preserved, but turns may
    /// still be ceded to successors using cancellation.
    /// </para>
    /// <para>
    /// All awaiters advance automatically in the queue as soon as their predecessor finishes their
    /// turn. As opposed to a <see cref="Queue"/>, however, they do not automatically unblock when
    /// they reach the head, and will only do so when <see cref="Turn"/> is called. This allows an
    /// external driver to manually release individual awaiters.
    /// </para>
    /// </remarks>
    [PublicAPI]
    public sealed class Turnstile : Disposable, IAwaitable
    {
        [NotNull]
        readonly Condition FRelease = Condition.False();
        [NotNull]
        readonly Queue FQueue = new Queue();

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                // Fault all remaining awaiters.
                FRelease.Dispose();
                FQueue.Dispose();
            }

            base.Dispose(ADisposing);
        }
        #endregion

        /// <summary>
        /// Release the next awaiter.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if an awaiter was released, <see langword="false"/> if there
        /// are no awaiters or the instance is disposed.
        /// </returns>
        public bool Turn() => FRelease.TrySet();

        #region IAwaitable
        /// <inheritdoc />
        /// <exception cref="OperationCanceledException">
        /// <paramref name="ACancel"/> was canceled.
        /// </exception>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        public async Task WaitAsync(CancellationToken ACancel)
        {
            ThrowIfDisposed();
            ACancel.ThrowIfCancellationRequested();

            // Await our turn.
            using (await FQueue.WaitAsync(ACancel))
            {
                // Await the next release.
                FRelease.TryReset();
                await FRelease.WaitAsync(ACancel);
            }
        }
        #endregion

        /// <summary>
        /// Get a value indicating whether there are awaiters.
        /// </summary>
        public bool HasWaiting => !FRelease.Value;
    }
}
