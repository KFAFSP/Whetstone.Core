using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial class Require
    {
        /// <summary>
        /// Require that a value is within a <see cref="Range{T}"/>.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is outside <paramref name="ARange"/>.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static T InRange<T>(
            in Range<T> ARange,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            if (!ARange.Contains(AParam))
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    $@"Value must be within {ARange}."
                );
            }

            return AParam;
        }

        /// <summary>
        /// Require that a <see cref="Range{T}"/> is within a <see cref="Range{T}"/>.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is outside <paramref name="ARange"/>.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static Range<T> InRange<T>(
            in Range<T> ARange,
            in Range<T> AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            if (!ARange.Contains(AParam))
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    $@"Value must be within {ARange}."
                );
            }

            return AParam;
        }

        /// <summary>
        /// Require that an index span is within a <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="ARange">The <see cref="Range{T}"/>.</param>
        /// <param name="AOffsetParam">The start offset parameter value.</param>
        /// <param name="AOffsetParamName">The start offset parameter name.</param>
        /// <param name="ALengthParam">The span length parameter value.</param>
        /// <param name="ALengthParamName">The span length parameter name.</param>
        /// <returns>
        /// The
        /// [<paramref name="AOffsetParam"/>, <paramref name="AOffsetParam"/> + <paramref name="ALengthParam"/>)
        /// <see cref="Range{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffsetParam"/> is outside <paramref name="ARange"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALengthParam"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALengthParam"/> reaches beyond <paramref name="ARange"/>.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static Range<int> Span(
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
            if (limit > ARange.Limit())
            {
                var maxLength = ARange.Length() - (AOffsetParam - ARange.Offset());
                throw new ArgumentOutOfRangeException(
                    ALengthParamName,
                    ALengthParam,
                    $@"Value must be within [0, {maxLength})."
                );
            }

            return Range.Of(AOffsetParam, true, limit, false);
        }
    }
}
