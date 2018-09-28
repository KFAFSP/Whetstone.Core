using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class Require
    {
        /// <summary>
        /// The exception message string for "invalid argument type".
        /// </summary>
        const string C_InvalidArgType = @"Invalid argument type.";

        /// <summary>
        /// Require that a parameter is an instance of a specific type.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AParam"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="AParam"/> has an invalid type.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [ContractAnnotation("AParam: null => halt; => notnull")]
        [NotNull]
        [DebuggerHidden]
        public static T Is<T>(
            object AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            NotNull(AParam, AParamName);

            if (AParam is T result)
            {
                // Type constraint is strongly matched!
                return result;
            }

            // Type mismatch.
            throw new ArgumentException(C_InvalidArgType, AParamName);
        }

        /// <summary>
        /// Require that a parameter is constrained to a specific generic type constraint.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AParam"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="AParam"/> has an invalid type.
        /// </exception>
        /// <remarks>
        /// <para>
        /// The difference between this method and <see cref="Is{T}(object, string)"/> lies in the
        /// treatment of <see langword="null"/> parameters. For reference types,
        /// <see langword="null"/> is propagated without error. For value types, an
        /// <see cref="ArgumentNullException"/> is thrown.
        /// </para>
        /// <para>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </para>
        /// </remarks>
        [ContractAnnotation("AParam: notnull => notnull; AParam: null => null")]
        [CanBeNull]
        [DebuggerHidden]
        public static T IsConstrainedBy<T>(
            object AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            if (AParam is T result)
            {
                // Type constraint is strongly matched!
                return result;
            }

            if (ReferenceEquals(AParam, null))
            {
                if (!typeof(T).IsValueType)
                {
                    // Reference type constraint is matched by null!
                    return default;
                }

                // Value type constraint is violated.
                throw new ArgumentNullException(AParamName);
            }

            // Type mismatch.
            throw new ArgumentException(C_InvalidArgType, AParamName);
        }
    }
}
