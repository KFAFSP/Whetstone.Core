using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial class Ensure
    {
        /// <summary>
        /// Require that a value is greater than or equal to a minimum.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AMinimum">The inclusive minimum.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void AtLeast<T>(
            in T AMinimum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            Debug.Assert(
                WeakOrdering.Compare(AMinimum, AParam) <= 0,
                $@"{AParamName} is less than {AMinimum}.",
                @"This indicates a contract violation."
            );
        }

        /// <summary>
        /// Require that a value is greater than to a minimum.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AMinimum">The exclusive minimum.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void GreaterThan<T>(
            in T AMinimum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            Debug.Assert(
                WeakOrdering.Compare(AMinimum, AParam) < 0,
                $@"{AParamName} is less than or equal to {AMinimum}.",
                @"This indicates a contract violation."
            );
        }

        /// <summary>
        /// Require that a value is less than or equal to a maximum.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AMaximum">The inclusive maximum.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void AtMost<T>(
            in T AMaximum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            Debug.Assert(
                WeakOrdering.Compare(AMaximum, AParam) >= 0,
                $@"{AParamName} is greater than {AMaximum}.",
                @"This indicates a contract violation."
            );
        }

        /// <summary>
        /// Require that a value is less than to a maximum.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AMaximum">The exclusive maximum.</param>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void LessThan<T>(
            in T AMaximum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : IComparable<T>
        {
            Debug.Assert(
                WeakOrdering.Compare(AMaximum, AParam) > 0,
                $@"{AParamName} is greater than or equal to {AMaximum}.",
                @"This indicates a contract violation."
            );
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
        [Conditional("DEBUG")]
        [DebuggerHidden]
        public static void Between<T>(
            in T AMinimum,
            in T AParam,
            [NotNull] [InvokerParameterName] string AParamName,
            in T AMaximum
        ) where T : IComparable<T>
        {
            Debug.Assert(
                WeakOrdering.Compare(AMinimum, AParam) <= 0
                    && WeakOrdering.Compare(AMaximum, AParam) >= 0,
                $@"{AParamName} is outside [{AMinimum}, {AMaximum}]",
                @"This indicates a contract violation."
            );
        }
    }
}
