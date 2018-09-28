using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public partial struct Result
    {
        /// <inheritdoc cref="Result{T}.Ok(T)"/>
        /// <typeparam name="T">The value type.</typeparam>
        [ExcludeFromCodeCoverage]
        [Pure]
        public static Result<T> Ok<T>(T AValue) => Result<T>.Ok(AValue);
        /// <inheritdoc cref="Result{T}.Fail(Exception)"/>
        /// <typeparam name="T">The value type.</typeparam>
        [ExcludeFromCodeCoverage]
        [Pure]
        public static Result<T> Fail<T>([NotNull] Exception AError) => Result<T>.Fail(AError);
    }

    public partial struct Result<T> { }
}
