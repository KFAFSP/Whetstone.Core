using System;

using JetBrains.Annotations;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented

namespace Whetstone.Core.Contracts
{
    [TestFixture]
    [Category("Contracts")]
    [TestOf(typeof(Dispose))]
    public sealed class DisposeTests
    {
        [Test]
        public void Of_NullReference_DoesNothing()
        {
            DisposeTTests.ClassImpl obj = null;
            Dispose.Of(ref obj);
        }

        [Test]
        public void Of_DisposableReference_Disposes()
        {
            var disposable = new DisposeTTests.ClassImpl();
            Dispose.Of(ref disposable);
            Assert.That(disposable.IsDisposed);
        }

        [Test]
        public void Of_DisposableValue_Disposes()
        {
            var disposable = new DisposeTTests.StructImpl();
            Dispose.Of(ref disposable);
            Assert.That(disposable.IsDisposed);
        }
    }

    [TestFixture]
    [Category("Contracts")]
    [TestOf(typeof(Dispose<>))]
    public sealed class DisposeTTests
    {
        [UsedImplicitly]
        public sealed class ClassImpl : IDisposable
        {
            #region IDisposable
            void IDisposable.Dispose()
            {
                IsDisposed = true;
            }
            #endregion

            [UsedImplicitly]
            public void Dispose()
            {
                // It's a trap!
            }

            public bool IsDisposed { get; private set; }
        }

        [UsedImplicitly]
        public struct StructImpl : IDisposable
        {
            public void Dispose()
            {
                IsDisposed = true;
            }

            public bool IsDisposed { get; private set; }
        }

        [UsedImplicitly]
        public sealed class NoImpl { }

        [Test]
        public void Action_InterfaceImpl_ImplMethodAction()
        {
            var obj = new ClassImpl();
            var action = Dispose<ClassImpl>.Action;

            Assert.That(action, Is.Not.Null);
            action(ref obj);
            Assert.That(obj.IsDisposed);
        }

        [Test]
        public void Action_StructImpl_MethodAction()
        {
            var obj = new StructImpl();
            var action = Dispose<StructImpl>.Action;

            Assert.That(action, Is.Not.Null);
            action(ref obj);
            Assert.That(obj.IsDisposed);
        }

        [Test]
        public void Action_NoImpl_Null()
        {
            var action = Dispose<NoImpl>.Action;

            Assert.That(action, Is.Null);
        }
    }
}
