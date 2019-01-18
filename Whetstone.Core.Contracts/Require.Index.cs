using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class Require
    {
        const string C_InvalidIndex = @"Value is not an index in range [0, {0}).";

        /// <summary>
        /// Require that an <see cref="int"/> index is within a [0, length) range.
        /// </summary>
        /// <param name="ALength">The range length.</param>
        /// <param name="AParam">The index parameter.</param>
        /// <param name="AParamName">The index parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is not within [0, <paramref name="ALength"/>).
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static int Index(
            int ALength,
            int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            if (AParam < 0 || AParam >= ALength)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    string.Format(C_InvalidIndex, ALength)
                );
            }

            return AParam;
        }

        /// <summary>
        /// Require that an <see cref="int"/> index is within a <see cref="string"/>.
        /// </summary>
        /// <param name="AString">The string.</param>
        /// <param name="AParam">The index parameter.</param>
        /// <param name="AParamName">The index parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is not within [0, <paramref name="AString"/>.Length).
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
        public static int Index(
            [CanBeNull] string AString,
            int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) => Index(AString?.Length ?? 0, AParam, AParamName);

        /// <summary>
        /// Require that an <see cref="int"/> index is within a 1D <see cref="Array"/>.
        /// </summary>
        /// <typeparam name="T">The array item type.</typeparam>
        /// <param name="AArray">The 1D <see cref="Array"/>.</param>
        /// <param name="AParam">The index parameter.</param>
        /// <param name="AParamName">The index parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is not within [0, <paramref name="AArray"/>.Length).
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
        public static int Index<T>(
            [CanBeNull] T[] AArray,
            int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) => Index(AArray?.Length ?? 0, AParam, AParamName);

        /// <summary>
        /// Require that an <see cref="int"/> index is within a <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The collection item type.</typeparam>
        /// <param name="ACollection">The <see cref="ICollection{T}"/>.</param>
        /// <param name="AParam">The index parameter.</param>
        /// <param name="AParamName">The index parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is not within [0, <paramref name="ACollection"/>.Count).
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
        public static int Index<T>(
            [CanBeNull] ICollection<T> ACollection,
            int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) => Index(ACollection?.Count ?? 0, AParam, AParamName);
    }
}
