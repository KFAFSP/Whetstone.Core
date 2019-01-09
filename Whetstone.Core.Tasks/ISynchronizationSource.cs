using JetBrains.Annotations;

namespace Whetstone.Core.Tasks
{
    /// <summary>
    /// Provides an awaitable synchronization event yielding a <see cref="SynchronizationHandle"/>.
    /// </summary>
    [PublicAPI]
    public interface ISynchronizationSource : IAwaitable<SynchronizationHandle>
    { }
}