using System;
using System.Linq;
using System.Threading;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Threading")]
    [TestOf(typeof(Queue))]
    public sealed class QueueTests
    {
        [TearDown]
        public void Teardown()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var queue = new Queue();

            using (var cts = new CancellationTokenSource())
            {
                var awaiters = new[]
                {
                    queue.WaitAsync(),
                    queue.WaitAsync(cts.Token),
                    queue.WaitAsync(),
                    queue.WaitAsync()
                };

                cts.Cancel();
                queue.Dispose();

                using (TaskAssert.Completed(awaiters[0])) { }
                TaskAssert.Cancelled(awaiters[1]);

                foreach (var awaiter in awaiters.Skip(2))
                {
                    TaskAssert.Faulted<ObjectDisposedException>(awaiter);
                }
            }
        }

        [Test]
        public void WaitAsync_Disposed_ThrowsObjectDisposedException()
        {
            var queue = new Queue();
            queue.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(queue.WaitAsync());
        }

        [Test]
        public void WaitAsync_Empty_ReturnsCompletedTask()
        {
            using (var queue = new Queue())
            {
                using (TaskAssert.Completed(queue.WaitAsync())) { }
            }
        }

        [Test]
        public void WaitAsync_NotEmpty_WaitsForTurn()
        {
            using (var queue = new Queue())
            {
                var t1 = queue.WaitAsync();
                var t2 = queue.WaitAsync();
                var t3 = queue.WaitAsync();

                using (TaskAssert.Completed(t1))
                {
                    TaskAssert.DoesNotEnd(t2);
                    TaskAssert.DoesNotEnd(t3);
                }

                using (TaskAssert.Completed(t2))
                {
                    TaskAssert.DoesNotEnd(t3);
                }

                using (TaskAssert.Completed(t3))
                {
                    Assert.That(!queue.IsEmpty);
                }

                Assert.That(queue.IsEmpty);
            }
        }

        [Test]
        public void WaitAsync_NotEmpty_CanCedeTurn()
        {
            using (var cts = new CancellationTokenSource())
            using (var queue = new Queue())
            {
                var t1 = queue.WaitAsync();
                var t2 = queue.WaitAsync(cts.Token);
                var t3 = queue.WaitAsync();

                using (TaskAssert.Completed(t1))
                {
                    TaskAssert.DoesNotEnd(t2);
                    TaskAssert.DoesNotEnd(t3);

                    cts.Cancel();
                    TaskAssert.Cancelled(t2);
                    TaskAssert.DoesNotEnd(t3);
                }

                using (TaskAssert.Completed(t3))
                {
                    Assert.That(!queue.IsEmpty);
                }

                Assert.That(queue.IsEmpty);
            }
        }
    }
}
