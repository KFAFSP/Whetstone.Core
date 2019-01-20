using System;
using System.Threading;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented
// ReSharper disable AssignmentIsFullyDiscarded

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(Barrier))]
    public sealed class BarrierTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void Constructor_NotPositive_ThrowsArgumentOutOfRangeException(int ACount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Barrier(ACount));
        }

        [Test]
        public void Constructor_WaitsForCount()
        {
            using (var barrier = new Barrier(3))
            {
                Assert.That(barrier.WaitingFor, Is.EqualTo(3));
            }
        }

        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var barrier = new Barrier(3);

            var awaiters = new []
            {
                barrier.WaitAsync(),
                barrier.WaitAsync()
            };

            barrier.Dispose();

            foreach (var awaiter in awaiters)
            {
                TaskAssert.Faulted<ObjectDisposedException>(awaiter);
            }
        }

        [Test]
        public void WaitAsync_Disposed_ThrowsObjectDisposedException()
        {
            var barrier = new Barrier(3);
            barrier.Dispose();

            var awaiter = barrier.WaitAsync();

            TaskAssert.Faulted<ObjectDisposedException>(awaiter);
        }

        [Test]
        public void WaitAsync_Synchronizes()
        {
            using (var barrier = new Barrier(3))
            {
                var t1 = barrier.WaitAsync();
                TaskAssert.DoesNotEnd(t1);
                Assert.That(barrier.WaitingFor, Is.EqualTo(2));

                var t2 = barrier.WaitAsync();
                TaskAssert.DoesNotEnd(t2);
                Assert.That(barrier.WaitingFor, Is.EqualTo(1));

                var t3 = barrier.WaitAsync();
                TaskAssert.Completed(t1);
                TaskAssert.Completed(t2);
                TaskAssert.Completed(t3);
            }
        }

        [Test]
        public void WaitAsync_Waiting_CanRetract()
        {
            using (var cts = new CancellationTokenSource())
            using (var barrier = new Barrier(3))
            {
                var t1 = barrier.WaitAsync();
                TaskAssert.DoesNotEnd(t1);
                Assert.That(barrier.WaitingFor, Is.EqualTo(2));

                var t2 = barrier.WaitAsync(cts.Token);
                TaskAssert.DoesNotEnd(t2);
                Assert.That(barrier.WaitingFor, Is.EqualTo(1));

                cts.Cancel();
                TaskAssert.Cancelled(t2);
                Assert.That(barrier.WaitingFor, Is.EqualTo(2));
            }
        }
    }
}