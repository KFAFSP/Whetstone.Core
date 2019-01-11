using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Provides factory extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    [PublicAPI]
    public static class EnumerableFactory
    {
        /// <summary>
        /// Return an <see cref="IEnumerable{T}"/> that yields the specified item.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="AItem">The item to yield.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that yields <paramref name="AItem"/> exactly once.
        /// </returns>
        [NotNull]
        [ItemCanBeNull]
        [ExcludeFromCodeCoverage]
        public static IEnumerable<T> Yield<T>([CanBeNull] [NoEnumeration] this T AItem)
        {
            yield return AItem;
        }

        /// <summary>
        /// Return an <see cref="IEnumerable{T}"/> that repeats the specified item indefinitely.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="AItem">The item to repeat.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that yields <paramref name="AItem"/> indefinitely.
        /// </returns>
        [ExcludeFromCodeCoverage]
        public static IEnumerable<T> Repeat<T>([CanBeNull] [NoEnumeration] this T AItem)
        {
            while (true)
            {
                yield return AItem;
            }
            // NOTE: This is intended behavior.
            // ReSharper disable once IteratorNeverReturns
        }

        /// <summary>
        /// Return an <see cref="IEnumerable{T}"/> that repeats the specified item.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="AItem">The item to repeat.</param>
        /// <param name="ACount">The number of repetitions.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that yields <paramref name="AItem"/> exactly
        /// <paramref name="ACount"/> times.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="ACount"/> is less than 0.
        /// </exception>
        [NotNull]
        [ItemCanBeNull]
        [ExcludeFromCodeCoverage]
        public static IEnumerable<T> Repeat<T>([CanBeNull] [NoEnumeration] this T AItem, int ACount)
            => Enumerable.Repeat(AItem, ACount);

        /// <summary>
        /// Return an <see cref="IEnumerable{T}"/> that repeats an item while a condition is
        /// <see langword="true"/>.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="AItem">The item to repeat.</param>
        /// <param name="ACondition">The condition to evaluate.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that yields <paramref name="AItem"/> while
        /// <paramref name="ACondition"/> evaluates to true.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ACondition"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// <see cref="Exception"/>s thrown by <paramref name="ACondition"/> are propagated to the
        /// caller.
        /// </remarks>
        [NotNull]
        [ItemCanBeNull]
        public static IEnumerable<T> RepeatWhile<T>(
            [CanBeNull] [NoEnumeration] this T AItem,
            [NotNull] [InstantHandle] Func<bool> ACondition
        )
        {
            Require.NotNull(ACondition, nameof(ACondition));

            // NOTE: Required, so that the above statement is always evaluated.
            IEnumerable<T> Generator()
            {
                while (ACondition())
                {
                    yield return AItem;
                }
            }

            return Generator();
        }

        /// <summary>
        /// Return an <see cref="IEnumerable{T}"/> that repeats an item until a condition is
        /// <see langword="true"/>.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="AItem">The item to repeat.</param>
        /// <param name="ACondition">The condition to evaluate.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that yields <paramref name="AItem"/> until
        /// <paramref name="ACondition"/> evaluates to true, but at least once.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ACondition"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// <see cref="Exception"/>s thrown by <paramref name="ACondition"/> are propagated to the
        /// caller.
        /// </remarks>
        [NotNull]
        [ItemCanBeNull]
        public static IEnumerable<T> RepeatUntil<T>(
            [CanBeNull] [NoEnumeration] this T AItem,
            [NotNull] [InstantHandle] Func<bool> ACondition
        )
        {
            Require.NotNull(ACondition, nameof(ACondition));

            // NOTE: Required, so that the above statement is always evaluated.
            IEnumerable<T> Generator()
            {
                do
                {
                    yield return AItem;
                } while (!ACondition());
            }

            return Generator();
        }
    }
}
