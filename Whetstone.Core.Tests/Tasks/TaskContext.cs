using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Whetstone.Core.Tasks
{
    [TestFixture]
    [Category("Tasks")]
    [TestOf(typeof(TaskContext))]
    public sealed class TaskContextTests
    {
        [Test]
        public void ThreadId()
        {
            var t1Id = TaskContext.C_NoThreadId;
            var t2Id = TaskContext.C_NoThreadId;

            var t1 = new Thread(() => { t1Id = TaskContext.CurrentThreadId; });
            var t2 = new Thread(() => { t2Id = TaskContext.CurrentThreadId; });
            t1.Start();
            t2.Start();

            Assume.That(t1.Join(10));
            Assume.That(t2.Join(10));

            Assert.That(t1Id, Is.Not.EqualTo(TaskContext.C_NoThreadId));
            Assert.That(t2Id, Is.Not.EqualTo(TaskContext.C_NoThreadId));
            Assert.That(t1Id, Is.Not.EqualTo(t2Id));
        }

        [Test]
        public void TaskId()
        {
            var t1Id = TaskContext.C_NoTaskId;
            var t2Id = TaskContext.C_NoTaskId;

            var t1 = Task.Run(() => { t1Id = TaskContext.CurrentTaskId; });
            var t2 = Task.Run(() => { t2Id = TaskContext.CurrentTaskId; });

            Assume.That(t1.Wait(10));
            Assume.That(t2.Wait(10));

            Assert.That(t1Id, Is.Not.EqualTo(TaskContext.C_NoTaskId));
            Assert.That(t2Id, Is.Not.EqualTo(TaskContext.C_NoTaskId));
            Assert.That(t1Id, Is.Not.EqualTo(t2Id));
        }

        [Test]
        public void Id()
        {
            var host1Id = TaskContext.C_NoId;
            var call1Id = TaskContext.C_NoId;
            var host2Id = TaskContext.C_NoId;
            var call2Id = TaskContext.C_NoId;

            void Host1()
            {
                host1Id = TaskContext.CurrentId;
                Call1();
            }

            void Call1()
            {
                call1Id = TaskContext.CurrentId;
            }

            async void Host2()
            {
                host2Id = TaskContext.CurrentId;
                await Call2();
            }

            async Task Call2()
            {
                call2Id = TaskContext.CurrentId;
                await Task.Yield();
            }

            var t1 = Task.Run(Host1);
            var t2 = Task.Run(Host2);

            Assume.That(t1.Wait(10));
            Assume.That(t2.Wait(10));

            Assert.That(host1Id, Is.Not.EqualTo(TaskContext.C_NoId));
            Assert.That(call1Id, Is.Not.EqualTo(TaskContext.C_NoId));
            Assert.That(host2Id, Is.Not.EqualTo(TaskContext.C_NoId));
            Assert.That(call2Id, Is.Not.EqualTo(TaskContext.C_NoId));

            Assert.That(host1Id, Is.Not.EqualTo(host2Id));
            Assert.That(host1Id, Is.EqualTo(call1Id));
            Assert.That(host2Id, Is.EqualTo(call2Id));
        }
    }
}
