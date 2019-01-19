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
        /// <param name="AOffsetParam">The offset parameter.</param>
        /// <param name="AOffsetParamName">The offset parameter name.</param>
        /// <param name="ALengthParam">The length parameter.</param>
        /// <param name="ALengthParamName">The length parameter name.</param>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void IndexRange(
            int ALength,
            int AOffsetParam,
            [NotNull] [InvokerParameterName] string AOffsetParamName,
            int ALengthParam,
            [NotNull] [InvokerParameterName] string ALengthParamName
        )
        {
            if (ALengthParam == 0)
            {
                return;
            }

            Debug.Assert(
                AOffsetParam >= 0,
                @"Offset is negative.",
                @"This indicates a contract violation."
            );

            var limit = ALength - AOffsetParam;

            Debug.Assert(
                ALength >= 0 && ALength <= limit,
                $@"{ALengthParamName} outside [0, {limit}].",
                @"This indicates a contract violation."
            );
        }

        /// <summary>
        /// Ensure that an <see cref="int"/> index is within a <see cref="string"/>.
        /// </summary>
        /// <param name="AString">The string.</param>
        /// <param name="AOffsetParam">The offset parameter.</param>
        /// <param name="AOffsetParamName">The offset parameter name.</param>
        /// <param name="ALengthParam">The length parameter.</param>
        /// <param name="ALengthParamName">The length parameter name.</param>
        /// <remarks>
        /// If <paramref name="AString"/> is <see langword="null"/>, it is assumed to have length 0.
        /// </remarks>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void IndexRange(
            [CanBeNull] string AString,
            int AOffsetParam,
            [NotNull] [InvokerParameterName] string AOffsetParamName,
            int ALengthParam,
            [NotNull] [InvokerParameterName] string ALengthParamName
        ) => IndexRange(
            AString?.Length ?? 0,
            AOffsetParam,
            AOffsetParamName,
            ALengthParam,
            ALengthParamName
        );

        /// <summary>
        /// Ensure that an <see cref="int"/> index is within a 1D <see cref="Array"/>.
        /// </summary>
        /// <param name="AArray">The string.</param>
        /// <param name="AOffsetParam">The offset parameter.</param>
        /// <param name="AOffsetParamName">The offset parameter name.</param>
        /// <param name="ALengthParam">The length parameter.</param>
        /// <param name="ALengthParamName">The length parameter name.</param>
        /// <exception cref="OverflowException">
        /// <paramref name="AArray"/> is longer than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// If <paramref name="AArray"/> is <see langword="null"/>, it is assumed to have length 0.
        /// </remarks>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void IndexRange<T>(
            [CanBeNull] T[] AArray,
            int AOffsetParam,
            [NotNull] [InvokerParameterName] string AOffsetParamName,
            int ALengthParam,
            [NotNull] [InvokerParameterName] string ALengthParamName
        ) => IndexRange(
            AArray?.Length ?? 0,
            AOffsetParam,
            AOffsetParamName,
            ALengthParam,
            ALengthParamName
        );

        /// <summary>
        /// Ensure that an <see cref="int"/> index is within a <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The collection item type.</typeparam>
        /// <param name="ACollection">The <see cref="ICollection{T}"/>.</param>
        /// <param name="AOffsetParam">The offset parameter.</param>
        /// <param name="AOffsetParamName">The offset parameter name.</param>
        /// <param name="ALengthParam">The length parameter.</param>
        /// <param name="ALengthParamName">The length parameter name.</param>
        /// <remarks>
        /// If <paramref name="ACollection"/> is <see langword="null"/>, it is assumed to have 0
        /// elements.
        /// </remarks>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void IndexRange<T>(
            [CanBeNull] ICollection<T> ACollection,
            int AOffsetParam,
            [NotNull] [InvokerParameterName] string AOffsetParamName,
            int ALengthParam,
            [NotNull] [InvokerParameterName] string ALengthParamName
        ) => IndexRange(
            ACollection?.Count ?? 0,
            AOffsetParam,
            AOffsetParamName,
            ALengthParam,
            ALengthParamName
        );
    }
}
