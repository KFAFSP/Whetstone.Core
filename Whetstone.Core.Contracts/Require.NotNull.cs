using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class Require
    {
        /// <summary>
        /// Require that a parameter is not-<see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        /// <returns><paramref name="AParam"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AParam"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// This method is annotated with the <see cref="DebuggerHiddenAttribute"/> and therefore
        /// not part of the stack-trace of the exception that it throws.
        /// </remarks>
        [NotNull]
        [ContractAnnotation("AParam: null => halt; => notnull")]
        [DebuggerHidden]
        public static T NotNull<T>(
            [NoEnumeration] T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : class
            => AParam ?? throw new ArgumentNullException(AParamName);
    }
}
