using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    /// <summary>
    /// Common delegate for an action that implements <see cref="IDisposable.Dispose()"/>.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="AInstance">The ref-reference of the instance.</param>
    /// <remarks>
    /// Because of value type <typeparamref name="T"/>s, <paramref name="AInstance"/> must be
    /// passed by ref-reference.
    /// </remarks>
    public delegate void DisposeAction<T>(ref T AInstance);

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
        /// <exception cref="TargetInvocationException">
        /// <paramref name="AType"/> ran a static initializer that threw an exception.
        /// </exception>
        [CanBeNull]
        internal static MethodInfo GetImpl([NotNull] Type AType)
        {
            Ensure.NotNull(AType, nameof(AType));

            if (!AType.GetInterfaces().Contains(typeof(IDisposable)))
            {
                // Only interface implementations are allowed.
                return null;
            }

            // Get the implementing method.
            var disposableImpl = AType.GetInterfaceMap(typeof(IDisposable));
            return disposableImpl.TargetMethods[0];
        }

        /// <summary>
        /// Get the dispose <see cref="DisposeAction{T}"/> for instances of the target type.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <returns>
        /// An open delegate to the implementation of the Disposable pattern (if any); otherwise
        /// <see langword="null"/>.
        /// </returns>
        /// <exception cref="TargetInvocationException">
        /// <typeparamref name="T"/> ran a static initializer that threw an exception.
        /// </exception>
        [CanBeNull]
        internal static DisposeAction<T> GetAction<T>()
        {
            var type = typeof(T);
            var impl = GetImpl(type);

            if (impl == null)
            {
                // Not implemented.
                return null;
            }

            if (type.IsValueType)
            {
                // For reference types, instance methods bind to ref-references.
                // NOTE: Does not throw.
                // ReSharper disable once ExceptionNotDocumented
                return (DisposeAction<T>)
                    Delegate.CreateDelegate(typeof(DisposeAction<T>), impl);
            }

            // For reference types, instance methods bind to object references.
            // NOTE: Does not throw.
            // ReSharper disable once ExceptionNotDocumented
            var action = (Action<T>)
                Delegate.CreateDelegate(typeof(Action<T>), impl);

            // Wrap in another delegate that drops the ref-reference.
            return (ref T X) => action(X);
        }

        /// <summary>
        /// Safely dispose of any generic object.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="AObject">A ref-reference to the object.</param>
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
        /// <exception cref="Exception">The dispose action threw an exception.</exception>
        public static void Of<T>(ref T AObject)
        {
            var type = typeof(T);
            if (!type.IsValueType && ReferenceEquals(AObject, null))
            {
                // Null object references are not disposed.
                return;
            }

            Dispose<T>.Action?.Invoke(ref AObject);
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
        static readonly Lazy<DisposeAction<T>> _FAction =
            new Lazy<DisposeAction<T>>(Dispose.GetAction<T>);

        /// <summary>
        /// Get the dispose <see cref="DisposeAction{T}"/> for type <typeparamref name="T"/>.
        /// </summary>
        [CanBeNull]
        public static DisposeAction<T> Action => _FAction.Value;
    }
}
