using System;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class WeakOrdering
    {
        /// <summary>
        /// Compare two instances of comparable types.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="ALhs">The left hand side operand.</param>
        /// <param name="ARhs">The right hand side operand.</param>
        /// <returns>
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Meaning</description>
        /// </listheader>
        /// <item>
        /// <term><c>&lt;0</c></term>
        /// <description><paramref name="ALhs"/> is less than <paramref name="ARhs"/>.</description>
        /// </item>
        /// <item>
        /// <term><c>0</c></term>
        /// <description><paramref name="ALhs"/> is equal to <paramref name="ARhs"/>.</description>
        /// </item>
        /// <item>
        /// <term><c>&gt;0</c></term>
        /// <description>
        /// <paramref name="ALhs"/> is greater than <paramref name="ARhs"/>.
        /// </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Correctly deals with <see langword="null"/> parameters as opposed to a simple call to
        /// <see cref="IComparable{T}.CompareTo(T)"/>. This handling only applies if
        /// <typeparamref name="T"/> is a reference type.
        /// </remarks>
        [Pure]
        public static int Compare<T>([CanBeNull] T ALhs, [CanBeNull] T ARhs)
            where T : IComparable<T>
        {
            // Special case: reference operand nullity.
            if (!typeof(T).IsValueType && ReferenceEquals(ALhs, null))
            {
                // Per definition, IComparable<T>.CompareTo(T) must return > 0 for null.
                // This means null is the "least T" and only equal to itself.
                return ReferenceEquals(ARhs, null)
                    ? 0
                    : -1;
            }

            // NOTE: Because of our nullity handling, ALhs is guaranteed to be not-null.
            // ReSharper disable once PossibleNullReferenceException
            return ALhs.CompareTo(ARhs);
        }
    }
}
