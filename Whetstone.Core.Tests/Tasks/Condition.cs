using System;
using System.Threading;

using NUnit.Framework;

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(Condition))]
    public sealed class ConditionTests
    {
        [Test]
        public void True_IsTrue()
        {
            using (var cond = Condition.True())
            {
                Assert.That(cond.Value);
            }
        }

        [Test]
        public void False_IsFalse()
        {
            using (var cond = Condition.False())
            {
                Assert.That(!cond.Value);
            }
        }

        [Test]
        public void Constructor_Default_IsFalse()
        {
            using (var cond = new Condition())
            {
                Assert.That(!cond.Value);
            }
        }

        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var cond = new Condition();

            var awaiter = cond.WaitAsync();

            cond.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(awaiter);
        }

        [Test]
        public void TrySet_Disposed_ReturnsFalse()
        {
            var cond = new Condition();
            cond.Dispose();

            Assert.That(!cond.TrySet());
        }

        [Test]
        public void TrySet_Set_ReturnsFalse()
        {
            using (var cond = Condition.True())
            {
                Assert.That(!cond.TrySet());
            }
        }

        [Test]
        public void TrySet_Reset_ReturnsTrue()
        {
            using (var cond = Condition.False())
            {
                Assert.That(cond.TrySet());
            }
        }

        [Test]
        public void Set_Disposed_ThrowsObjectDisposedException()
        {
            var cond = new Condition();
            cond.Dispose();

            Assert.Throws<ObjectDisposedException>(() => cond.Set());
        }

        [Test]
        public void Set_Set_ThrowsInvalidOperationException()
        {
            using (var cond = Condition.True())
            {
                Assert.Throws<InvalidOperationException>(() => cond.Set());
            }
        }

        [Test]
        public void Set_Reset_Sets()
        {
            using (var cond = Condition.False())
            {
                cond.Set();
                Assert.That(cond.Value);
            }
        }

        [Test]
        public void TryReset_Disposed_ReturnsFalse()
        {
            var cond = new Condition();
            cond.Dispose();

            Assert.That(!cond.TryReset());
        }

        [Test]
        public void TryReset_Reset_ReturnsFalse()
        {
            using (var cond = Condition.True())
            {
                Assert.That(cond.TryReset());
            }
        }

        [Test]
        public void TryReset_Reset_ReturnsTrue()
        {
            using (var cond = Condition.False())
            {
                Assert.That(!cond.TryReset());
            }
        }

        [Test]
        public void Reset_Disposed_ThrowsObjectDisposedException()
        {
            var cond = new Condition();
            cond.Dispose();

            Assert.Throws<ObjectDisposedException>(() => cond.Reset());
        }

        [Test]
        public void Reset_Reset_ThrowsInvalidOperationException()
        {
            using (var cond = Condition.False())
            {
                Assert.Throws<InvalidOperationException>(() => cond.Reset());
            }
        }

        [Test]
        public void Reset_Reset_Resets()
        {
            using (var cond = Condition.True())
            {
                cond.Reset();
                Assert.That(!cond.Value);
            }
        }

        [Test]
        public void WaitAsync_Disposed_ThrowsObjectDisposedException()
        {
            var cond = new Condition();
            cond.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(cond.WaitAsync());
        }

        [Test]
        public void WaitAsync_Reset_Cancelable()
        {
            using (var cts = new CancellationTokenSource())
            using (var cond = Condition.False())
            {
                var awaiter = cond.WaitAsync(cts.Token);

                TaskAssert.DoesNotEnd(awaiter);
                cts.Cancel();
                TaskAssert.Cancelled(awaiter);
            }
        }

        [Test]
        public void WaitAsync_Set_ReturnsCompletedTask()
        {
            using (var cond = Condition.True())
            {
                TaskAssert.Completed(cond.WaitAsync());
            }
        }

        [Test]
        public void WaitAsync_Reset_WaitsForSet()
        {
            using (var cond = Condition.False())
            {
                var awaiter = cond.WaitAsync();

                TaskAssert.DoesNotEnd(awaiter);
                cond.Set();
                TaskAssert.Completed(awaiter);
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void OpImplicit_ToBool(bool AValue)
        {
            bool val = new Condition(AValue);

            Assert.That(val, Is.EqualTo(AValue));
        }
    }
}
