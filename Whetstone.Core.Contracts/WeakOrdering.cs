using System;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Provides static methods for working with types that implement the
    /// <see cref="IComparable{T}"/> interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Types that implement the <see cref="IComparable{T}"/> interface define a weak-ordering of
    /// their instances. This helper class allows for the user to perform weak-ordering specific
    /// operations on these types without the need for implementation support on their side.
    /// </para>
    /// <para>
    /// Works for <see cref="ValueType"/> and reference type implementors of the interface.
    /// </para>
    /// </remarks>
    [PublicAPI]
    public static partial class WeakOrdering { }
}