using System;
using System.Threading;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(Event<>))]
    public sealed class EventTests
    {
        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var evt = new Event<int>();

            var awaiter = evt.WaitAsync();
            evt.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(awaiter);
        }

        [Test]
        public void Post_Disposed_ThrowsObjectDisposedException()
        {
            var evt = new Event<int>();
            evt.Dispose();

            Assert.Throws<ObjectDisposedException>(() => evt.Post(1));
        }

        [Test]
        public void Post_Barrier()
        {
            using (var evt = new Event<int>())
            {
                var before = evt.WaitAsync();
                evt.Post(1);
                var after = evt.WaitAsync();
                evt.Post(2);

                Assert.That(
                    TaskAssert.Completed(before),
                    Is.EqualTo(1)
                );
                Assert.That(
                    TaskAssert.Completed(after),
                    Is.EqualTo(2)
                );
            }
        }

        [Test]
        public void WaitAsync_WaitsForPost()
        {
            using (var evt = new Event<int>())
            {
                var awaiter = evt.WaitAsync();

                TaskAssert.DoesNotEnd(awaiter);
                evt.Post(1);
                Assert.That(
                    TaskAssert.Completed(awaiter),
                    Is.EqualTo(1)
                );
            }
        }

        [Test]
        public void WaitAsync_Cancelable()
        {
            using (var cts = new CancellationTokenSource())
            using (var evt = new Event<int>())
            {
                var awaiter = evt.WaitAsync(cts.Token);

                TaskAssert.DoesNotEnd(awaiter);
                cts.Cancel();
                TaskAssert.Cancelled(awaiter);
            }
        }

        [Test]
        public void WaitAsync_Disposed_ReturnsFaultedTask()
        {
            var evt = new Event<int>();
            evt.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(evt.WaitAsync());
        }
    }
}
