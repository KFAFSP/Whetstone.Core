using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial struct Range<T>
    {
        /// <summary>
        /// Get the intersection of two <see cref="Range{T}"/>s.
        /// </summary>
        /// <param name="ALhs">The left hand side operand.</param>
        /// <param name="ARhs">The right hand side operand.</param>
        /// <returns>
        /// A new <see cref="Range{T}"/> that is the intersection of <paramref name="ALhs"/> and
        /// <paramref name="ARhs"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Although an empty range might be contained in another range, empty ranges always yield
        /// empty intersections with other ranges.
        /// </para>
        /// <para>
        /// Optimized for the quantitative intersection result. If only the qualitative result is
        /// needed, use <see cref="Intersects(in Range{T})"/> instead.
        /// </para>
        /// </remarks>
        [Pure]
        public static Range<T> Intersect(in Range<T> ALhs, in Range<T> ARhs)
        {
            if (ALhs.IsEmpty)
            {
                // Intersections with empty ranges are empty.
                return ALhs;
            }

            if (ARhs.IsEmpty)
            {
                // Intersections with empty ranges are empty.
                return ARhs;
            }

            // Determine the new lower bound as the maximum of both.
            bool includesLower;
            var lowerComp = WeakOrdering.Compare(ALhs.Lower, ARhs.Lower);
            if (lowerComp < 0)
            {
                // ARhs has the higher lower bound. It dictates inclusiveness.
                includesLower = ARhs.IncludesLower;
            }
            else if (lowerComp > 0)
            {
                // ALhs has the higher lower bound. It dictates inclusiveness.
                includesLower = ALhs.IncludesLower;
            }
            else
            {
                // Both have the same lower bound. Determine inclusiveness by AND.
                includesLower = ALhs.IncludesLower && ARhs.IncludesLower;
            }

            // Determine the new upper bound as the minimum of both.
            bool includesUpper;
            var upperComp = WeakOrdering.Compare(ALhs.Upper, ARhs.Upper);
            if (upperComp > 0)
            {
                // ARhs has the lower upper bound. It dictates inclusiveness.
                includesUpper = ARhs.IncludesUpper;
            }
            else if (upperComp < 0)
            {
                // ALhs has the lower upper bound. It dictates inclusiveness.
                includesUpper = ALhs.IncludesUpper;
            }
            else
            {
                // Both have the same upper bound. Determine inclusiveness by AND.
                includesUpper = ALhs.IncludesUpper && ARhs.IncludesUpper;
            }

            // Make a new (potentially empty) range.
            return Of(
                lowerComp <= 0 ? ARhs.Lower : ALhs.Lower,
                includesLower,
                upperComp <= 0 ? ALhs.Upper : ARhs.Upper,
                includesUpper
            );
        }

        /// <summary>
        /// Check whether the specified <see cref="Range{T}"/> intersects this range.
        /// </summary>
        /// <param name="ATest">The <see cref="Range{T}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ATest"/> intersects this range;
        /// <see langword="false"/> otherwise.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Although an empty range might be contained in another range, empty ranges cannot
        /// intersect with any range.
        /// </para>
        /// <para>
        /// Optimized for the qualitative intersection result. If the actual intersection range is
        /// required, use <see cref="Intersect(in Range{T},in Range{T})"/> instead.
        /// </para>
        /// </remarks>
        [Pure]
        public bool Intersects(in Range<T> ATest)
        {
            if (IsEmpty || ATest.IsEmpty)
            {
                // Empty ranges do not intersect.
                return false;
            }

            var lower = WeakOrdering.Compare(Lower, ATest.Upper);
            if (lower > 0 || lower == 0 && !IncludesLower)
            {
                // ATest has an upper bound less than our lower bound.
                return false;
            }

            var upper = WeakOrdering.Compare(Upper, ATest.Lower);
            if (upper < 0 || upper == 0 && !IncludesUpper)
            {
                // ATest has a lower bound greater than our upper bound.
                return false;
            }

            // ATest must intersect.
            return true;
        }
    }

    public static partial class Range
    {
        /// <summary>
        /// Get the intersection of two <see cref="Range{T}"/>s.
        /// </summary>
        /// <param name="ALhs">The left hand side operand.</param>
        /// <param name="ARhs">The right hand side operand.</param>
        /// <returns>
        /// A new <see cref="Range{T}"/> that is the intersection of <paramref name="ALhs"/> and
        /// <paramref name="ARhs"/>.
        /// </returns>
        /// <remarks>
        /// For more information, see <see cref="Range{T}.Intersect(in Range{T},in Range{T})"/>.
        /// </remarks>
        [ExcludeFromCodeCoverage]
        [Pure]
        public static Range<T> Intersect<T>(in Range<T> ALhs, in Range<T> ARhs)
            where T : IComparable<T>
            => Range<T>.Intersect(ALhs, ARhs);
    }
}
