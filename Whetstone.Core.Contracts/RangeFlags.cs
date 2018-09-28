using System;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Internal enumeration of property bit-flags for <see cref="Range{T}"/> structs.
    /// </summary>
    [Flags]
    internal enum RangeFlags
    {
        /// <summary>
        /// Indicates that the range is empty.
        /// </summary>
        Empty = 0b000,
        /// <summary>
        /// Indicates that the range may not be empty.
        /// </summary>
        MayNotBeEmpty = 0b100,

        /// <summary>
        /// Indicates that the lower bound is included.
        /// </summary>
        IncludesLower = 0b010,
        /// <summary>
        /// Indicates that the upper bound is included.
        /// </summary>
        IncludesUpper = 0b001
    }
}