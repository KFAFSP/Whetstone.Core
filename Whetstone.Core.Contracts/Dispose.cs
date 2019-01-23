using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Provides helpers for dealing with objects that implement the disposable pattern.
    /// </summary>
    [PublicAPI]
    public static class Dispose
    {
        /// <summary>
        /// Get the <see cref="MethodInfo"/> of the method that implements the disposable pattern
        /// for the specified <see cref="Type"/>, if any.
        /// </summary>
        /// <param name="AType">The <see cref="Type"/>.</param>
        /// <returns>
        /// The <see cref="MethodInfo"/> for the preffered implementor, or <see langword="null"/> if
        /// none found.
        /// </returns>
        [CanBeNull]
        internal static MethodInfo GetImpl([NotNull] Type AType)
        {
            Ensure.NotNull(AType, nameof(AType));

            // Priority 1: Interface implementation takes precedence.
            if (AType.GetInterfaces().Contains(typeof(IDisposable)))
            {
                var disposableImpl = AType.GetInterfaceMap(typeof(IDisposable));
                return disposableImpl.TargetMethods[0];
            }

            // Priority 2: Public Dispose() instance method.
            return AType.GetMethod(
                @"Dispose",
                BindingFlags.Public | BindingFlags.Instance,
                Type.DefaultBinder,
                Type.EmptyTypes,
                null
            );
        }

        /// <summary>
        /// Get the dispose <see cref="Action{T}"/> for instances of the target type.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <returns>
        /// An open delegate to the implementation of the Disposable pattern (if any); otherwise
        /// <see langword="null"/>.
        /// </returns>
        [CanBeNull]
        internal static Action<T> GetAction<T>()
        {
            var type = typeof(T);
            var impl = GetImpl(type);

            return impl != null
                ? (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), impl)
                : null;
        }

        /// <summary>
        /// Safely dispose of any generic object.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="AObject">The object.</param>
        /// <remarks>
        /// <para>
        /// If <paramref name="AObject"/> is <see langword="null"/>, nothing will be peformed.
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements the disposable pattern, the highest ranking
        /// implementation will be performed on <paramref name="AObject"/>; otherwise nothing will
        /// be performed.
        /// </para>
        /// </remarks>
        public static void Of<T>(T AObject)
        {
            var type = typeof(T);
            if (!type.IsValueType && ReferenceEquals(AObject, null))
            {
                return;
            }

            Dispose<T>.Action?.Invoke(AObject);
        }
    }

    /// <summary>
    /// Provides helpers for dealing with objects that implement the disposable pattern.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    [PublicAPI]
    public static class Dispose<T>
    {
        [NotNull]
        readonly static Lazy<Action<T>> _FAction = new Lazy<Action<T>>(Dispose.GetAction<T>);

        /// <summary>
        /// Get the dispose <see cref="Action{T}"/> for type <typeparamref name="T"/>.
        /// </summary>
        [CanBeNull]
        public static Action<T> Action => _FAction.Value;
    }
}
