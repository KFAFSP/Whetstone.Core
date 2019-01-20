using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
	public partial class Require
	{
		/// <summary>
        /// Require that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is negative.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static sbyte NotNegative(
            in sbyte AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            if (AParam < 0)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    @"Value must be positive."
                );
            }

            return AParam;
        }

		/// <summary>
        /// Require that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is negative.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static short NotNegative(
            in short AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            if (AParam < 0)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    @"Value must be positive."
                );
            }

            return AParam;
        }

		/// <summary>
        /// Require that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is negative.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static int NotNegative(
            in int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            if (AParam < 0)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    @"Value must be positive."
                );
            }

            return AParam;
        }

		/// <summary>
        /// Require that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is negative.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static long NotNegative(
            in long AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            if (AParam < 0L)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    @"Value must be positive."
                );
            }

            return AParam;
        }

		/// <summary>
        /// Require that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is negative.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static float NotNegative(
            in float AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            if (AParam < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    @"Value must be positive."
                );
            }

            return AParam;
        }

		/// <summary>
        /// Require that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is negative.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static double NotNegative(
            in double AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            if (AParam < 0)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    @"Value must be positive."
                );
            }

            return AParam;
        }

		/// <summary>
        /// Require that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is negative.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static decimal NotNegative(
            in decimal AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            if (AParam < 0m)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    @"Value must be positive."
                );
            }

            return AParam;
        }

	}
}
