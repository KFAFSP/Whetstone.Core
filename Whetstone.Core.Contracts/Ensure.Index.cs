using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class Ensure
    {
        /// <summary>
        /// Ensure that an <see cref="int"/> index is within a [0, length) range.
        /// </summary>
        /// <param name="ALength">The range length.</param>
        /// <param name="AParam">The index parameter.</param>
        /// <param name="AParamName">The index parameter name.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Index(
            int ALength,
            int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
                AParam >= 0 && AParam < ALength,
                $@"{AParamName} outside [0, {ALength}).",
                @"This indicates a contract violation."
            );
        }

        /// <summary>
        /// Ensure that an <see cref="int"/> index is within a <see cref="string"/>.
        /// </summary>
        /// <param name="AString">The string.</param>
        /// <param name="AParam">The index parameter.</param>
        /// <param name="AParamName">The index parameter name.</param>
        /// <remarks>
        /// If <paramref name="AString"/> is <see langword="null"/>, it is assumed to have length 0.
        /// </remarks>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Index(
            [CanBeNull] string AString,
            int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) => Index(AString?.Length ?? 0, AParam, AParamName);

        /// <summary>
        /// Ensure that an <see cref="int"/> index is within a 1D <see cref="Array"/>.
        /// </summary>
        /// <typeparam name="T">The array item type.</typeparam>
        /// <param name="AArray">The 1D <see cref="Array"/>.</param>
        /// <param name="AParam">The index parameter.</param>
        /// <param name="AParamName">The index parameter name.</param>
        /// <exception cref="OverflowException">
        /// <paramref name="AArray"/> is longer than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// If <paramref name="AArray"/> is <see langword="null"/>, it is assumed to have length 0.
        /// </remarks>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Index<T>(
            [CanBeNull] T[] AArray,
            int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) => Index(AArray?.Length ?? 0, AParam, AParamName);

        /// <summary>
        /// Ensure that an <see cref="int"/> index is within a <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The collection item type.</typeparam>
        /// <param name="ACollection">The <see cref="ICollection{T}"/>.</param>
        /// <param name="AParam">The index parameter.</param>
        /// <param name="AParamName">The index parameter name.</param>
        /// <remarks>
        /// If <paramref name="ACollection"/> is <see langword="null"/>, it is assumed to have 0
        /// elements.
        /// </remarks>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Index<T>(
            [CanBeNull] ICollection<T> ACollection,
            int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) => Index(ACollection?.Count ?? 0, AParam, AParamName);
    }
}
