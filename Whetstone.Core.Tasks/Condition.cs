using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Represents an awaitable boolean flag.
    /// </summary>
    /// <remarks>
    /// Set and reset operations are synchronized using atomic operations.
    /// </remarks>
    [PublicAPI]
    public sealed class Condition : Disposable, IAwaitable
    {
        const string C_Set = @"Condition is already set.";
        const string C_Reset = @"Condition is already reset.";

        const int C_False = 0;
        const int C_True = 1;
        const int C_Disposed = 2;

        /// <summary>
        /// Create a new <see cref="Condition"/> that is initialized to <see langword="true"/>.
        /// </summary>
        /// <returns>A new <see cref="Condition"/> that is <see langword="true"/>.</returns>
        [NotNull]
        [Pure]
        public static Condition True() => new Condition(true);
        /// <summary>
        /// Create a new <see cref="Condition"/> that is initialized to <see langword="false"/>.
        /// </summary>
        /// <returns>A new <see cref="Condition"/> that is <see langword="false"/>.</returns>
        [NotNull]
        [Pure]
        public static Condition False() => new Condition();

        int FValue;
        [NotNull]
        Era FCurrent;

        /// <summary>
        /// Create a new <see cref="Condition"/>.
        /// </summary>
        /// <param name="AValue">The initial value.</param>
        public Condition(bool AValue = false)
        {
            FValue = AValue ? C_True : C_False;
            FCurrent = AValue ? Era.Ended() : new Era();
        }

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                FValue = C_Disposed;
                FCurrent.Dispose();
            }

            base.Dispose(ADisposing);
        }
        #endregion

        /// <summary>
        /// Attempt to set the condition.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the condition was set; <see langword="false"/> if it was
        /// already set.
        /// </returns>
        public bool TrySet()
        {
            // NOTE: Exception cannot be thrown.
            // ReSharper disable once ExceptionNotDocumented
            if (Interlocked.CompareExchange(
                    ref FValue,
                    C_True,
                    C_False) != C_False)
            {
                return false;
            }

            return FCurrent.TryEnd();
        }
        /// <summary>
        /// Set the condition.
        /// </summary>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        /// <exception cref="InvalidOperationException">The condition is already set.</exception>
        public void Set()
        {
            if (TrySet()) return;

            ThrowIfDisposed();
            throw new InvalidOperationException(C_Set);
        }

        /// <summary>
        /// Attempt to reset the condition.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the condition was reset; <see langword="false"/> if it was
        /// already reset.
        /// </returns>
        public bool TryReset()
        {
            // NOTE: Exception cannot be thrown.
            // ReSharper disable once ExceptionNotDocumented
            if (Interlocked.CompareExchange(
                ref FValue,
                C_False,
                C_True) != C_True)
            {
                return false;
            }

            FCurrent = new Era();
            return true;
        }
        /// <summary>
        /// Reset the condition.
        /// </summary>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        /// <exception cref="InvalidOperationException">The condition is already reset.</exception>
        public void Reset()
        {
            if (TryReset()) return;

            ThrowIfDisposed();
            throw new InvalidOperationException(C_Reset);
        }

        #region IAwaitable
        /// <inheritdoc />
        public Task WaitAsync(CancellationToken ACancel) => FCurrent.WaitAsync(ACancel);
        #endregion

        /// <summary>
        /// Get a value indicating whether the condition is set.
        /// </summary>
        public bool Value => FValue == C_True;

        /// <summary>
        /// Implicitly get the <see cref="Value"/> property.
        /// </summary>
        /// <param name="ACondition">The <see cref="Condition"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ACondition"/> is <see langword="null"/>.
        /// </exception>
        [Pure]
        [ContractAnnotation("null => halt")]
        public static implicit operator bool([NotNull] Condition ACondition)
        {
            Require.NotNull(ACondition, nameof(ACondition));

            return ACondition.Value;
        }
    }
}
