﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Base class for conform implementors of the <see cref="IDisposable"/> interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This base class handles the <see cref="IsDisposed"/> state of the instance and provides
    /// methods to uniformly deal with it (see <see cref="ThrowIfDisposed()"/>).
    /// </para>
    /// <para>
    /// Any deriving class that adds dispose tasks should overwrite <see cref="Dispose(bool)"/>
    /// given the following requirements and guarantees:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Never call <see cref="Dispose(bool)"/> directly; only ever call <see cref="Dispose()"/>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The <see cref="Dispose(bool)"/> method is guaranteed to be called only once.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The <see cref="Dispose(bool)"/> implementation MUST NEVER throw.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// If the parameter of <see cref="Dispose(bool)"/> is <see langword="false"/>, managed
    /// attributes of the instance may or may not have been disposed already and shall be avoided.
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    [PublicAPI]
    public abstract class Disposable : IDisposable
    {
        /// <summary>
        /// Create a new <see cref="Disposable"/> instance.
        /// </summary>
        /// <param name="ADisposed">
        /// If <see langword="true"/>, the instance starts out disposed.
        /// </param>
        protected Disposable(bool ADisposed = false)
        {
            IsDisposed = ADisposed;

            if (ADisposed)
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Finalize this instance, calling <see cref="Dispose(bool)"/>.
        /// </summary>
        ~Disposable()
        {
            Dispose(false);
            IsDisposed = true;
        }

        /// <summary>
        /// Dispose of this instance by freeing all unmanaged and optionally all managed resources.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See <see cref="Disposable"/> for details on implementation.
        /// </para>
        /// <para>
        /// Never throw in any implementation of <see cref="Dispose(bool)"/>!
        /// Never call <see cref="Dispose(bool)"/> directly; only ever call <see cref="Dispose()"/>.
        /// </para>
        /// </remarks>
        /// <param name="ADisposing">
        /// If <see langword="false"/>, managed attributes of this instance may or may not have been
        /// disposed already and shall be avoided.
        /// </param>
        protected virtual void Dispose(bool ADisposing) { }

        #region IDisposable
        /// <inheritdoc />
        /* Note: We do this correctly, but relieve Dispose(bool) of the check to make overriding
                 even simpler - it wont have to do anything if there is no real work to do, and
                 might as well not even be overridden at all.
         */
        [
            SuppressMessage(
                "Microsoft.Design",
                "CA1063:ImplementIDisposableCorrectly"
            )
        ]
        public void Dispose()
        {
            // Multiple calls to Dispose() SHALL NOT throw.
            if (IsDisposed) return;
            IsDisposed = true;

            Dispose(true);

            // The finalizer SHALL NOT be invoked.
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Throw an <see cref="ObjectDisposedException"/> if this instance is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">This instance is disposed.</exception>
        [DebuggerHidden]
        [ExcludeFromCodeCoverage]
        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
        /// <summary>
        /// Assert that this instance is not disposed.
        /// </summary>
        /// <remarks>
        /// This method is conditionally available only in DEBUG builds.
        /// </remarks>
        [Conditional("DEBUG")]
        [DebuggerHidden]
        [ExcludeFromCodeCoverage]
        public void AssertNotDisposed()
        {
            Debug.Assert(
                !IsDisposed,
                "Instance is disposed.",
                "An operation was attempted on a disposed instance. " +
                "This indicates a severe logic error."
            );
        }

        /// <summary>
        /// Get a value indicating whether this instance is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }
    }
}
