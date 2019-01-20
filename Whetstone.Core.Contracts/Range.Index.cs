using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial class Range
    {
        /// <summary>
        /// Initialize a [0, count) <see cref="int"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ACount">The exclusive upper limit.</param>
        /// <returns>The [0, <paramref name="ACount"/>) <see cref="Range{T}"/>.</returns>
        [Pure]
        public static Range<int> Indices(this int ACount)
            => Of(0, true, ACount, false);

        /// <summary>
        /// Initialize an index range for characters in a <see cref="string"/>.
        /// </summary>
        /// <param name="AString">The <see cref="string"/>.</param>
        /// <returns>The [0, <paramref name="AString"/> length) <see cref="Range{T}"/>.</returns>
        /// <remarks>
        /// If <paramref name="AString"/> is <see langword="null"/>, it is assumed to be empty.
        /// </remarks>
        [Pure]
        public static Range<int> Indices([CanBeNull] this string AString)
            => Indices(AString?.Length ?? 0);

        /// <summary>
        /// Initialize an index range for items in a 1D <see cref="Array"/>.
        /// </summary>
        /// <typeparam name="T">The array item type.</typeparam>
        /// <param name="AArray">The 1D <see cref="Array"/>.</param>
        /// <returns>The [0, <paramref name="AArray"/> length) <see cref="Range{T}"/>.</returns>
        /// <exception cref="OverflowException">
        /// The length of <paramref name="AArray"/> exceeds <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// If <paramref name="AArray"/> is <see langword="null"/>, it is assumed to be empty.
        /// </para>
        /// <para>
        /// Consider using <see cref="LongIndices{T}(T[])"/> if <paramref name="AArray"/> is
        /// expected to have lengths exceeding <see cref="int.MaxValue"/>.
        /// </para>
        /// </remarks>
        [Pure]
        public static Range<int> Indices<T>([CanBeNull] this T[] AArray)
            => Indices(AArray?.Length ?? 0);

        /// <summary>
        /// Initialize an index range for items in a <see cref="ICollection"/>.
        /// </summary>
        /// <param name="ACollection">The <see cref="ICollection"/>.</param>
        /// <returns>
        /// The [0, <paramref name="ACollection"/> length) <see cref="Range{T}"/>.
        /// </returns>
        /// <remarks>
        /// If <paramref name="ACollection"/> is <see langword="null"/>, it is assumed to be empty.
        /// </remarks>
        [Pure]
        public static Range<int> Indices([CanBeNull] this ICollection ACollection)
            => Indices(ACollection?.Count ?? 0);

        /// <summary>
        /// Initialize an index range for items in a <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The collection item type.</typeparam>
        /// <param name="ACollection">The <see cref="ICollection{T}"/>.</param>
        /// <returns>
        /// The [0, <paramref name="ACollection"/> length) <see cref="Range{T}"/>.
        /// </returns>
        /// <remarks>
        /// If <paramref name="ACollection"/> is <see langword="null"/>, it is assumed to be empty.
        /// </remarks>
        [Pure]
        public static Range<int> Indices<T>([CanBeNull] this ICollection<T> ACollection)
            => Indices(ACollection?.Count ?? 0);

        /// <summary>
        /// Initialize a [0, count) <see cref="long"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ACount">The exclusive upper limit.</param>
        /// <returns>The [0, <paramref name="ACount"/>) <see cref="Range{T}"/>.</returns>
        [Pure]
        public static Range<long> LongIndices(this long ACount)
            => Of(0L, true, ACount, false);

        /// <summary>
        /// Initialize an index range for items in a 1D <see cref="Array"/>.
        /// </summary>
        /// <typeparam name="T">The array item type.</typeparam>
        /// <param name="AArray">The 1D <see cref="Array"/>.</param>
        /// <returns>The [0, <paramref name="AArray"/> length) <see cref="Range{T}"/>.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="AArray"/> is <see langword="null"/>, it is assumed to be empty.
        /// </para>
        /// <para>
        /// Consider using <see cref="Indices{T}(T[])"/> if <paramref name="AArray"/> is sure to
        /// have lengths not exceeding <see cref="int.MaxValue"/>.
        /// </para>
        /// </remarks>
        [Pure]
        public static Range<long> LongIndices<T>([CanBeNull] this T[] AArray)
            => LongIndices(AArray?.LongLength ?? 0L);
    }
}
