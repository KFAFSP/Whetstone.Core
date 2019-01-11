using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using NUnit.Framework;
using NUnit.Framework.Constraints;

using Whetstone.Core.Contracts;

namespace Whetstone.Core
{
    internal class TaskAssert
    {
        const int C_Timeout = 10;

        [NotNull]
        public static Task Detach([NotNull] Func<Task> ADelegate)
        {
            Require.NotNull(ADelegate, nameof(ADelegate));

            var task = Task.Run<Task>(ADelegate);
            return task.Result;
        }

        [NotNull]
        public static Task<TResult> Detach<TResult>([NotNull] Func<Task<TResult>> ADelegate)
        {
            Require.NotNull(ADelegate, nameof(ADelegate));

            var task = Task.Run<Task<TResult>>(ADelegate);
            return task.Result;
        }

        public static Exception Faulted(
            IResolveConstraint AExpression,
            [NotNull] Task ATask,
            int ATimeout = C_Timeout
        )
        {
            Require.NotNull(ATask, nameof(ATask));

            Exception caught = null;
            try
            {
                ATask.Wait(ATimeout);
            }
            catch (AggregateException error)
            {
                caught = error.InnerException;
            }

            Assert.That(caught, AExpression);

            return caught;
        }

        public static Exception Faulted(
            Type AExpectedExceptionType,
            [NotNull] Task ATask,
            int ATimeout = C_Timeout
        ) => Faulted(
            new ExceptionTypeConstraint(AExpectedExceptionType),
            ATask,
            ATimeout
        );

        public static TException Faulted<TException>(
            [NotNull] Task ATask,
            int ATimeout = C_Timeout
        ) where TException : Exception
            => (TException)Faulted(
                typeof(TException),
                ATask,
                ATimeout
            );

        public static void Cancelled(
            [NotNull] Task ATask,
            int ATimeout = C_Timeout
        ) => Faulted<TaskCanceledException>(ATask, ATimeout);

        public static void Completed(
            [NotNull] Task ATask,
            int ATimeout = C_Timeout
        )
        {
            Require.NotNull(ATask, nameof(ATask));

            try
            {
                if (!ATask.Wait(ATimeout))
                {
                    Assert.Inconclusive(@"Task did not terminate.");
                }
            }
            catch (AggregateException error)
            {
                switch (error.InnerException)
                {
                    case TaskCanceledException _:
                        Assert.Fail(@"Task was cancelled.");
                        break;

                    default:
                        Assert.Fail(
                            $@"Task faulted: {error.InnerException.GetType().Name}."
                        );
                        break;
                }
            }
        }

        public static TResult Completed<TResult>(
            [NotNull] Task<TResult> ATask,
            int ATimeout = C_Timeout
        )
        {
            Completed((Task)ATask, ATimeout);
            return ATask.Result;
        }

        public static void DoesNotEnd(
            [NotNull] Task ATask,
            int ATimeout = C_Timeout
        )
        {
            Require.NotNull(ATask, nameof(ATask));

            try
            {
                if (ATask.Wait(ATimeout))
                {
                    Assert.Fail(@"Task was completed.");
                }
            }
            catch (AggregateException error)
            {
                switch (error.InnerException)
                {
                case TaskCanceledException _:
                    Assert.Fail(@"Task was cancelled.");
                    break;

                default:
                    Assert.Fail(
                        $@"Task faulted: {error.InnerException.GetType().Name}."
                    );
                    break;
                }
            }
        }
    }
}
