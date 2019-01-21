using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial class Ensure
    {
        /// <summary>
        /// Ensure that a value is within a <see cref="Range{T}"/>.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void InRange<T>(
            in Range<T> ARange,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            Debug.Assert(
                ARange.Contains(AParam),
                $@"{AParamName} is outside {ARange}.",
                @"This indicates a contract violation."
            );
        }

        /// <summary>
        /// Ensure that a <see cref="Range{T}"/> is within a <see cref="Range{T}"/>.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void InRange<T>(
            in Range<T> ARange,
            in Range<T> AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            Debug.Assert(
                ARange.Contains(AParam),
                $@"{AParamName} is outside {ARange}.",
                @"This indicates a contract violation."
            );
        }

        /// <summary>
        /// Ensure that an index span is within a <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AOffsetParam">The start offset parameter value.</param>
        /// <param name="AOffsetParamName">The start offset parameter name.</param>
        /// <param name="ALengthParam">The span length parameter value.</param>
        /// <param name="ALengthParamName">The span length parameter name.</param>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void Span(
            in Range<int> ARange,
            in int AOffsetParam,
            [NotNull] [InvokerParameterName] string AOffsetParamName,
            in int ALengthParam,
            [NotNull] [InvokerParameterName] string ALengthParamName
        )
        {
            InRange(ARange, AOffsetParam, AOffsetParamName);
            NotNegative(ALengthParam, ALengthParamName);

            var limit = AOffsetParam + ALengthParam;
            var maxLength = ARange.Length() - (AOffsetParam - ARange.Offset());
            Debug.Assert(
                ARange.Limit() >= limit,
                $@"{ALengthParam} is outside {maxLength}.",
                @"This indicates a contract violation."
            );
        }
    }
}
