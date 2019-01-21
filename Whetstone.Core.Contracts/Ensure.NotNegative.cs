using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
	public partial class Ensure
	{
		/// <summary>
        /// Ensure that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void NotNegative(
            in sbyte AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam >= 0,
				$@"{AParamName} is negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void NotNegative(
            in short AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam >= 0,
				$@"{AParamName} is negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void NotNegative(
            in int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam >= 0,
				$@"{AParamName} is negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void NotNegative(
            in long AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam >= 0L,
				$@"{AParamName} is negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void NotNegative(
            in float AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam >= 0f,
				$@"{AParamName} is negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void NotNegative(
            in double AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam >= 0,
				$@"{AParamName} is negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is not negative.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void NotNegative(
            in decimal AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam >= 0m,
				$@"{AParamName} is negative.",
				@"This indicates a contract violation."
			);
        }

	}
}
