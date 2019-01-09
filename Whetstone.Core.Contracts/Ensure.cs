using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Provides methods for uniformly handling debug contract validation.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class Ensure
    {
        /// <summary>
        /// Assert that a condition is <see langword="true"/>.
        /// </summary>
        /// <param name="ACondition">The assertion condition.</param>
        /// <param name="AMessage">An optional assertion message.</param>
        [ContractAnnotation("ACondition: false => halt")]
        [AssertionMethod]
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void That(
            [AssertionCondition(AssertionConditionType.IS_TRUE)] bool ACondition,
            [CanBeNull] string AMessage = null
        )
        {
            Debug.Assert(
                ACondition,
                AMessage ?? "Assertion failed.",
                "This indicates a severe logic error."
            );
        }
    }
}
