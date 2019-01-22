using System;
using System.Linq;
using System.Threading;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented
// ReSharper disable AssignmentIsFullyDiscarded

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(Quota))]
    public sealed class QuotaTests
    {
        [TearDown]
        public void TearDown()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        [TestCase(0L)]
        [TestCase(-1L)]
        public void Constructor_NotPositive_ThrowsArgumentOutOfRangeException(long AValue)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = new Quota(AValue)
            );
        }

        [Test]
        public void Constructor_Positive_HasQuota()
        {
            using (var quota = new Quota(3L))
            {
                Assert.That(quota.Balance, Is.EqualTo(3L));
            }
        }

        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var quota = new Quota(1L);

            using (var cts = new CancellationTokenSource())
            {
                var awaiters = new[]
                {
                    quota.WaitAsync(),
                    quota.WaitAsync(cts.Token),
                    quota.WaitAsync(),
                    quota.WaitAsync()
                };

                cts.Cancel();
                quota.Dispose();

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
            var quota = new Quota(1L);
            quota.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(quota.WaitAsync());
        }

        [Test]
        public void WaitAsync_Available_ReturnsCompletedTask()
        {
            using (var quota = new Quota(3L))
            {
                var release = new IDisposable[3];

                for (var I = 0; I <= 2; ++I)
                {
                    Assert.That(quota.Active, Is.EqualTo(I));
                    release[I] = TaskAssert.Completed(quota.WaitAsync());
                }

                for (var I = 2; I >= 0; --I)
                {
                    release[I].Dispose();
                    Assert.That(quota.Active, Is.EqualTo(I));
                }
            }
        }

        [Test]
        public void WaitAsync_Exceeded_WaitsForTurn()
        {
            using (var quota = new Quota(1L))
            {
                var t1 = quota.WaitAsync();
                var t2 = quota.WaitAsync();
                var t3 = quota.WaitAsync();

                Assert.That(quota.Balance, Is.EqualTo(-2L));

                using (TaskAssert.Completed(t1))
                {
                    TaskAssert.DoesNotEnd(t2);
                    TaskAssert.DoesNotEnd(t3);
                }

                Assert.That(quota.Balance, Is.EqualTo(-1L));

                using (TaskAssert.Completed(t2))
                {
                    TaskAssert.DoesNotEnd(t3);
                }

                Assert.That(quota.Balance, Is.EqualTo(0L));

                using (TaskAssert.Completed(t3)) { }

                Assert.That(quota.Balance, Is.EqualTo(1L));
            }
        }

        [Test]
        public void WaitAsync_Exceeded_CanCedeTurn()
        {
            using (var cts = new CancellationTokenSource())
            using (var quota = new Quota(1L))
            {
                var t1 = quota.WaitAsync();
                var t2 = quota.WaitAsync(cts.Token);
                var t3 = quota.WaitAsync();

                Assert.That(quota.Balance, Is.EqualTo(-2L));

                cts.Cancel();

                Assert.That(quota.Balance, Is.EqualTo(-1L));

                using (TaskAssert.Completed(t1))
                {
                    TaskAssert.Cancelled(t2);
                    TaskAssert.DoesNotEnd(t3);
                }

                Assert.That(quota.Balance, Is.EqualTo(0L));

                using (TaskAssert.Completed(t3)) { }

                Assert.That(quota.Balance, Is.EqualTo(1L));
            }
        }
    }
}
