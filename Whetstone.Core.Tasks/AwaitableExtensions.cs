using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Provides some extension methods for use with the <see cref="IAwaitable"/> and
    /// <see cref="IAwaitable"/> types.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static class AwaitableExtensions
    {
        /// <summary>
        /// Synchronously wait for the event.
        /// </summary>
        /// <param name="AAwaitable">The <see cref="IAwaitable"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AAwaitable"/> is <see langword="null"/>.
        /// </exception>
        public static void Wait([NotNull] this IAwaitable AAwaitable)
        {
            Require.NotNull(AAwaitable, nameof(AAwaitable));

            AAwaitable.Wait(CancellationToken.None);
        }

        /// <summary>
        /// Synchronously wait for the event.
        /// </summary>
        /// <param name="AAwaitable">The <see cref="IAwaitable"/>.</param>
        /// <param name="ACancel">A <see cref="CancellationToken"/> to cancel the wait.</param>
        /// <exception cref="OperationCanceledException">The wait was canceled.</exception>
        /// <exception cref="ObjectDisposedException">
        /// The <see cref="CancellationTokenSource"/> of <paramref name="ACancel"/> is disposed.
        /// </exception>
        /// <remarks>
        /// If required, internal exceptions that were wrapped in a <see cref="AggregateException"/>
        /// are unwrapped and rethrown.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AAwaitable"/> is <see langword="null"/>.
        /// </exception>
        public static void Wait([NotNull] this IAwaitable AAwaitable, CancellationToken ACancel)
        {
            Require.NotNull(AAwaitable, nameof(AAwaitable));

            try
            {
                AAwaitable.WaitAsync(ACancel).Wait(ACancel);
            }
            catch (AggregateException error)
            {
                // Throw the first aggregated exception.
                throw error.InnerException ?? error;
            }
        }

        /// <summary>
        /// Wait for the event.
        /// </summary>
        /// <param name="AAwaitable">The <see cref="IAwaitable"/>.</param>
        /// <returns>An awaitable <see cref="Task"/> that waits for the result.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AAwaitable"/> is <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static Task WaitAsync([NotNull] this IAwaitable AAwaitable)
        {
            Require.NotNull(AAwaitable, nameof(AAwaitable));

            return AAwaitable.WaitAsync(CancellationToken.None);
        }

        /// <summary>
        /// Synchronously wait for the result.
        /// </summary>
        /// <typeparam name="TResult">The awaitable result type.</typeparam>
        /// <param name="AAwaitable">The <see cref="IAwaitable{TResult}"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AAwaitable"/> is <see langword="null"/>.
        /// </exception>
        public static TResult Wait<TResult>([NotNull] this IAwaitable<TResult> AAwaitable)
        {
            Require.NotNull(AAwaitable, nameof(AAwaitable));

            return AAwaitable.Wait(CancellationToken.None);
        }

        /// <summary>
        /// Synchronously wait for the result.
        /// </summary>
        /// <typeparam name="TResult">The awaitable result type.</typeparam>
        /// <param name="AAwaitable">The <see cref="IAwaitable{TResult}"/>.</param>
        /// <param name="ACancel">A <see cref="CancellationToken"/> to cancel the wait.</param>
        /// <exception cref="OperationCanceledException">The wait was canceled.</exception>
        /// <exception cref="ObjectDisposedException">
        /// The <see cref="CancellationTokenSource"/> of <paramref name="ACancel"/> is disposed.
        /// </exception>
        /// <remarks>
        /// If required, internal exceptions that were wrapped in a <see cref="AggregateException"/>
        /// are unwrapped and rethrown.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AAwaitable"/> is <see langword="null"/>.
        /// </exception>
        public static TResult Wait<TResult>(
            [NotNull] this IAwaitable<TResult> AAwaitable,
            CancellationToken ACancel
        )
        {
            Require.NotNull(AAwaitable, nameof(AAwaitable));

            try
            {
                var task = AAwaitable.WaitAsync(ACancel);
                task.Wait(ACancel);
                return task.Result;
            }
            catch (AggregateException error)
            {
                // Throw the first aggregated exception.
                throw error.InnerException ?? error;
            }
        }

        /// <summary>
        /// Wait for the result.
        /// </summary>
        /// <typeparam name="TResult">The awaitable result type.</typeparam>
        /// <param name="AAwaitable">The <see cref="IAwaitable{TResult}"/>.</param>
        /// <returns>An awaitable <see cref="Task{TResult}"/> that waits for the result.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AAwaitable"/> is <see langword="null"/>.
        /// </exception>
        [NotNull]
        public static Task<TResult> WaitAsync<TResult>(
            [NotNull] this IAwaitable<TResult> AAwaitable
        )
        {
            Require.NotNull(AAwaitable, nameof(AAwaitable));

            return AAwaitable.WaitAsync(CancellationToken.None);
        }

        /// <summary>
        /// Get an awaiter for this <see cref="IAwaitable"/>.
        /// </summary>
        /// <param name="AAwaitable">The <see cref="IAwaitable"/>.</param>
        /// <returns>An awaiter for <paramref name="AAwaitable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AAwaitable"/> is <see langword="null"/>.
        /// </exception>
        public static TaskAwaiter GetAwaiter([NotNull] this IAwaitable AAwaitable)
        {
            Require.NotNull(AAwaitable, nameof(AAwaitable));

            return AAwaitable.WaitAsync().GetAwaiter();
        }

        /// <summary>
        /// Get an awaiter for this <see cref="IAwaitable{TResult}"/>.
        /// </summary>
        /// <typeparam name="TResult">The awaitable result type.</typeparam>
        /// <param name="AAwaitable">The <see cref="IAwaitable{TResult}"/>.</param>
        /// <returns>An awaiter for <paramref name="AAwaitable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AAwaitable"/> is <see langword="null"/>.
        /// </exception>
        public static TaskAwaiter<TResult> GetAwaiter<TResult>(
            [NotNull] this IAwaitable<TResult> AAwaitable
        )
        {
            Require.NotNull(AAwaitable, nameof(AAwaitable));

            return AAwaitable.WaitAsync().GetAwaiter();
        }
    }
}