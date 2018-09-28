﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Value type keeping the semantics of an optional value.
    /// </summary>
    /// <typeparam name="T">The type of the contained value.</typeparam>
    [PublicAPI]
    public readonly partial struct Optional<T> : IEquatable<Optional<T>>
    {
        /// <summary>
        /// The exception message string for "optional is absent".
        /// </summary>
        const string C_Absent = "Optional is absent.";

        /// <summary>
        /// Get a present <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="AValue">The present value.</param>
        /// <returns>
        /// A present <see cref="Optional{T}"/> with value <paramref name="AValue"/>.
        /// </returns>
        [Pure]
        public static Optional<T> Present([NoEnumeration] T AValue) => new Optional<T>(AValue);
        /// <summary>
        /// Get an absent <see cref="Optional{T}"/>.
        /// </summary>
        public static Optional<T> Absent => default;

        /// <summary>
        /// Initialize a present <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="AValue">The present value.</param>
        Optional([NoEnumeration] T AValue)
        {
            FValue = AValue;
            IsPresent = true;
        }

        /// <summary>
        /// The contained value.
        /// </summary>
        readonly T FValue;
        /// <summary>
        /// Unpacks the contained value.
        /// </summary>
        internal T Unpack
        {
            [Pure]
            get
            {
                Debug.Assert(
                    IsPresent,
                    C_Absent,
                    "This indicates a severe logic error."
                );

                return FValue;
            }
        }

        /// <summary>
        /// Try to perform a cast on this <see cref="Optional{T}"/>'s contained value.
        /// </summary>
        /// <typeparam name="TOut">The cast target type.</typeparam>
        /// <returns>
        /// An absent <see cref="Optional{T}"/> if the value is absent or mismatching; otherwise a
        /// present <see cref="Optional{T}"/> with the casted non-<see langword="null"/> result.
        /// </returns>
        public Optional<TOut> ThatIs<TOut>() where TOut : T
            => IsPresent && Unpack is TOut res
                ? Optional<TOut>.Present(res)
                : Optional<TOut>.Absent;

        #region IEquatable<Optional<T>>
        /// <inheritdoc />
        /// <remarks>
        /// Two <see cref="Optional{T}"/> instances are equal if either both are absent or both are
        /// present and their contained <see cref="Value"/>s are equal using the default
        /// (<see cref="EqualityComparer{T}.Default"/>) equality comparison.
        /// </remarks>
        [Pure]
        public bool Equals(Optional<T> AOther)
        {
            if (!IsPresent)
            {
                // Absent optionals of same type are equal to each other, but nothing else.
                return !AOther.IsPresent;
            }

            // Default equality comparison for T.
            return AOther.IsPresent && EqualityComparer<T>.Default.Equals(Unpack, AOther.Unpack);
        }
        #endregion

        #region System.Object overrides
        /// <inheritdoc />
        [Pure]
        public override bool Equals(object AOther)
        {
            switch (AOther)
            {
                case Optional<T> opt:
                    return Equals(opt);

                default:
                    return false;
            }
        }
        /// <inheritdoc />
        /// <remarks>
        /// The hash code of an absent <see cref="Optional{T}"/> is <c>0</c>, while a present
        /// <see cref="Optional{T}"/> will have the same hash code as the default hashing function
        /// (<see cref="EqualityComparer{T}.Default"/>) for it's contained <see cref="Value"/>
        /// yields.
        /// </remarks>
        [Pure]
        public override int GetHashCode()
            => IsPresent ? EqualityComparer<T>.Default.GetHashCode(Unpack) : 0;
        /// <inheritdoc />
        /// <remarks>
        /// An absent <see cref="Optional{T}"/> will return a string that indicates it's type and
        /// absence, while a present <see cref="Optional{T}"/> will simply delegate to it's
        /// contained <see cref="Value"/>.
        /// </remarks>
        [Pure]
        public override string ToString()
            => IsPresent ? $"{Unpack}" : $"Optional<{typeof(T).Name}>.Absent";
        #endregion

        /// <summary>
        /// Gets a value indicating whether the optional value is present.
        /// </summary>
        public bool IsPresent { get; }
        /// <summary>
        /// Gets the value if present; otherwise throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">Optional is absent.</exception>
        public T Value => IsPresent ? Unpack : throw new InvalidOperationException(C_Absent);

        /// <summary>
        /// Implicitly initialize a present <see cref="Optional{T}"/> from a value.
        /// </summary>
        /// <param name="AValue">The present value.</param>
        [Pure]
        public static implicit operator Optional<T>([NoEnumeration] T AValue) => Present(AValue);
        /// <summary>
        /// Explicitly unwrap the <see cref="Value"/> of an <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="AOptional">The <see cref="Optional{T}"/>.</param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="AOptional"/> is absent.
        /// </exception>
        [Pure]
        public static explicit operator T(Optional<T> AOptional) => AOptional.Value;

        /// <summary>
        /// Check whether two <see cref="Optional{T}"/> instances are equal.
        /// </summary>
        /// <param name="ALhs">The left hand side.</param>
        /// <param name="ARhs">The right hand side.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ALhs"/> and <paramref name="ARhs"/> are
        /// equal; oterhwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool operator ==(Optional<T> ALhs, Optional<T> ARhs) => ALhs.Equals(ARhs);
        /// <summary>
        /// Check whether two <see cref="Optional{T}"/> instances are unequal.
        /// </summary>
        /// <param name="ALhs">The left hand side.</param>
        /// <param name="ARhs">The right hand side.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ALhs"/> and <paramref name="ARhs"/> are
        /// unequal; oterhwise <see langword="false"/>.
        /// </returns>
        [Pure]
        public static bool operator !=(Optional<T> ALhs, Optional<T> ARhs) => !ALhs.Equals(ARhs);
    }

    /// <summary>
    /// Provides static convenience methods for using the <see cref="Optional{T}"/> generic type
    /// with inferred type arguments.
    /// </summary>
    [PublicAPI]
    public static partial class Optional { }
}
