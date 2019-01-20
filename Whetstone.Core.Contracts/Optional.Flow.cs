using System;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Provides declarative-flow extension methods for the <see cref="Optional"/> type.
    /// </summary>
    [PublicAPI]
    public static class OptionalFlow
    {
        /// <summary>
        /// Check whether the present value matches a <see cref="Predicate{T}"/>.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="AOptional">The <see cref="Optional{T}"/>.</param>
        /// <param name="APredicate">The <see cref="Predicate{T}"/>.</param>
        /// <returns>
        /// An absent <see cref="Optional{T}"/> if the value is absent or mismatching; otherwise
        /// <paramref name="AOptional"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="APredicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// <paramref name="APredicate"/> threw an exception.
        /// </exception>
        [MustUseReturnValue]
        [Pure]
        public static Optional<T> That<T>(
            in this Optional<T> AOptional,
            [NotNull] [InstantHandle] Predicate<T> APredicate
        )
        {
            Require.NotNull(APredicate, nameof(APredicate));

            return AOptional.IsPresent && APredicate(AOptional.Unpack)
                ? AOptional
                : Optional.Absent<T>();
        }

        /// <summary>
        /// Execute an <see cref="Action{T}"/> on the present value.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="AOptional">The <see cref="Optional{T}"/>.</param>
        /// <param name="AAction">The <see cref="Action{T}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="AAction"/> was invoked; otherwise
        /// <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AAction"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception"><paramref name="AAction"/> threw an exception.</exception>
        public static bool Forward<T>(
            in this Optional<T> AOptional,
            [NotNull] [InstantHandle] Action<T> AAction
        )
        {
            Require.NotNull(AAction, nameof(AAction));

            if (AOptional.IsPresent)
            {
                AAction(AOptional.Unpack);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Map the present value using a <see cref="Func{T, TResult}"/>.
        /// </summary>
        /// <typeparam name="TIn">The input type.</typeparam>
        /// <typeparam name="TOut">The output type.</typeparam>
        /// <param name="AOptional">The <see cref="Optional{T}"/>.</param>
        /// <param name="AFunc">The <see cref="Func{T, TResult}"/>.</param>
        /// <returns>
        /// An absent <see cref="Optional{T}"/> if the value is absent; otherwise a present
        /// <see cref="Optional{T}"/> containing the result of applying <paramref name="AFunc"/> to
        /// the present value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AFunc"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception"><paramref name="AFunc"/> threw an exception.</exception>
        [MustUseReturnValue]
        [Pure]
        public static Optional<TOut> Map<TIn, TOut>(
            in this Optional<TIn> AOptional,
            [NotNull] [InstantHandle] Func<TIn, TOut> AFunc
        )
        {
            Require.NotNull(AFunc, nameof(AFunc));

            return AOptional.IsPresent
                ? Optional.Present(AFunc(AOptional.Unpack))
                : Optional.Absent<TOut>();
        }

        /// <summary>
        /// Get the present value or a default.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="AOptional">The <see cref="Optional{T}"/>.</param>
        /// <param name="ADefault">The default value.</param>
        /// <returns>
        /// <paramref name="ADefault"/> if the value is absent; otherwise the present value.
        /// </returns>
        [Pure]
        public static T OrDefault<T>(in this Optional<T> AOptional, T ADefault = default)
            => AOptional.IsPresent ? AOptional.Unpack : ADefault;
    }
}
