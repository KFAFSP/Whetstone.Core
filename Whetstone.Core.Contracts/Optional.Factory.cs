using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class Optional
    {
        /// <summary>
        /// Get an absent <see cref="Optional{T}"/>.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <returns>An absent <see cref="Optional{T}"/>.</returns>
        [ExcludeFromCodeCoverage]
        [Pure]
        public static Optional<T> Absent<T>() => Optional<T>.Absent;

        /// <summary>
        /// Get a present <see cref="Optional{T}"/>.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="AValue">The present value.</param>
        /// <returns>
        /// A present <see cref="Optional{T}"/> with value <paramref name="AValue"/>.
        /// </returns>
        [ExcludeFromCodeCoverage]
        [Pure]
        public static Optional<T> Present<T>([NoEnumeration] T AValue)
            => Optional<T>.Present(AValue);

        /// <summary>
        /// Get an <see cref="Optional{T}"/> that is present on non-<see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="AValue">The value.</param>
        /// <returns>
        /// A present <see cref="Optional{T}"/> with value <paramref name="AValue"/> if it is
        /// non-<see langword="null"/>; otherwise <see cref="Optional{T}.Absent"/>.
        /// </returns>
        [ExcludeFromCodeCoverage]
        [Pure]
        public static Optional<T> IfNotNull<T>([NoEnumeration] this T AValue) where T : class
            => ReferenceEquals(AValue, null) ? Optional<T>.Absent : Optional<T>.Present(AValue);

        [ExcludeFromCodeCoverage]
        [Pure]
        public static Optional<T> If<T>(
            this T AValue,
            [InstantHandle] [NotNull] Predicate<T> APredicate
        ) => Present(AValue).That(APredicate);

        /// <summary>
        /// Try to perform a cast on a value.
        /// </summary>
        /// <typeparam name="T">The cast target type.</typeparam>
        /// <returns>
        /// An absent <see cref="Optional{T}"/> if the value is mismatching; otherwise a present
        /// <see cref="Optional{T}"/> with the casted non-<see langword="null"/> result.
        /// </returns>
        [ExcludeFromCodeCoverage]
        [Pure]
        public static Optional<T> IfIs<T>(this object AValue) => Present(AValue).ThatIs<T>();

        /// <summary>
        /// Get an <see cref="Optional{T}"/> containing the first value in an
        /// <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="AEnumerable">The <see cref="IEnumerable{T}"/>.</param>
        /// <returns>
        /// An absent <see cref="Optional{T}"/> if <paramref name="AEnumerable"/> is empty;
        /// otherwise a present <see cref="Optional{T}"/> that contains the first value in
        /// <paramref name="AEnumerable"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AEnumerable"/> is <see langword="null"/>.
        /// </exception>
        [MustUseReturnValue]
        public static Optional<T> IfAny<T>(
            [InstantHandle] [NotNull] this IEnumerable<T> AEnumerable
        )
        {
            Require.NotNull(AEnumerable, nameof(AEnumerable));

            using (var enumerator = AEnumerable.GetEnumerator())
            {
                return enumerator.MoveNext()
                    ? Present(enumerator.Current)
                    : Absent<T>();
            }
        }
    }
}
