using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class WeakOrdering
    {
        /// <summary>
        /// Get the minimum of two instances of a comparable type.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="ALhs">The left hand side operand.</param>
        /// <param name="ARhs">The right hand side operand.</param>
        /// <returns>
        /// <paramref name="ALhs"/> if it is less than or equal to <paramref name="ARhs"/>;
        /// otherwise <paramref name="ARhs"/>.
        /// </returns>
        /// <remarks>
        /// This implementation has a left-bias, which means that in the event of a tie it will
        /// pick <paramref name="ALhs"/> over <paramref name="ARhs"/>. Depending on the
        /// implementation of <typeparamref name="T"/> this might be semantically different from
        /// a right-bias, but as the weak-ordering is concerned both are the same.
        /// </remarks>
        [Pure]
		[CanBeNull]
        public static T Min<T>([CanBeNull] T ALhs, [CanBeNull] T ARhs)
		    where T : IComparable<T>
        {
            // Special case: reference operand nullity.
            if (!typeof(T).IsValueType && ReferenceEquals(ALhs, null))
            {
                // See Compare(T, T) for more details.

				// NOTE: ALhs is the "least T".
                // ReSharper disable once ExpressionIsAlwaysNull
                return ALhs;
            }

            // NOTE: Because of our nullity handling, ALhs is guaranteed to be not-null.
            // ReSharper disable once PossibleNullReferenceException
            return ALhs.CompareTo(ARhs) <= 0 ? ALhs : ARhs;
        }

        /// <summary>
        /// Find the index of the least value in an array of instances of a comparable type.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="AItems">The items.</param>
        /// <returns>
        /// The index of the least value in <paramref name="AItems"/>; or <c>-1</c> if
        /// <paramref name="AItems"/> is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AItems"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// This implementation is a slightly more optimized version of performing a linear search
        /// on <paramref name="AItems"/> since it handles quick-exit on reaching
        /// <see langword="null"/> for reference type <typeparamref name="T"/>s.
        /// </remarks>
		[Pure]
		[ContractAnnotation("AItems: null => halt")]
        public static int MinIndex<T>([NotNull] [ItemCanBeNull] params T[] AItems)
            where T : IComparable<T>
        {
            Require.NotNull(AItems, nameof(AItems));

            // Quick exit: AItems is empty.
            if (AItems.Length == 0)
            {
                // AItems is empty.
                return -1;
            }

            // Initialize the result with the first element.
            var min = 0;

            // Branching path: value types are not nullable.
            if (typeof(T).IsValueType)
            {
                // Find the minimum using linear search.
                for (var I = 1; I < AItems.Length; ++I)
                {
                    // NOTE: Since T is a value type result is guaranteed to be not-null.
                    // ReSharper disable once PossibleNullReferenceException
                    if (AItems[I].CompareTo(AItems[min]) < 0)
                    {
						// New champion.
                        min = I;
                    }
                }
            }
            else
            {
                if (ReferenceEquals(AItems[min], null))
                {
                    // We already reached a "least T".
                    return min;
                }

                // Find the minimum using linear search.
                for (var I = 1; I < AItems.Length; ++I)
                {
                    if (ReferenceEquals(AItems[I], null))
                    {
						// We found a "least T".
                        return I;
                    }

                    if (AItems[I].CompareTo(AItems[min]) < 0)
                    {
						// New champion.
                        min = I;
                    }
                }
            }

            return min;
        }

        /// <summary>
        /// Get the minimum of an <see cref="IEnumerable{T}"/> of instances of a comparable type.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="AOperands">The operands.</param>
        /// <returns>The first, least operand in <paramref name="AOperands"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AOperands"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="AOperands"/> is empty.</exception>
        /// <remarks>
        /// This implementation is a slightly more optimized version of folding
        /// <paramref name="AOperands"/> with <see cref="Min{T}(T, T)"/> since it handles quick-exit
        /// on reaching <see langword="null"/> for reference type <typeparamref name="T"/>s.
        /// </remarks>
        [Pure]
        [CanBeNull]
        [ContractAnnotation("AOperands: null => halt")]
        public static T Min<T>([NotNull] [ItemCanBeNull] IEnumerable<T> AOperands)
            where T : IComparable<T>
        {
            Require.NotNull(AOperands, nameof(AOperands));

            using (var enumerator = AOperands.GetEnumerator())
            {
				// Quick exit: nothing to compare.
                if (!enumerator.MoveNext())
                {
					// Source enumeration is empty.
                    throw new ArgumentException("Enumeration is empty.", nameof(AOperands));
                }

				// Initialize the result with the first element.
                var result = enumerator.Current;

				// Branching path: value types are not nullable.
                if (typeof(T).IsValueType)
                {
					// Fold result with all operands.
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        // NOTE: Since T is a value type result is guaranteed to be not-null.
                        // ReSharper disable once PossibleNullReferenceException
                        result = result.CompareTo(current) <= 0
                            ? result
                            : current;
                    }
                }
                else
                {
					// Fold result with all operands as long as it is not-null.
                    while (!ReferenceEquals(result, null) && enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        result = result.CompareTo(current) <= 0
                            ? result
                            : current;
                    }
                }

                return result;
            }
        }
        /// <summary>
        /// Get the minimum of an array of instances of a comparable type.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="AItems">The array of items.</param>
        /// <returns>The first, least item in <paramref name="AItems"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AItems"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="AItems"/> is empty.</exception>
        /// <remarks>
        /// Uses the <see cref="MinIndex{T}(T[])"/> method to find the minimum using an optimized
        /// linear search.
        /// </remarks>
        [Pure]
		[ContractAnnotation("AItems: null => halt")]
        public static T Min<T>([NotNull] [ItemCanBeNull] params T[] AItems)
		    where T : IComparable<T>
        {
            Require.NotNull(AItems, nameof(AItems));

            var index = MinIndex(AItems);
            return index >= 0
                ? AItems[index]
                : throw new ArgumentException("Array is empty.", nameof(AItems));
        }
    }
}
