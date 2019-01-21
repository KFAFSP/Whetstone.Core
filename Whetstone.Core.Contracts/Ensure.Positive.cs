using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
	public partial class Ensure
	{
		/// <summary>
        /// Ensure that a parameter is positive.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void Positive(
            in sbyte AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam > 0,
				$@"{AParamName} is zero or negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is positive.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void Positive(
            in short AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam > 0,
				$@"{AParamName} is zero or negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is positive.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void Positive(
            in int AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam > 0,
				$@"{AParamName} is zero or negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is positive.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void Positive(
            in long AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam > 0L,
				$@"{AParamName} is zero or negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is positive.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void Positive(
            in float AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam > 0f,
				$@"{AParamName} is zero or negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is positive.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void Positive(
            in double AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam > 0,
				$@"{AParamName} is zero or negative.",
				@"This indicates a contract violation."
			);
        }

		/// <summary>
        /// Ensure that a parameter is positive.
        /// </summary>
        /// <param name="AParam">The parameter value.</param>
        /// <param name="AParamName">The parameter name.</param>
        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void Positive(
            in decimal AParam,
            [NotNull] [InvokerParameterName] string AParamName
        )
        {
            Debug.Assert(
				AParam > 0m,
				$@"{AParamName} is zero or negative.",
				@"This indicates a contract violation."
			);
        }

	}
}
