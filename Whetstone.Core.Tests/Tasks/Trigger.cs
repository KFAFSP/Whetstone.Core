using System;
using System.Threading;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(Trigger))]
    public sealed class TriggerTests
    {
        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var trigger = new Trigger();

            var awaiter = trigger.WaitAsync();
            trigger.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(awaiter);
        }

        [Test]
        public void Fire_Disposed_ThrowsObjectDisposedException()
        {
            var trigger = new Trigger();
            trigger.Dispose();

            Assert.Throws<ObjectDisposedException>(() => trigger.Fire());
        }

        [Test]
        public void Fire_Barrier()
        {
            using (var trigger = new Trigger())
            {
                var before = trigger.WaitAsync();
                trigger.Fire();
                var after = trigger.WaitAsync();

                TaskAssert.Completed(before);
                TaskAssert.DoesNotEnd(after);
            }
        }

        [Test]
        public void WaitAsync_WaitsForFire()
        {
            using (var trigger = new Trigger())
            {
                var awaiter = trigger.WaitAsync();

                TaskAssert.DoesNotEnd(awaiter);
                trigger.Fire();
                TaskAssert.Completed(awaiter);
            }
        }

        [Test]
        public void WaitAsync_Cancelable()
        {
            using (var cts = new CancellationTokenSource())
            using (var trigger = new Trigger())
            {
                var awaiter = trigger.WaitAsync(cts.Token);

                TaskAssert.DoesNotEnd(awaiter);
                cts.Cancel();
                TaskAssert.Cancelled(awaiter);
            }
        }

        [Test]
        public void WaitAsync_Disposed_ReturnsFaultedTask()
        {
            var trigger = new Trigger();
            trigger.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(trigger.WaitAsync());
        }
    }
}
