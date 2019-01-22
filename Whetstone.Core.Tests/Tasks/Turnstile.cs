using System;
using System.Linq;
using System.Threading;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Threading")]
    [TestOf(typeof(Turnstile))]
    public sealed class TurnstileTests
    {
        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var turnstile = new Turnstile();

            using (var cts = new CancellationTokenSource())
            {
                var awaiters = new[]
                {
                    turnstile.WaitAsync(),
                    turnstile.WaitAsync(cts.Token),
                    turnstile.WaitAsync(),
                    turnstile.WaitAsync()
                };

                cts.Cancel();
                turnstile.Dispose();

                TaskAssert.Faulted<ObjectDisposedException>(awaiters[0]);
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
            var turnstile = new Turnstile();
            turnstile.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(turnstile.WaitAsync());
        }

        [Test]
        public void WaitAsync_NotEmpty_WaitsForTurn()
        {
            using (var turnstile = new Turnstile())
            {
                var t1 = turnstile.WaitAsync();
                var t2 = turnstile.WaitAsync();
                var t3 = turnstile.WaitAsync();

                Assert.That(turnstile.HasWaiting);

                TaskAssert.DoesNotEnd(t1);
                TaskAssert.DoesNotEnd(t2);
                TaskAssert.DoesNotEnd(t3);

                Assert.That(turnstile.Turn());

                TaskAssert.Completed(t1);
                TaskAssert.DoesNotEnd(t2);
                TaskAssert.DoesNotEnd(t3);

                Assert.That(turnstile.Turn());

                TaskAssert.Completed(t2);
                TaskAssert.DoesNotEnd(t3);

                Assert.That(turnstile.Turn());

                TaskAssert.Completed(t3);

                Assert.That(!turnstile.HasWaiting);
                Assert.That(!turnstile.Turn());
            }
        }

        [Test]
        public void WaitAsync_NotEmpty_CanCedeTurn()
        {
            using (var cts = new CancellationTokenSource())
            using (var turnstile = new Turnstile())
            {
                var t1 = turnstile.WaitAsync();
                var t2 = turnstile.WaitAsync(cts.Token);
                var t3 = turnstile.WaitAsync();

                Assert.That(turnstile.HasWaiting);

                TaskAssert.DoesNotEnd(t1);
                TaskAssert.DoesNotEnd(t2);
                TaskAssert.DoesNotEnd(t3);

                cts.Cancel();
                Assert.That(turnstile.Turn());

                TaskAssert.Completed(t1);
                TaskAssert.Cancelled(t2);
                TaskAssert.DoesNotEnd(t3);

                Assert.That(turnstile.Turn());

                TaskAssert.Completed(t3);

                Assert.That(!turnstile.HasWaiting);
                Assert.That(!turnstile.Turn());
            }
        }

        [Test]
        public void HasWaiting_DetectsCeded()
        {
            using (var cts = new CancellationTokenSource())
            using (var turnstile = new Turnstile())
            {
                var awaiter = turnstile.WaitAsync(cts.Token);

                TaskAssert.DoesNotEnd(awaiter);
                Assert.That(turnstile.HasWaiting);

                cts.Cancel();

                TaskAssert.Cancelled(awaiter);
                Assert.That(!turnstile.HasWaiting);
            }
        }
    }
}
