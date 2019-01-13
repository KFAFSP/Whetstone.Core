using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Static helper class that hands out unique IDs to execution contexts in
    /// <see cref="Task"/>-aware processes.
    /// </summary>
    [PublicAPI]
    public sealed class TaskContext
    {
        /// <summary>
        /// Special task ID that indicates no active task.
        /// </summary>
        public const int C_NoTaskId = 0;
        /// <summary>
        /// Special thread ID that indicates no active thread.
        /// </summary>
        public const int C_NoThreadId = 0;
        /// <summary>
        /// Special context ID that indicates no context.
        /// </summary>
        // NOTE: This is bullshit.
        // ReSharper disable once RedundantCast
        public const long C_NoId = ((long)C_NoThreadId << 32) | (long)C_NoTaskId;

        static int _FLastThreadId = C_NoThreadId - 1;
        static readonly ThreadLocal<int> _FThreadId = new ThreadLocal<int>(() =>
        {
            while (true)
            {
                var id = Interlocked.Increment(ref _FLastThreadId);
                if (id != C_NoThreadId) return id;
            }
        });

        /// <summary>
        /// Get the unique ID of the current <see cref="Task"/>.
        /// </summary>
        /// <remarks>
        /// Can be <see cref="C_NoTaskId"/> if no task is active.
        /// </remarks>
        public static int CurrentTaskId => Task.CurrentId ?? 0;
        /// <summary>
        /// Get the unique ID of the current managed thread.
        /// </summary>
        /// <remarks>
        /// Can never be <see cref="C_NoThreadId"/>.
        /// </remarks>
        public static int CurrentThreadId => _FThreadId.Value;
        /// <summary>
        /// Get the unique ID of the current task context.
        /// </summary>
        /// <remarks>
        /// Can never be <see cref="C_NoId"/>.
        /// </remarks>
        // NOTE: This is bullshit.
        // ReSharper disable once RedundantCast
        public static long CurrentId => ((long)CurrentThreadId << 32) | (long)CurrentTaskId;
    }
}
