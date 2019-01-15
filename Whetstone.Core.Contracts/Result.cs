using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Value type representing the return type of a method that might fail without throwing.
    /// </summary>
    /// <remarks>
    /// A <see langword="default"/> <see cref="Result"/> is erroneous with a default error.
    /// </remarks>
    [PublicAPI]
    public readonly partial struct Result : IEquatable<Result>
    {
        /// <summary>
        /// Exception message string for "result is erroneous".
        /// </summary>
        internal const string C_Error = @"Result is erroneous.";
        /// <summary>
        /// Exception message string for "uninitialized result".
        /// </summary>
        internal const string C_Default = @"Uninitialized result.";
        /// <summary>
        /// Exception message string for "tried to create erroneous result without error".
        /// </summary>
        internal const string C_WithoutError = @"Cannot create erroneous result without error.";
        /// <summary>
        /// Get an <see cref="Exception"/> that indicates an uninitialized <see cref="Result"/>.
        /// </summary>
        internal static Exception UninitializedError => new Exception(C_Default);

        /// <summary>
        /// Get a successful <see cref="Result"/>.
        /// </summary>
        public static Result Ok() => new Result(null);
        /// <summary>
        /// Get an erroneous <see cref="Result"/>.
        /// </summary>
        /// <param name="AError">The error.</param>
        /// <returns>
        /// An erroneous <see cref="Result"/> containing <paramref name="AError"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AError"/> is <see langword="null"/>.
        /// </exception>
        [Pure]
        [ContractAnnotation("null => halt")]
        public static Result Fail([NotNull] Exception AError)
        {
            Require.NotNull(AError, nameof(AError));

            return new Result(AError);
        }

        /// <summary>
        /// Initialize a <see cref="Result"/>.
        /// </summary>
        /// <param name="AError">The error.</param>
        /// <remarks>
        /// Initializes a successful result if <paramref name="AError"/> is <see langword="null"/>.
        /// </remarks>
        Result([CanBeNull] Exception AError)
        {
            IsSuccess = AError == null;
            FError = AError;
        }

        /// <summary>
        /// The erroneous result <see cref="Exception"/>.
        /// </summary>
        [CanBeNull]
        readonly Exception FError;
        /// <summary>
        /// Unpacks the contained error.
        /// </summary>
        /// <remarks>
        /// Behaviour is undefined if <see cref="IsSuccess"/> is <see langword="true"/>.
        /// </remarks>
        [NotNull]
        internal Exception UnpackError
        {
            get
            {
                Ensure.That(!IsSuccess, @"Result is success.");
                return FError ?? UninitializedError;
            }
        }

        /// <summary>
        /// Throw <see cref="Error"/> if this <see cref="Result"/> is erroneous.
        /// </summary>
        /// <exception cref="Exception">The <see cref="Error"/> if any.</exception>
        [ExcludeFromCodeCoverage]
        public void ThrowIfError()
        {
            if (!IsSuccess) throw UnpackError;
        }

        /// <summary>
        /// Perform an <see cref="Action"/> if this <see cref="Result"/> is successful.
        /// </summary>
        /// <param name="AHandler">The <see cref="Action"/>.</param>
        /// <returns><c>this</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AHandler"/> is <see langword="null"/>.
        /// </exception>
        public Result OnSuccess(
            [NotNull] [InstantHandle] Action AHandler
        )
        {
            Require.NotNull(AHandler, nameof(AHandler));

            if (IsSuccess)
            {
                AHandler();
            }

            return this;
        }

        /// <summary>
        /// Perform an <see cref="Action"/> on the contained <see cref="Error"/> if this
        /// <see cref="Result"/> is erroneous.
        /// </summary>
        /// <param name="AHandler">The <see cref="Action{T}"/>.</param>
        /// <returns><c>this</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AHandler"/> is <see langword="null"/>.
        /// </exception>
        public Result OnError(
            [NotNull] [InstantHandle] Action<Exception> AHandler
        )
        {
            Require.NotNull(AHandler, nameof(AHandler));

            if (!IsSuccess)
            {
                AHandler(UnpackError);
            }

            return this;
        }
        /// <summary>
        /// Perform an <see cref="Action"/> on the contained <see cref="Error"/> if this
        /// <see cref="Result"/> is erroneous and <see cref="Error"/> has a specific type.
        /// </summary>
        /// <typeparam name="TException">The <see cref="Exception"/> type.</typeparam>
        /// <param name="AHandler">The <see cref="Action{T}"/>.</param>
        /// <returns><c>this</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AHandler"/> is <see langword="null"/>.
        /// </exception>
        public Result OnError<TException>(
            [NotNull] [InstantHandle] Action<TException> AHandler
        ) where TException : Exception
        {
            Require.NotNull(AHandler, nameof(AHandler));

            if (!IsSuccess && UnpackError is TException error)
            {
                AHandler(error);
            }

            return this;
        }

        #region IEquatable<Result>
        /// <inheritdoc />
        /// <remarks>
        /// Two <see cref="Result"/>s are equal if either both are successful or both are erroneous
        /// and their <see cref="Error"/>s are the same instance.
        /// </remarks>
        [Pure]
        public bool Equals(Result AResult)
        {
            if (IsSuccess)
            {
                return AResult.IsSuccess;
            }

            return !AResult.IsSuccess && ReferenceEquals(UnpackError, AResult.UnpackError);
        }
        #endregion

        #region System.Object overrides
        /// <inheritdoc />
        /// <remarks>See <see cref="Equals(Result)"/> for details on equality.</remarks>
        [Pure]
        public override bool Equals(object AOther)
        {
            switch (AOther)
            {
                case Result result:
                    return Equals(result);

                default:
                    return false;
            }
        }
        /// <inheritdoc />
        /// <remarks>
        /// For a successful <see cref="Result"/> the hash code is always <c>0</c>, otherwise it is
        /// the hash code of <see cref="Error"/>.
        /// </remarks>
        [Pure]
        public override int GetHashCode() => IsSuccess ? 0 : UnpackError.GetHashCode();
        /// <inheritdoc />
        /// <remarks>
        /// A successful <see cref="Result"/> will return "OK", while an erroneous
        /// <see cref="Result"/> will return a string that contains a string representation of
        /// <see cref="Error"/>.
        /// </remarks>
        [Pure]
        public override string ToString() => IsSuccess
            ? "OK"
            : $"ERROR({UnpackError.Message})";
        #endregion

        /// <summary>
        /// Get a value indicating whether this <see cref="Result"/> is successful.
        /// </summary>
        public bool IsSuccess { get; }
        /// <summary>
        /// Get the <see cref="Exception"/> of this <see cref="Result"/>.
        /// </summary>
        /// <remarks>
        /// If <see cref="IsSuccess"/> is <see langword="true"/>, the <see cref="Optional{T}"/>
        /// will be absent. Otherwise it contains the non-<see langword="null"/> error.
        /// </remarks>
        [ItemNotNull]
        public Optional<Exception> Error => !IsSuccess ? Optional.Present(UnpackError) : default;

        /// <summary>
        /// Implicitly get the <see cref="IsSuccess"/> value of this <see cref="Result"/>.
        /// </summary>
        /// <param name="AResult">The <see cref="Result"/>.</param>
        [Pure]
        public static implicit operator bool(Result AResult) => AResult.IsSuccess;

        /// <summary>
        /// Implicitly initialize a successful <see cref="Result"/>.
        /// </summary>
        /// <param name="AValue">Must be <see langword="true"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="AValue"/> is <see langword="false"/>.
        /// </exception>
        [Pure]
        [ContractAnnotation("false => halt")]
        public static implicit operator Result(bool AValue)
        {
            if (!AValue)
            {
                throw new ArgumentException(C_WithoutError, nameof(AValue));
            }

            return Ok();
        }
        /// <summary>
        /// Implicitly initialize an erroneous <see cref="Result"/> from an <see cref="Exception"/>.
        /// </summary>
        /// <param name="AException">The <see cref="Exception"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AException"/> is <see langword="null"/>.
        /// </exception>
        [Pure]
        [ContractAnnotation("null => halt")]
        public static implicit operator Result([NotNull] Exception AException)
            => Fail(AException);
    }

    /// <summary>
    /// Value type representing the return type of a method returning a value that might fail
    /// without throwing.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <remarks>
    /// A <see langword="default"/> <see cref="Result{T}"/> is erroneous with a default error.
    /// </remarks>
    [PublicAPI]
    public readonly struct Result<T> : IEquatable<Result<T>>
    {
        /// <summary>
        /// Get a successful <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="AValue">The result value.</param>
        public static Result<T> Ok(T AValue) => new Result<T>(AValue);
        /// <summary>
        /// Get an erroneous <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="AError">The error.</param>
        /// <returns>
        /// An erroneous <see cref="Result{T}"/> containing <paramref name="AError"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AError"/> is <see langword="null"/>.
        /// </exception>
        [Pure]
        [ContractAnnotation("null => halt")]
        public static Result<T> Fail([NotNull] Exception AError)
        {
            Require.NotNull(AError, nameof(AError));

            return new Result<T>(AError);
        }

        /// <summary>
        /// Initialize a successful <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="AValue">The result value.</param>
        Result(T AValue)
        {
            IsSuccess = true;
            FError = null;
            FValue = AValue;
        }
        /// <summary>
        /// Initialize an erroneous <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="AError">The error.</param>
        Result([NotNull] Exception AError)
        {
            Ensure.NotNull(AError, nameof(AError));

            IsSuccess = false;
            FError = AError;
            FValue = default;
        }

        /// <summary>
        /// The successful result value.
        /// </summary>
        readonly T FValue;
        /// <summary>
        /// Unpacks the contained error.
        /// </summary>
        /// <remarks>
        /// Behaviour is undefined if <see cref="IsSuccess"/> is <see langword="true"/>.
        /// </remarks>
        internal T UnpackValue
        {
            get
            {
                Ensure.That(IsSuccess, Result.C_Error);

                return FValue;
            }
        }

        /// <inheritdoc cref="Result.FError"/>
        [CanBeNull]
        readonly Exception FError;
        /// <inheritdoc cref="Result.UnpackError"/>
        [NotNull]
        internal Exception UnpackError
        {
            get
            {
                Ensure.That(!IsSuccess, @"Result is success.");
                return FError ?? Result.UninitializedError;
            }
        }

        /// <summary>
        /// Throw <see cref="Error"/> if this <see cref="Result{T}"/> is erroneous.
        /// </summary>
        /// <exception cref="Exception">The <see cref="Error"/> if any.</exception>
        [ExcludeFromCodeCoverage]
        public void ThrowIfError()
        {
            if (!IsSuccess) throw UnpackError;
        }

        /// <summary>
        /// Perform an <see cref="Action"/> if this <see cref="Result{T}"/> is successful.
        /// </summary>
        /// <param name="AHandler">The <see cref="Action"/>.</param>
        /// <returns><c>this</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AHandler"/> is <see langword="null"/>.
        /// </exception>
        public Result<T> OnSuccess(
            [NotNull] [InstantHandle] Action<T> AHandler
        )
        {
            Require.NotNull(AHandler, nameof(AHandler));

            if (IsSuccess)
            {
                AHandler(UnpackValue);
            }

            return this;
        }

        /// <summary>
        /// Perform an <see cref="Action"/> on the contained <see cref="Error"/> if this
        /// <see cref="Result{T}"/> is erroneous.
        /// </summary>
        /// <param name="AHandler">The <see cref="Action{T}"/>.</param>
        /// <returns><c>this</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AHandler"/> is <see langword="null"/>.
        /// </exception>
        public Result<T> OnError(
            [NotNull] [InstantHandle] Action<Exception> AHandler
        )
        {
            Require.NotNull(AHandler, nameof(AHandler));

            if (!IsSuccess)
            {
                AHandler(UnpackError);
            }

            return this;
        }
        /// <summary>
        /// Perform an <see cref="Action"/> on the contained <see cref="Error"/> if this
        /// <see cref="Result{T}"/> is erroneous and <see cref="Error"/> has a specific type.
        /// </summary>
        /// <typeparam name="TException">The <see cref="Exception"/> type.</typeparam>
        /// <param name="AHandler">The <see cref="Action{T}"/>.</param>
        /// <returns><c>this</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AHandler"/> is <see langword="null"/>.
        /// </exception>
        public Result<T> OnError<TException>(
            [NotNull] [InstantHandle] Action<TException> AHandler
        ) where TException : Exception
        {
            Require.NotNull(AHandler, nameof(AHandler));

            if (!IsSuccess && UnpackError is TException error)
            {
                AHandler(error);
            }

            return this;
        }

        #region IEquatable<Result>
        /// <inheritdoc />
        /// <remarks>
        /// Two <see cref="Result{T}"/>s are equal if either both are successful and their values
        /// are equal according to the default (<see cref="EqualityComparer{T}.Default"/>) equality
        /// comparison; or both are erroneous and their <see cref="Error"/>s are the same instance.
        /// </remarks>
        [Pure]
        public bool Equals(Result<T> AResult)
        {
            if (IsSuccess)
            {
                return AResult.IsSuccess
                    && EqualityComparer<T>.Default.Equals(UnpackValue, AResult.UnpackValue);
            }

            return !AResult.IsSuccess && ReferenceEquals(UnpackError, AResult.UnpackError);
        }
        #endregion

        #region System.Object overrides
        /// <inheritdoc />
        /// <remarks>See <see cref="Equals(Result{T})"/> for details on equality.</remarks>
        [Pure]
        public override bool Equals(object AOther)
        {
            switch (AOther)
            {
            case Result<T> result:
                return Equals(result);

            default:
                return false;
            }
        }
        /// <inheritdoc />
        /// <remarks>
        /// For a successful <see cref="Result{T}"/> the hash code is determined by the default
        /// (<see cref="EqualityComparer{T}.Default"/>) hash function applied on the contained
        /// value, otherwise it is the hash code of <see cref="Error"/>.
        /// </remarks>
        [Pure]
        public override int GetHashCode() => IsSuccess
            ? EqualityComparer<T>.Default.GetHashCode(UnpackValue)
            : UnpackError.GetHashCode();
        /// <inheritdoc />
        /// <remarks>
        /// A successful <see cref="Result{T}"/> will return a string that contains a string
        /// representation of <see cref="Value"/>, while an erroneous <see cref="Result{T}"/> will
        /// return a string that contains a string representation of <see cref="Error"/>.
        /// </remarks>
        [Pure]
        public override string ToString() => IsSuccess
            ? $"OK({UnpackValue})"
            : $"ERROR({UnpackError.Message})";
        #endregion

        /// <summary>
        /// Get a value indicating whether this <see cref="Result{T}"/> is successful.
        /// </summary>
        public bool IsSuccess { get; }
        /// <summary>
        /// Get the <see cref="Exception"/> of this <see cref="Result{T}"/>.
        /// </summary>
        /// <inheritdoc cref="Result.Error" select="remarks"/>
        [ItemNotNull]
        public Optional<Exception> Error => !IsSuccess ? Optional.Present(UnpackError) : default;
        /// <summary>
        /// Get the successful value of this <see cref="Result{T}"/>; otherwise throws the contained
        /// <see cref="Exception"/>.
        /// </summary>
        /// <exception cref="Exception">The <see cref="Result{T}"/> is erroneous.</exception>
        public T Value => IsSuccess ? UnpackValue : throw UnpackError;

        /// <summary>
        /// Implicitly get the <see cref="IsSuccess"/> value of this <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="AResult">The <see cref="Result{T}"/>.</param>
        [Pure]
        public static implicit operator bool(Result<T> AResult) => AResult.IsSuccess;

        /// <summary>
        /// Explicitly get the <see cref="Value"/> of this <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="AResult">The <see cref="Result{T}"/>.</param>
        /// <exception cref="Exception">The <see cref="Result{T}"/> is erroneous.</exception>
        [Pure]
        public static explicit operator T(Result<T> AResult) => AResult.Value;

        /// <summary>
        /// Implicitly initialize a successful <see cref="Result{T}"/>.
        /// </summary>
        /// <param name="AValue">The result value.</param>
        [Pure]
        public static implicit operator Result<T>([NoEnumeration] T AValue) => Ok(AValue);
        /// <summary>
        /// Implicitly initialize an erroneous <see cref="Result{T}"/> from
        /// an <see cref="Exception"/>.
        /// </summary>
        /// <param name="AException">The <see cref="Exception"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AException"/> is <see langword="null"/>.
        /// </exception>
        [Pure]
        [ContractAnnotation("null => halt")]
        public static implicit operator Result<T>([NotNull] Exception AException)
            => Fail(AException);
    }
}
