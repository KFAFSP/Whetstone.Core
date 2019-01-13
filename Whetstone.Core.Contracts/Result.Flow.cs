using System;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Provides declarative-flow extension methods for the <see cref="Result"/> and
    /// <see cref="Result{T}"/> type.
    /// </summary>
    [PublicAPI]
    public static class ResultFlow
    {
        /// <summary>
        /// Continue after a successful <see cref="Result"/>.
        /// </summary>
        /// <typeparam name="TOut">The output value type.</typeparam>
        /// <param name="AResult">The <see cref="Result"/>.</param>
        /// <param name="AContinuation">The continuation <see cref="Func{TResult}"/>.</param>
        /// <returns>
        /// <paramref name="AResult"/> if it is erroneous, otherwise a <see cref="Result"/> wrapping
        /// the result of <paramref name="AContinuation"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AContinuation"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// <paramref name="AContinuation"/> threw an exception.
        /// </exception>
        [MustUseReturnValue]
        public static Result<TOut> AndThen<TOut>(
            this Result AResult,
            [NotNull] [InstantHandle] Func<TOut> AContinuation
        )
        {
            Require.NotNull(AContinuation, nameof(AContinuation));

            return AResult.IsSuccess
                ? Result.Ok(AContinuation())
                : Result.Fail<TOut>(AResult.UnpackError);
        }
        /// <summary>
        /// Continue after a successful <see cref="Result{T}"/>.
        /// </summary>
        /// <typeparam name="TIn">The input value type.</typeparam>
        /// <typeparam name="TOut">The output value type.</typeparam>
        /// <param name="AResult">The <see cref="Result{T}"/>.</param>
        /// <param name="AContinuation">The continuation <see cref="Func{T, TResult}"/>.</param>
        /// <returns>
        /// <paramref name="AResult"/> if it is erroneous, otherwise a <see cref="Result{T}"/>
        /// wrapping the result of applying <paramref name="AContinuation"/> to the successful
        /// result value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AContinuation"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// <paramref name="AContinuation"/> threw an exception.
        /// </exception>
        [MustUseReturnValue]
        public static Result<TOut> AndThen<TIn, TOut>(
            this Result<TIn> AResult,
            [NotNull] [InstantHandle] Func<TIn, TOut> AContinuation
        )
        {
            Require.NotNull(AContinuation, nameof(AContinuation));

            return AResult.IsSuccess
                ? Result.Ok(AContinuation(AResult.UnpackValue))
                : Result.Fail<TOut>(AResult.UnpackError);
        }

        /// <summary>
        /// Try continuing after a successful <see cref="Result"/>.
        /// </summary>
        /// <param name="AResult">The <see cref="Result"/>.</param>
        /// <param name="AContinuation">The continuation <see cref="Action"/>.</param>
        /// <returns>
        /// <paramref name="AResult"/> if it is erroneous, an erroneous <see cref="Result"/>
        /// wrapping the <see cref="Exception"/> thrown by <paramref name="AContinuation"/> or
        /// <see cref="Result.Ok"/> if no error happened.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AContinuation"/> is <see langword="null"/>.
        /// </exception>
        [MustUseReturnValue]
        public static Result AndThenTry(
            this Result AResult,
            [NotNull] [InstantHandle] Action AContinuation
        )
        {
            Require.NotNull(AContinuation, nameof(AContinuation));

            if (AResult.IsSuccess)
            {
                try
                {
                    AContinuation();
                }
                catch (Exception error)
                {
                    return Result.Fail(error);
                }
            }

            return AResult;
        }
        /// <summary>
        /// Try continuing after a successful <see cref="Result{T}"/>.
        /// </summary>
        /// <typeparam name="TOut">The output value type.</typeparam>
        /// <param name="AResult">The <see cref="Result{T}"/>.</param>
        /// <param name="AContinuation">The continuation <see cref="Func{TResult}"/>.</param>
        /// <returns>
        /// <paramref name="AResult"/> if it is erroneous, an erroneous <see cref="Result"/>
        /// wrapping the <see cref="Exception"/> thrown by <paramref name="AContinuation"/> or a
        /// successful <see cref="Result{T}"/> wrapping it's return value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AContinuation"/> is <see langword="null"/>.
        /// </exception>
        [MustUseReturnValue]
        public static Result<TOut> AndThenTry<TOut>(
            this Result AResult,
            [NotNull] [InstantHandle] Func<TOut> AContinuation
        )
        {
            Require.NotNull(AContinuation, nameof(AContinuation));

            if (!AResult.IsSuccess)
            {
                return Result.Fail<TOut>(AResult.UnpackError);
            }

            try
            {
                return Result.Ok(AContinuation());
            }
            catch (Exception error)
            {
                return Result.Fail<TOut>(error);
            }
        }
        /// <summary>
        /// Try continuing after a successful <see cref="Result{T}"/>.
        /// </summary>
        /// <typeparam name="TIn">The input value type.</typeparam>
        /// <typeparam name="TOut">The output value type.</typeparam>
        /// <param name="AResult">The <see cref="Result{T}"/>.</param>
        /// <param name="AContinuation">The continuation <see cref="Func{T, TResult}"/>.</param>
        /// <returns>
        /// <paramref name="AResult"/> if it is erroneous, an erroneous <see cref="Result"/>
        /// wrapping the <see cref="Exception"/> thrown by <paramref name="AContinuation"/> or a
        /// successful <see cref="Result{T}"/> wrapping it's return value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AContinuation"/> is <see langword="null"/>.
        /// </exception>
        [MustUseReturnValue]
        public static Result<TOut> AndThenTry<TOut, TIn>(
            this Result<TIn> AResult,
            [NotNull] [InstantHandle] Func<TIn, TOut> AContinuation
        )
        {
            Require.NotNull(AContinuation, nameof(AContinuation));

            if (!AResult.IsSuccess)
            {
                return Result.Fail<TOut>(AResult.UnpackError);
            }

            try
            {
                return Result.Ok(AContinuation(AResult.UnpackValue));
            }
            catch (Exception error)
            {
                return Result.Fail<TOut>(error);
            }
        }
    }
}
