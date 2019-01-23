using System;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Base class for handles used in synchronization primitives.
    /// </summary>
    [PublicAPI]
    public abstract class SynchronizationHandle : IDisposable
    {
        sealed class DummyHandle : SynchronizationHandle
        {
            public DummyHandle()
            {
                Dispose();
            }

            #region SynchronizationHandle overrides
            /// <inheritdoc />
            protected override void Release() { }
            #endregion
        }

        /// <summary>
        /// Get the released dummy <see cref="SynchronizationHandle"/>.
        /// </summary>
        [NotNull]
        public static SynchronizationHandle Released { get; } = new DummyHandle();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        ~SynchronizationHandle()
        {
            Ensure.That(
                IsReleased,
                @"Handle not released.",
                @"The handle was garbage collected before it was released."
            );
        }

        /// <summary>
        /// When overridden in a derived class, releases the handle.
        /// </summary>
        protected abstract void Release();

        #region IDisposable
        /// <inheritdoc />
        public void Dispose()
        {
            if (IsReleased) return;

            IsReleased = true;
            Release();
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Get a value indicating whether this handle was released.
        /// </summary>
        public bool IsReleased { get; private set; }
    }
}
