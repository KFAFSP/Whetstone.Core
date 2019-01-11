using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Provides static factory methods for the <see cref="Future"/> class.
    /// </summary>
    [PublicAPI]
    public static class Future
    {
        /// <summary>
        /// Create a new <see cref="Future{TValue}"/> with an existing value.
        /// </summary>
        /// <param name="AValue">The value.</param>
        /// <returns>A new <see cref="Future{TValue}"/> with an existing value.</returns>
        [NotNull]
        public static Future<TValue> Of<TValue>(TValue AValue)
        {
            var future = new Future<TValue>();
            var ok = future.TryPost(AValue);
            Ensure.That(ok, "Future contested.");

            return future;
        }
    }

    /// <summary>
    /// Represents a promise of a future value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <remarks>
    /// Essentially an <see cref="Era"/> that also produces a result value upon ending.
    /// </remarks>
    [PublicAPI]
    public sealed class Future<TValue> : Disposable, IAwaitable<TValue>
    {
        const string C_Exists = @"Future already exists.";
        const string C_DoesNotExist = @"Future does not exist.";

        [NotNull]
        readonly TaskCompletionSource<TValue> FSource = new TaskCompletionSource<TValue>();

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                // If the future does not exist yet, fault it.
                FSource.TrySetException(new ObjectDisposedException(typeof(Era).Name));
            }

            base.Dispose(ADisposing);
        }
        #endregion

        /// <summary>
        /// Attempt to post the value.
        /// </summary>
        /// <param name="AValue">The value.</param>
        /// <returns>
        /// <see langword="true"/> if the value was posted; <see langword="false"/> if the future
        /// already exists or is disposed.
        /// </returns>
        public bool TryPost(TValue AValue) => FSource.TrySetResult(AValue);
        /// <summary>
        /// Post the value.
        /// </summary>
        /// <param name="AValue">The value.</param>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        /// <exception cref="InvalidOperationException">The future already exists.</exception>
        /// <remarks>
        /// Use this method in contexts where there is no contention on the future.
        /// </remarks>
        public void Post(TValue AValue)
        {
            if (TryPost(AValue)) return;

            ThrowIfDisposed();
            throw new InvalidOperationException(C_Exists);
        }

        /// <summary>
        /// Attempt to retrieve the value.
        /// </summary>
        /// <returns>
        /// A present <see cref="Optional{T}"/> if the future exists; otherwise absent.
        /// </returns>
        [Pure]
        public Optional<TValue> TryGet()
            => !IsDisposed && FSource.Task.IsCompleted
                ? Optional.Present(FSource.Task.Result)
                : Optional.Absent<TValue>();
        /// <summary>
        /// Retrieve the value.
        /// </summary>
        /// <returns>The value.</returns>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        /// <exception cref="InvalidOperationException">The future does not exist.</exception>
        [Pure]
        public TValue Get()
        {
            var value = TryGet();
            if (value.IsPresent)
            {
                return value.Value;
            }

            ThrowIfDisposed();
            throw new InvalidOperationException(C_DoesNotExist);
        }

        #region IAwaitable<TValue>
        /// <inheritdoc />
        public Task<TValue> WaitAsync(CancellationToken ACancel)
            => FSource.Task.OrCanceledBy(ACancel);
        #endregion

        /// <summary>
        /// Get a value indicating whether the value exists.
        /// </summary>
        public bool Exists => FSource.Task.Status == TaskStatus.RanToCompletion;
    }
}
