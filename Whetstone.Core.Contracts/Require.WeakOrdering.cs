using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial class Require
    {
        /// <summary>
        /// Require that a value is greater than or equal to a minimum.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AMinimum">The inclusive minimum.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is less than <paramref name="AMinimum"/>.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static T AtLeast<T>(
            in T AMinimum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            if (WeakOrdering.Compare(AMinimum, AParam) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    $@"Value must be at least {AMinimum}."
                );
            }

            return AParam;
        }

        /// <summary>
        /// Require that a value is greater than to a minimum.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AMinimum">The exclusive minimum.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is less than or equal to <paramref name="AMinimum"/>.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static T GreaterThan<T>(
            in T AMinimum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            if (WeakOrdering.Compare(AMinimum, AParam) >= 0)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    $@"Value must be greater than {AMinimum}."
                );
            }

            return AParam;
        }

        /// <summary>
        /// Require that a value is less than or equal to a maximum.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AMaximum">The inclusive maximum.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is greater than <paramref name="AMaximum"/>.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static T AtMost<T>(
            in T AMaximum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            if (WeakOrdering.Compare(AMaximum, AParam) < 0)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    $@"Value must be at most {AMaximum}."
                );
            }

            return AParam;
        }

        /// <summary>
        /// Require that a value is less than to a maximum.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AMaximum">The exclusive maximum.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is greater than or equal to <paramref name="AMaximum"/>.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static T LessThan<T>(
            in T AMaximum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            if (WeakOrdering.Compare(AMaximum, AParam) <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    $@"Value must be less than {AMaximum}."
                );
            }

            return AParam;
        }

        /// <summary>
        /// Require that a value is inside an inclusive range.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AMinimum">The inclusive minimum.></param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <param name="AMaximum">The inclusive maximum.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AParam"/> is outside
        /// [<paramref name="AMinimum"/>, <paramref name="AMaximum"/>].
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [DebuggerHidden]
        public static T Between<T>(
            in T AMinimum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName,
            in T AMaximum
        ) where T : IComparable<T>
        {
            if (WeakOrdering.Compare(AMinimum, AParam) > 0
                || WeakOrdering.Compare(AMaximum, AParam) < 0)
            {
                throw new ArgumentOutOfRangeException(
                    AParamName,
                    AParam,
                    $@"Value must be within [{AMinimum}, {AMaximum}]."
                );
            }

            return AParam;
        }
    }
}
