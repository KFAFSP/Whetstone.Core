﻿using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Provides a way to synchronize a known number of participants.
    /// </summary>
    [PublicAPI]
    public sealed class Barrier : Disposable, IAwaitable
    {
        int FWaitingFor;
        [NotNull]
        readonly Trigger FRelease = new Trigger();

        /// <summary>
        /// Create a new <see cref="Barrier"/>.
        /// </summary>
        /// <param name="ACount">The number of participants.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ACount"/> is not positive.
        /// </exception>
        public Barrier(int ACount)
        {
            Require.Positive(ACount, nameof(ACount));

            FWaitingFor = ACount;
        }

        #region Disposable overrides
        /// <inheritdoc />
        protected override void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                FRelease.Dispose();
            }

            base.Dispose(ADisposing);
        }
        #endregion

        #region IAwaitable
        /// <inheritdoc />
        public async Task WaitAsync(CancellationToken ACancel)
        {
            ThrowIfDisposed();
            ACancel.ThrowIfCancellationRequested();

            var remaining = Interlocked.Decrement(ref FWaitingFor);
            try
            {
                if (remaining == 0)
                {
                    FRelease.Fire();
                }
                else
                {
                    await FRelease.WaitAsync(ACancel);
                }
            }
            finally
            {
                Interlocked.Increment(ref FWaitingFor);
            }
        }
        #endregion

        /// <summary>
        /// Get the number of participants this barrier is waiting for.
        /// </summary>
        public int WaitingFor => FWaitingFor;
    }
}