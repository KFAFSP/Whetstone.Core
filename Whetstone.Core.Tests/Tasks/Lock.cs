using System;
using System.Linq;
using System.Threading;

using NUnit.Framework;

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(Lock))]
    public sealed class LockTests
    {
        [TearDown]
        public void TearDown()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        [Test]
        public void Constructor_Default_IsReleased()
        {
            using (var lck = new Lock())
            {
                Assert.That(!lck.IsAcquired);
            }
        }

        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var lck = new Lock();

            var awaiters = new[]
            {
                TaskAssert.Detach(lck.WaitAsync),
                TaskAssert.Detach(lck.WaitAsync),
                TaskAssert.Detach(lck.WaitAsync)
            };

            lck.Dispose();

            using (TaskAssert.Completed(awaiters.First())) { }

            foreach (var awaiter in awaiters.Skip(1))
            {
                TaskAssert.Faulted<ObjectDisposedException>(awaiter);
            }
        }

        [Test]
        public void WaitAsync_Disposed_ThrowsObjectDisposedException()
        {
            var lck = new Lock();
            lck.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(lck.WaitAsync());
        }

        [Test]
        public void WaitAsync_Released_ReturnsCompletedTask()
        {
            using (var lck = new Lock())
            {
                using (TaskAssert.Completed(lck.WaitAsync())) { }
            }
        }

        [Test]
        public void WaitAsync_Acquired_WaitsForTurn()
        {
            using (var lck = new Lock())
            {
                var t1 = TaskAssert.Detach(lck.WaitAsync);
                var t2 = TaskAssert.Detach(lck.WaitAsync);

                using (TaskAssert.Completed(t1))
                {
                    TaskAssert.DoesNotEnd(t2);
                }

                using (TaskAssert.Completed(t2)) { }
            }
        }

        [Test]
        public void WaitAsync_Acquired_Reenters()
        {
            using (var lck = new Lock())
            {
                var h1 = TaskAssert.Completed(lck.WaitAsync());
                var t1 = TaskAssert.Detach(lck.WaitAsync);
                var h2 = TaskAssert.Completed(lck.WaitAsync());

                TaskAssert.DoesNotEnd(t1);

                h1.Dispose();
                TaskAssert.DoesNotEnd(t1);

                h2.Dispose();
                using (TaskAssert.Completed(t1)) { }
            }
        }

        [Test]
        public void WaitAsync_Acquired_CanCedeTurn()
        {
            using (var cts = new CancellationTokenSource())
            using (var lck = new Lock())
            {
                var t1 = TaskAssert.Detach(lck.WaitAsync);
                // NOTE: We wait for the task to complete.
                // ReSharper disable AccessToDisposedClosure
                var t2 = TaskAssert.Detach(() => lck.WaitAsync(cts.Token));
                // ReSharper enable AccessToDisposedClosure
                var t3 = TaskAssert.Detach(lck.WaitAsync);

                TaskAssert.DoesNotEnd(t2);
                TaskAssert.DoesNotEnd(t3);

                cts.Cancel();
                TaskAssert.Cancelled(t2);
                TaskAssert.DoesNotEnd(t3);

                using (TaskAssert.Completed(t1)) { }
                using (TaskAssert.Completed(t3)) { }
            }
        }

        [Test]
        public void IsAcquiredHere_Get()
        {
            using (var lck = new Lock())
            {
                Assert.That(!lck.IsAcquiredHere);
                using (TaskAssert.Completed(lck.WaitAsync()))
                {
                    Assert.That(lck.IsAcquiredHere);
                }
            }
        }
    }
}
