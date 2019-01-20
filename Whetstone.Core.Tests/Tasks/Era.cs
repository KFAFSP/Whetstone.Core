using System;
using System.Threading;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(Era))]
    public sealed class EraTests
    {
        [Test]
        public void Ended_HasEnded()
        {
            using (var era = Era.Ended())
            {
                Assert.That(era.HasEnded);
            }
        }

        [Test]
        public void Constructor_Default_NotEnded()
        {
            using (var era = new Era())
            {
                Assert.That(!era.HasEnded);
            }
        }

        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var era = new Era();

            var awaiter = era.WaitAsync();
            era.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(awaiter);
        }

        [Test]
        public void TryEnd_Ended_ReturnsFalse()
        {
            using (var era = Era.Ended())
            {
                Assert.That(!era.TryEnd());
            }
        }

        [Test]
        public void TryEnd_NotEnded_ReturnsTrue()
        {
            using (var era = new Era())
            {
                Assert.That(era.TryEnd());
                Assert.That(era.HasEnded);
            }
        }

        [Test]
        public void TryEnd_Disposed_ReturnsFalse()
        {
            var era = new Era();
            era.Dispose();

            Assert.That(!era.TryEnd());
        }

        [Test]
        public void End_Ended_ThrowsInvalidOperationException()
        {
            using (var era = Era.Ended())
            {
                Assert.Throws<InvalidOperationException>(() => era.End());
            }
        }

        [Test]
        public void End_NotEnded_Ends()
        {
            using (var era = new Era())
            {
                era.End();
                Assert.That(era.HasEnded);
            }
        }

        [Test]
        public void End_Disposed_ThrowsObjectDisposedException()
        {
            var era = new Era();
            era.Dispose();

            Assert.Throws<ObjectDisposedException>(() => era.End());
        }

        [Test]
        public void WaitAsync_Ended_ReturnsCompletedTask()
        {
            using (var era = Era.Ended())
            {
                TaskAssert.Completed(era.WaitAsync());
            }
        }

        [Test]
        public void WaitAsync_NotEnded_WaitsForEnd()
        {
            using (var era = new Era())
            {
                var awaiter = era.WaitAsync();

                TaskAssert.DoesNotEnd(awaiter);
                era.End();
                TaskAssert.Completed(awaiter);
            }
        }

        [Test]
        public void WaitAsync_Disposed_ReturnsFaultedTask()
        {
            var era = new Era();
            era.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(era.WaitAsync());
        }

        [Test]
        public void WaitAsync_NotEnded_Cancelable()
        {
            using (var cts = new CancellationTokenSource())
            using (var era = new Era())
            {
                var awaiter = era.WaitAsync(cts.Token);

                TaskAssert.DoesNotEnd(awaiter);
                cts.Cancel();
                TaskAssert.Cancelled(awaiter);
            }
        }
    }
}
