using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial class Range
    {
        #region Int32 Span
        /// <summary>
        /// Initialize a [offset, offset + length) <see cref="int"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="AOffset">The start offset.</param>
        /// <param name="ALength">The span length.</param>
        /// <returns>
        /// The [<paramref name="AOffset"/>, <paramref name="AOffset"/> + <paramref name="ALength"/>)
        /// <see cref="Range{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALength"/> is negative.
        /// </exception>
        [Pure]
        public static Range<int> Span(in int AOffset, in int ALength)
        {
            Require.NotNegative(AOffset, nameof(AOffset));
            Require.NotNegative(ALength, nameof(ALength));

            return Of(AOffset, true, AOffset + ALength, false);
        }

        /// <summary>
        /// Get the lowest index in the <see cref="int"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ASpan">The <see cref="Range{T}"/>.</param>
        /// <returns>The lowest index in the span.</returns>
        [Pure]
        public static int Offset(in this Range<int> ASpan)
            => ASpan.Lower + (ASpan.IncludesLower ? 0 : 1);

        /// <summary>
        /// Get the smallest index greater than the <see cref="int"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ASpan">The <see cref="Range{T}"/>.</param>
        /// <returns>The highest index in the span + 1.</returns>
        [Pure]
        public static int Limit(in this Range<int> ASpan)
            => ASpan.Upper + (ASpan.IncludesUpper ? 1 : 0);

        /// <summary>
        /// Get the length of the <see cref="int"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ASpan">The <see cref="Range{T}"/>.</param>
        /// <returns>The length of the span.</returns>
        [Pure]
        public static int Length(in this Range<int> ASpan)
            => ASpan.Limit() - ASpan.Offset();
        #endregion

        #region Int64 Span
        /// <summary>
        /// Initialize a [offset, offset + length) <see cref="long"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="AOffset">The start offset.</param>
        /// <param name="ALength">The span length.</param>
        /// <returns>
        /// The [<paramref name="AOffset"/>, <paramref name="AOffset"/> + <paramref name="ALength"/>)
        /// <see cref="Range{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALength"/> is negative.
        /// </exception>
        [Pure]
        public static Range<long> Span(in long AOffset, in long ALength)
        {
            Require.NotNegative(AOffset, nameof(AOffset));
            Require.NotNegative(ALength, nameof(ALength));

            return Of(AOffset, true, AOffset + ALength, false);
        }

        /// <summary>
        /// Get the lowest index in the <see cref="long"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ASpan">The <see cref="Range{T}"/>.</param>
        /// <returns>The lowest index in the span.</returns>
        [Pure]
        public static long Offset(in this Range<long> ASpan)
            => ASpan.Lower + (ASpan.IncludesLower ? 0L : 1L);

        /// <summary>
        /// Get the smallest index greater than the <see cref="long"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ASpan">The <see cref="Range{T}"/>.</param>
        /// <returns>The highest index in the span + 1.</returns>
        [Pure]
        public static long Limit(in this Range<long> ASpan)
            => ASpan.Upper + (ASpan.IncludesUpper ? 1L : 0L);

        /// <summary>
        /// Get the length of the <see cref="long"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ASpan">The <see cref="Range{T}"/>.</param>
        /// <returns>The length of the span.</returns>
        [Pure]
        public static long Length(in this Range<long> ASpan)
            => ASpan.Limit() - ASpan.Offset();
        #endregion

        #region Int32 Index ranges
        /// <summary>
        /// Initialize a [0, count) <see cref="int"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ACount">The exclusive upper limit.</param>
        /// <returns>The [0, <paramref name="ACount"/>) <see cref="Range{T}"/>.</returns>
        [Pure]
        public static Range<int> Indices(in this int ACount)
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
        #endregion

        #region Int64 Index ranges
        /// <summary>
        /// Initialize a [0, count) <see cref="long"/> index <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ACount">The exclusive upper limit.</param>
        /// <returns>The [0, <paramref name="ACount"/>) <see cref="Range{T}"/>.</returns>
        [Pure]
        public static Range<long> LongIndices(in this long ACount)
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
        #endregion
    }
}
