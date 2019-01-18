using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class Require
    {
        const string C_InvalidIndexRangeLength = @"Length is not within range [0, {0}].";

        /// <summary>
        /// Require that an <see cref="int"/> index is within a [0, length) range.
        /// </summary>
        /// <param name="ALength">The range length.</param>
        /// <param name="AOffsetParam">The offset parameter.</param>
        /// <param name="AOffsetParamName">The offset parameter name.</param>
        /// <param name="ALengthParam">The length parameter.</param>
        /// <param name="ALengthParamName">The length parameter name.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffsetParam"/> is not within [0, <paramref name="ALength"/>).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALengthParam"/> is not within
        /// [0, <paramref name="ALength"/>-<paramref name="AOffsetParam"/>].
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static void IndexRange(
            int ALength,
            int AOffsetParam,
            [NotNull] [InvokerParameterName] string AOffsetParamName,
            int ALengthParam,
            [NotNull] [InvokerParameterName] string ALengthParamName
        )
        {
            Index(ALength, AOffsetParam, AOffsetParamName);

            var limit = ALength - AOffsetParam;

            if (ALengthParam < 0 || ALengthParam > limit)
            {
                throw new ArgumentOutOfRangeException(
                    ALengthParamName,
                    ALengthParam,
                    string.Format(C_InvalidIndexRangeLength, limit)
                );
            }
        }

        /// <summary>
        /// Require that an <see cref="int"/> index is within a <see cref="string"/>.
        /// </summary>
        /// <param name="AString">The string.</param>
        /// <param name="AOffsetParam">The offset parameter.</param>
        /// <param name="AOffsetParamName">The offset parameter name.</param>
        /// <param name="ALengthParam">The length parameter.</param>
        /// <param name="ALengthParamName">The length parameter name.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffsetParam"/> is not within [0, <paramref name="AString"/>.Length).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALengthParam"/> is not within
        /// [0, <paramref name="AString"/>.Length-<paramref name="AOffsetParam"/>].
        /// </exception>
        /// <remarks>
        /// <para>
        /// If <paramref name="AString"/> is <see langword="null"/>, it is assumed to have length
        /// 0.
        /// </para>
        /// <para>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </para>
        /// </remarks>
        [DebuggerHidden]
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
        /// Require that an <see cref="int"/> index is within a 1D <see cref="Array"/>.
        /// </summary>
        /// <param name="AArray">The string.</param>
        /// <param name="AOffsetParam">The offset parameter.</param>
        /// <param name="AOffsetParamName">The offset parameter name.</param>
        /// <param name="ALengthParam">The length parameter.</param>
        /// <param name="ALengthParamName">The length parameter name.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffsetParam"/> is not within [0, <paramref name="AArray"/>.Length).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALengthParam"/> is not within
        /// [0, <paramref name="AArray"/>.Length-<paramref name="AOffsetParam"/>].
        /// </exception>
        /// <exception cref="OverflowException">
        /// <paramref name="AArray"/> is longer than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// If <paramref name="AArray"/> is <see langword="null"/>, it is assumed to have length
        /// 0.
        /// </para>
        /// <para>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </para>
        /// </remarks>
        [DebuggerHidden]
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
        /// Require that an <see cref="int"/> index is within a <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The collection item type.</typeparam>
        /// <param name="ACollection">The <see cref="ICollection{T}"/>.</param>
        /// <param name="AOffsetParam">The offset parameter.</param>
        /// <param name="AOffsetParamName">The offset parameter name.</param>
        /// <param name="ALengthParam">The length parameter.</param>
        /// <param name="ALengthParamName">The length parameter name.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffsetParam"/> is not within [0, <paramref name="ACollection"/>.Count).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALengthParam"/> is not within
        /// [0, <paramref name="ACollection"/>.Count-<paramref name="AOffsetParam"/>].
        /// </exception>
        /// <remarks>
        /// <para>
        /// If <paramref name="ACollection"/> is <see langword="null"/>, it is assumed to have 0
        /// elements.
        /// </para>
        /// <para>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </para>
        /// </remarks>
        [DebuggerHidden]
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
