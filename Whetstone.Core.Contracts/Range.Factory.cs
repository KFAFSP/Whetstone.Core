﻿using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class Range
    {
        /// <inheritdoc cref="Range{T}.Of(in T, bool, in T, bool)"/>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        [ExcludeFromCodeCoverage]
        [Pure]
        public static Range<T> Of<T>(
            [CanBeNull] in T ALower,
            bool AIncludesLower,
            [CanBeNull] in T AUpper,
            bool AIncludesUpper
        ) where T : IComparable<T>
            => Range<T>.Of(ALower, AIncludesLower, AUpper, AIncludesUpper);

        /// <summary>
        /// Make a new <see cref="Range{T}"/> with a different lower bound.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="ALower">The new lower bound.</param>
        /// <returns>A copy of <paramref name="ARange"/> with a different lower bound.</returns>
        [Pure]
        public static Range<T> WithLower<T>(in this Range<T> ARange, [CanBeNull] in T ALower)
            where T : IComparable<T>
            => Range<T>.Of(ALower, ARange.IncludesLower, ARange.Upper, ARange.IncludesUpper);
        /// <summary>
        /// Make a new <see cref="Range{T}"/> with a different lower bound.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AIncludesLower">Whether the lower bound is included.</param>
        /// <returns>A copy of <paramref name="ARange"/> with a different lower bound.</returns>
        [Pure]
        public static Range<T> WithLower<T>(in this Range<T> ARange, bool AIncludesLower)
            where T : IComparable<T>
            => Range<T>.Of(ARange.Lower, AIncludesLower, ARange.Upper, ARange.IncludesUpper);
        /// <summary>
        /// Make a new <see cref="Range{T}"/> with a different lower bound.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="ALower">The new lower bound.</param>
        /// <param name="AIncludesLower">Whether the lower bound is included.</param>
        /// <returns>A copy of <paramref name="ARange"/> with a different lower bound.</returns>
        [Pure]
        public static Range<T> WithLower<T>(
            in this Range<T> ARange,
            [CanBeNull] in T ALower,
            bool AIncludesLower
        ) where T : IComparable<T>
            => Range<T>.Of(ALower, AIncludesLower, ARange.Upper, ARange.IncludesUpper);

        /// <summary>
        /// Make a new <see cref="Range{T}"/> with a different upper bound.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AUpper">The new upper bound.</param>
        /// <returns>A copy of <paramref name="ARange"/> with a different upper bound.</returns>
        [Pure]
        public static Range<T> WithUpper<T>(in this Range<T> ARange, [CanBeNull] in T AUpper)
            where T : IComparable<T>
            => Range<T>.Of(ARange.Lower, ARange.IncludesLower, AUpper, ARange.IncludesUpper);
        /// <summary>
        /// Make a new <see cref="Range{T}"/> with a different upper bound.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AIncludesUpper">Whether the upper bound is included.</param>
        /// <returns>A copy of <paramref name="ARange"/> with a different upper bound.</returns>
        [Pure]
        public static Range<T> WithUpper<T>(in this Range<T> ARange, bool AIncludesUpper)
            where T : IComparable<T>
            => Range<T>.Of(ARange.Lower, ARange.IncludesLower, ARange.Upper, AIncludesUpper);
        /// <summary>
        /// Make a new <see cref="Range{T}"/> with a different upper bound.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AUpper">The new upper bound.</param>
        /// <param name="AIncludesUpper">Whether the upper bound is included.</param>
        /// <returns>A copy of <paramref name="ARange"/> with a different upper bound.</returns>
        [Pure]
        public static Range<T> WithUpper<T>(
            in this Range<T> ARange,
            [CanBeNull] in T AUpper,
            bool AIncludesUpper
        ) where T : IComparable<T>
            => Range<T>.Of(ARange.Lower, ARange.IncludesLower, AUpper, AIncludesUpper);
    }
}
