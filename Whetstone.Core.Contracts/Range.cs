using System;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Represents a range between to values of a comparable type.
    /// </summary>
    /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
    /// <remarks>
    /// <para>
    /// <c>default(Range<typeparamref name="T"/>)</c> is guaranteed to be an exclusive and empty
    /// range, specifically the (<c>default</c>, <c>default</c>) range.
    /// </para>
    /// <para>
    /// Note that special semantics concerning the emptiness of <see cref="Range{T}"/> apply since
    /// the limitation on <typeparamref name="T"/> is only that it must implement the
    /// <see cref="IComparable{T}"/> interface. See <see cref="IsEmpty"/> for details.
    /// </para>
    /// </remarks>
    [PublicAPI]
    public readonly partial struct Range<T> : IEquatable<Range<T>>
        where T : IComparable<T>
    {
        /// <summary>
        /// Make a <see cref="Range{T}"/> between two values.
        /// </summary>
        /// <param name="ALower">The lower bound of the range.</param>
        /// <param name="AIncludesLower">Whether the lower bound is included.</param>
        /// <param name="AUpper">The upper bound of the range.</param>
        /// <param name="AIncludesUpper">Whether the upper bound is included.</param>
        /// <returns>
        /// A <see cref="Range{T}"/> between <paramref name="ALower"/> and
        /// <paramref name="AUpper"/>.
        /// </returns>
        /// <remarks>
        /// Any combination of parameters produces a valid <see cref="Range{T}"/>, but the strong
        /// criteria for emptiness (see <see cref="IsEmpty"/>) are checked by this method
        /// and flagged for the resulting range if applicable. No information will be lost, but
        /// empty <see cref="Range{T}"/>s do receive special treatment.
        /// </remarks>
        [Pure]
        public static Range<T> Of(
            [CanBeNull] T ALower,
            bool AIncludesLower,
            [CanBeNull] T AUpper,
            bool AIncludesUpper
        )
        {
            // Determine if a strong emptiness criteria applies.
            var flags = MustBeEmpty(ALower, AIncludesLower, AUpper, AIncludesUpper)
                ? RangeFlags.Empty
                : RangeFlags.MayNotBeEmpty;

            // Build the range flags.
            if (AIncludesLower) flags |= RangeFlags.IncludesLower;
            if (AIncludesUpper) flags |= RangeFlags.IncludesUpper;

            return new Range<T>(flags, ALower, AUpper);
        }

        /// <summary>
        /// Get an empty <see cref="Range{T}"/> of (<c>default</c>, <c>default</c>).
        /// </summary>
        public static Range<T> Empty => default;

        /// <summary>
        /// Initialize a new <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="AFlags">The <see cref="RangeFlags"/>.</param>
        /// <param name="ALower">The lower bound.</param>
        /// <param name="AUpper">The upper bound.</param>
        Range(RangeFlags AFlags, T ALower, T AUpper)
        {
            Flags = AFlags;
            Lower = ALower;
            Upper = AUpper;
        }

        /// <summary>
        /// Compare a value to both bounds of the range.
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
        public int CompareWith([CanBeNull] T ATest)
        {
            // Quick exit: reference argument nullity.
            if (!typeof(T).IsValueType && ReferenceEquals(ATest, null))
            {
                // See WeakOrdering.Compare<T>(T, T) for more information.
                // In this case, this special case saves us from dealing with null anywhere else.

                // If this is the [null, null] range, null is contained; otherwise the lower bound
                // is violated.
                return ReferenceEquals(Lower, null) && IncludesLower
                    ? 0 // CONTAINED
                    : -1; // LESS-THAN
            }

            // NOTE: Because of our nullity handling, ATest is guaranteed not-null.
            // ReSharper disable once AssignNullToNotNullAttribute
            return CompareToInternal(ATest);
        }

        #region IEquatable<Range<T>>
        /// <inheritdoc />
        [Pure]
        public bool Equals(Range<T> ARange)
        {
            return Flags == ARange.Flags
                && WeakOrdering.Compare(Lower, ARange.Lower) == 0
                && WeakOrdering.Compare(Upper, ARange.Upper) == 0;
        }
        #endregion

        #region System.Object overrides
        /// <inheritdoc />
        [Pure]
        public override bool Equals(object AOther)
            => AOther is Range<T> range && Equals(range);
        /// <inheritdoc />
        [Pure]
        public override int GetHashCode()
        {
            var hash = Flags.GetHashCode() * 9;
            hash = hash * 19 + Lower?.GetHashCode() ?? 0;
            hash = hash * 23 + Upper?.GetHashCode() ?? 0;
            return hash;
        }
        /// <inheritdoc />
        [Pure]
        public override string ToString()
            => $"{(IncludesLower ? "[" : "(")}" +
               $"{Lower?.ToString() ?? "null"}, {Upper?.ToString() ?? "null"}" +
               $"{(IncludesUpper ? "]" : ")")}";
        #endregion

        /// <summary>
        /// Get the <see cref="RangeFlags"/>.
        /// </summary>
        RangeFlags Flags { get; }

        /// <summary>
        /// Get the lower bound.
        /// </summary>
        [CanBeNull]
        public T Lower { get; }
        /// <summary>
        /// Get the upper bound.
        /// </summary>
        [CanBeNull]
        public T Upper { get; }

        /// <summary>
        /// Get a value indicating whether the range is empty.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Since this type does not require a bijective N &lt;-&gt; <typeparamref name="T"/>
        /// mapping to be present, successors or predecessors of the <see cref="Lower"/> or
        /// <see cref="Upper"/> bound are not known to it. Only ranges that violate the strong
        /// emptiness criteria are known to be empty; all others may or may not be empty.
        /// </para>
        /// <para>
        /// Specifically, two strong criteria are checked and represented by this property:
        /// <list type="bullet">
        /// <item><description>
        /// Any range {a, b} (any inclusion/exclusion) where a > b is empty.
        /// </description></item>
        /// <item><description>
        /// Any range {a, a} (any inclusion/exclusion) EXCEPT [a, a] (fully inclusive) is empty.
        /// </description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public bool IsEmpty => (Flags & RangeFlags.MayNotBeEmpty) == RangeFlags.Empty;

        /// <summary>
        /// Get a value indicating whether the <see cref="Lower"/> bound is included in the range.
        /// </summary>
        public bool IncludesLower => (Flags & RangeFlags.IncludesLower) == RangeFlags.IncludesLower;
        /// <summary>
        /// Get a value indicating whether the <see cref="Upper"/> bound is included in the range.
        /// </summary>
        public bool IncludesUpper => (Flags & RangeFlags.IncludesUpper) == RangeFlags.IncludesUpper;

        /// <summary>
        /// Check whether two <see cref="Range{T}"/> instances are equal.
        /// </summary>
        /// <param name="ALhs">The left hand side.</param>
        /// <param name="ARhs">The right hand side.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ALhs"/> is equal to <paramref name="ARhs"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool operator ==(Range<T> ALhs, Range<T> ARhs)
            => ALhs.Equals(ARhs);
        /// <summary>
        /// Check whether two <see cref="Range{T}"/> instances are unequal.
        /// </summary>
        /// <param name="ALhs">The left hand side.</param>
        /// <param name="ARhs">The right hand side.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ALhs"/> is unequal to <paramref name="ARhs"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool operator !=(Range<T> ALhs, Range<T> ARhs)
            => !ALhs.Equals(ARhs);
    }

    /// <summary>
    /// Provides static convenience methods for using the <see cref="Range{T}"/> generic type with
    /// inferred type arguments.
    /// </summary>
    [PublicAPI]
    public static partial class Range { }
}
