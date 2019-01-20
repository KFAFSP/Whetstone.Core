using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial struct Range<T>
    {
        /// <summary>
        /// Check whether the specified value is within this range.
        /// </summary>
        /// <param name="ATest">The value.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ATest"/> is within this range;
        /// <see langword="false"/> otherwise.
        /// </returns>
        /// <remarks>
        /// Ranges that have the value <see langword="true"/> for <see cref="IsEmpty"/> are
        /// definitely empty and will always return <see langword="false"/>.
        /// </remarks>
        [Pure]
        public bool Contains([CanBeNull] in T ATest)
        {
            // Quick exit: emptiness.
            if (IsEmpty)
            {
                // Empty ranges contains nothing.
                return false;
            }

            // Quick exit: reference argument nullity.
            if (!typeof(T).IsValueType && ReferenceEquals(ATest, null))
            {
                // See WeakOrdering.Compare<T>(T, T) for more information.

                // Any range may only contain null iff: Lower is null AND Lower is included
                return ReferenceEquals(Lower, null) && IncludesLower;
            }

            // NOTE: Because of our nullity handling, ATest is guaranteed not-null.
            // ReSharper disable once AssignNullToNotNullAttribute
            return CompareToInternal(ATest) == 0;
        }
        /// <summary>
        /// Check whether the specified <see cref="Range{T}"/> is within this range.
        /// </summary>
        /// <param name="ATest">The <see cref="Range{T}"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ATest"/> is within this range;
        /// <see langword="false"/> otherwise.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Ranges that have the value <see langword="true"/> for <see cref="IsEmpty"/> are
        /// definitely empty and will always return <see langword="false"/>.
        /// </para>
        /// <para>
        /// Supplying an empty <paramref name="ATest"/> range however may still result in it being
        /// contained since only the bounds are checked.
        /// </para>
        /// </remarks>
        [Pure]
        public bool Contains(in Range<T> ATest)
        {
            // Quick exit: emptiness.
            if (IsEmpty)
            {
                // Empty ranges contains nothing.
                return false;
            }

            // Compare the lower boundaries.
            var lower = WeakOrdering.Compare(Lower, ATest.Lower);
            if (lower > 0 || !IncludesLower && ATest.IncludesLower && lower == 0)
            {
                // ATest's lower bound is less than our lower bound.
                return false;
            }

            // Compare the upper boundaries.
            var upper = WeakOrdering.Compare(Upper, ATest.Upper);
            if (upper < 0 || !IncludesUpper && ATest.IncludesUpper && upper == 0)
            {
                // ATest's upper bound is greater than out upper bound.
                return false;
            }

            return true;
        }
    }
}
