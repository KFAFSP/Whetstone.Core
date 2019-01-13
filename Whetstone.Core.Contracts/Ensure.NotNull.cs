using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public static partial class Ensure
    {
        /// <summary>
        /// Assert that a parameter is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [ContractAnnotation("AParam: null => halt")]
        [AssertionMethod]
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNull<T>(
            [NoEnumeration] [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T AParam,
            [NotNull] [InvokerParameterName] string AParamName
        ) where T : class
        {
            Debug.Assert(
				AParam != null,
				$@"{AParamName} is null.",
				"This indicates a contract violation."
            );
        }
    }
}
