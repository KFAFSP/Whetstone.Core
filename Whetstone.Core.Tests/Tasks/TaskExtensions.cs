using System;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(TaskExtensions))]
    public sealed class TaskExtensionsTests
    {
        static readonly CancellationTokenSource _FCanceled = new CancellationTokenSource();
        static readonly Exception _FFaulted = new Exception();

        static TaskExtensionsTests()
        {
            _FCanceled.Cancel();
        }

        static Task MakeCompletedTask(TaskStatus AStatus)
        {
            switch (AStatus)
            {
                case TaskStatus.RanToCompletion:
                    return Task.CompletedTask;

                case TaskStatus.Canceled:
                    return Task.FromCanceled(_FCanceled.Token);

                case TaskStatus.Faulted:
                    return Task.FromException(_FFaulted);

                default:
                    throw new NotImplementedException();
            }
        }
        static async Task MakeUnendingTask()
        {
            while (true)
            {
                await Task.Yield();
            }

            // NOTE: This is intended behavior.
            // ReSharper disable once FunctionNeverReturns
        }

        [Test]
        public void OrCanceledBy_Cancelable_CanBeCanceled()
        {
            using (var cts = new CancellationTokenSource())
            {
                var task = MakeUnendingTask();

                var cont = task.OrCanceledBy(cts.Token);
                cts.Cancel();

                TaskAssert.Cancelled(cont);
            }
        }

        [Test]
        public void OrCanceledBy_CanceledToken_ReturnsCanceled()
        {
            var task = MakeUnendingTask();

            var cont = task.OrCanceledBy(_FCanceled.Token);
            Assert.That(cont.IsCanceled);
        }

        [TestCase(TaskStatus.RanToCompletion)]
        [TestCase(TaskStatus.Faulted)]
        [TestCase(TaskStatus.Canceled)]
        public void OrCanceledBy_NotCanceled_Propagates(TaskStatus AStatus)
        {
            using (var cts = new CancellationTokenSource())
            {
                var tcs = new TaskCompletionSource<Void>();
                var cont = tcs.Task.OrCanceledBy(cts.Token);
                Assume.That(cont.Wait(10), Is.False);

                switch (AStatus)
                {
                    case TaskStatus.RanToCompletion:
                        tcs.TrySetResult(default);
                        TaskAssert.Completed(cont);
                        break;

                    case TaskStatus.Canceled:
                        tcs.TrySetCanceled(_FCanceled.Token);
                        TaskAssert.Cancelled(cont);
                        break;

                    case TaskStatus.Faulted:
                        tcs.TrySetException(_FFaulted);
                        Assert.That(
                            TaskAssert.Faulted<Exception>(cont),
                            Is.SameAs(_FFaulted)
                        );
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        [Test]
        public void OrCanceledBy_NotCanceled_PropagatesResult()
        {
            using (var cts = new CancellationTokenSource())
            {
                var tcs = new TaskCompletionSource<int>();
                var cont = tcs.Task.OrCanceledBy(cts.Token);
                TaskAssert.DoesNotEnd(cont);

                tcs.TrySetResult(1);
                Assert.That(TaskAssert.Completed(cont), Is.EqualTo(1));
            }
        }

        [TestCase(TaskStatus.RanToCompletion)]
        [TestCase(TaskStatus.Faulted)]
        [TestCase(TaskStatus.Canceled)]
        public void OrCanceledBy_Completed_ReturnsTask(TaskStatus AStatus)
        {
            using (var cts = new CancellationTokenSource())
            {
                var task = MakeCompletedTask(AStatus);
                Assume.That(task.Status == AStatus);

                var cont = task.OrCanceledBy(cts.Token);
                Assert.That(cont, Is.SameAs(task));
            }
        }

        [Test]
        public void OrCanceledBy_Uncancelable_ReturnsTask()
        {
            var task = MakeUnendingTask();

            var cont = task.OrCanceledBy(CancellationToken.None);
            Assert.That(cont, Is.SameAs(task));
        }
    }
}
