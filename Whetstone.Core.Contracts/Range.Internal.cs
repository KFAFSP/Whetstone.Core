using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial struct Range<T>
    {
        /// <summary>
        /// Check whether a specified range must be empty.
        /// </summary>
        /// <param name="ALower">The lower bound of the range.</param>
        /// <param name="AIncludesLower">Whether the lower bound is included.</param>
        /// <param name="AUpper">The upper bound of the range.</param>
        /// <param name="AIncludesUpper">Whether the upper bound is included.</param>
        /// <returns>
        /// <see langword="true"/> if one of the strong emptiness criteria applies;
        /// <see langword="false"/> otherwise.
        /// </returns>
        [Pure]
        static bool MustBeEmpty(
            [CanBeNull] T ALower,
            bool AIncludesLower,
            [CanBeNull] T AUpper,
            bool AIncludesUpper
        )
        {
            // Special case: reference bound nullity.
            if (!typeof(T).IsValueType && ReferenceEquals(AUpper, null))
            {
                // See WeakOrdering.Compare<T>(T, T) for more information.
                // In this case, this special case saves us from dealing with null anywhere else.

                // Any range with an upper bound of null is empty except the [null, null] range.
                return !AIncludesLower || !AIncludesUpper || !ReferenceEquals(ALower, null);
            }

            // Compare the bounds.
            // NOTE: Because of our nullity handling, AUpper is guaranteed not-null.
            // ReSharper disable once PossibleNullReferenceException
            var compare = AUpper.CompareTo(ALower);

            // Check strong criteria: Upper < Lower.
            if (compare < 0)
            {
                // Any range where the bounds are reversed must be empty.
                return true;
            }

            // Check strong criteria: Upper == Lower AND Upper not in range
            if (compare == 0)
            {
                // Any range with same bounds that is not fully inclusive is empty.
                return !AIncludesLower || !AIncludesUpper;
            }

            return false;
        }

        /// <summary>
        /// Compare a value that is known to be non-<see langword="null"/> to both bounds of the
        /// range.
        /// </summary>
        /// <param name="ATest">The value.</param>
        /// <returns>
        /// <list type="table">
        /// <listheader>
        /// </listheader>
        /// <item>
        /// <term>-1</term>
        /// <description><paramref name="ATest"/> is below the lower bound.</description>
        /// </item>
        /// <item>
        /// <term>0</term>
        /// <description><paramref name="ATest"/> is inside the range.</description>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <description><paramref name="ATest"/> is above the lower bound.</description>
        /// </item>
        /// </list>
        /// </returns>
        [Pure]
        int CompareToInternal([NotNull] T ATest)
        {
            // Compare against the lower boundary.
            var lower = ATest.CompareTo(Lower);
            if (lower < 0 || !IncludesLower && lower == 0)
            {
                // ATest is less than the lower bound.
                return -1; // LESS-THAN
            }

            // Compare against the upper boundary.
            var upper = ATest.CompareTo(Upper);
            if (upper > 0 || !IncludesUpper && upper == 0)
            {
                // ATest is greater than the upper bound.
                return 1; // GREATER-THAN
            }

            // ATest is within the lower and upper bound.
            return 0; // CONTAINED
        }
    }
}
