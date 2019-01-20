using System;
using System.Threading;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(Future<>))]
    public sealed class FutureTests
    {
        [Test]
        public void Of_Value_ExistsWithValue()
        {
            using (var future = Future.Of(1))
            {
                Assert.That(future.Exists);
                Assert.That(future.Get(), Is.EqualTo(1));
            }
        }

        [Test]
        public void Constructor_Default_DoesNotExist()
        {
            using (var future = new Future<int>())
            {
                Assert.That(!future.Exists);
            }
        }

        [Test]
        public void Dispose_FaultsAwaiters()
        {
            var future = new Future<int>();

            var awaiter = future.WaitAsync();
            future.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(awaiter);
        }

        [Test]
        public void TryPost_Exists_ReturnsFalse()
        {
            using (var future = Future.Of(1))
            {
                Assert.That(!future.TryPost(0));
            }
        }

        [Test]
        public void TryPost_DoesNotExist_ReturnsTrue()
        {
            using (var future = new Future<int>())
            {
                Assert.That(future.TryPost(1));
                Assert.That(future.Exists);
                Assert.That(future.Get(), Is.EqualTo(1));
            }
        }

        [Test]
        public void TryPost_Disposed_ReturnsFalse()
        {
            var future = new Future<int>();
            future.Dispose();

            Assert.That(!future.TryPost(1));
        }

        [Test]
        public void Post_Exists_ThrowsInvalidOperationException()
        {
            using (var future = Future.Of(1))
            {
                Assert.Throws<InvalidOperationException>(() => future.Post(1));
            }
        }

        [Test]
        public void Post_DoesNotExist_Posts()
        {
            using (var future = new Future<int>())
            {
                future.Post(1);
                Assert.That(future.Exists);
                Assert.That(future.Get(), Is.EqualTo(1));
            }
        }

        [Test]
        public void Post_Disposed_ThrowsObjectDisposedException()
        {
            var future = new Future<int>();
            future.Dispose();

            Assert.Throws<ObjectDisposedException>(() => future.Post(1));
        }

        [Test]
        public void TryGet_Exists_PresentValue()
        {
            using (var future = Future.Of(1))
            {
                Assert.That(future.TryGet().Value, Is.EqualTo(1));
            }
        }

        [Test]
        public void TryGet_DoesNotExist_Absent()
        {
            using (var future = new Future<int>())
            {
                Assert.That(!future.TryGet().IsPresent);
            }
        }

        [Test]
        public void TryGet_Disposed_Absent()
        {
            var future = new Future<int>();
            future.Dispose();

            Assert.That(!future.TryGet().IsPresent);
        }

        [Test]
        public void Get_Exists_ReturnsValue()
        {
            using (var future = Future.Of(1))
            {
                Assert.That(future.Get(), Is.EqualTo(1));
            }
        }

        [Test]
        public void Get_DoesNotExist_ThrowsInvalidOperationException()
        {
            using (var future = new Future<int>())
            {
                Assert.Throws<InvalidOperationException>(() => _ = future.Get());
            }
        }

        [Test]
        public void Get_Disposed_ThrowsObjectDisposedException()
        {
            var future = new Future<int>();
            future.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _ = future.Get());
        }

        [Test]
        public void WaitAsync_Exists_ReturnsCompletedTask()
        {
            using (var future = Future.Of(1))
            {
                Assert.That(
                    TaskAssert.Completed(future.WaitAsync()),
                    Is.EqualTo(1)
                );
            }
        }

        [Test]
        public void WaitAsync_DoesNotExist_WaitsForEnd()
        {
            using (var future = new Future<int>())
            {
                var awaiter = future.WaitAsync();

                TaskAssert.DoesNotEnd(awaiter);
                future.Post(1);
                Assert.That(
                    TaskAssert.Completed(awaiter),
                    Is.EqualTo(1)
                );
            }
        }

        [Test]
        public void WaitAsync_Disposed_ReturnsFaultedTask()
        {
            var future = new Future<int>();
            future.Dispose();

            TaskAssert.Faulted<ObjectDisposedException>(future.WaitAsync());
        }

        [Test]
        public void WaitAsync_DoesNotExist_Cancelable()
        {
            using (var cts = new CancellationTokenSource())
            using (var future = new Future<int>())
            {
                var awaiter = future.WaitAsync(cts.Token);

                TaskAssert.DoesNotEnd(awaiter);
                cts.Cancel();
                TaskAssert.Cancelled(awaiter);
            }
        }
    }
}
