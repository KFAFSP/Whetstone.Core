using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    [TestFixture]
    [TestOf(typeof(WeakOrdering))]
    [Category("Contracts")]
    public sealed partial class WeakOrderingTests
    {
        [NotNull]
        [ItemNotNull]
        [MustUseReturnValue]
        public static IEnumerable MapTestCases(
            [NotNull] [ItemNotNull] IEnumerable AEnumerable,
            bool AMapResults = true
        )
        {
            foreach (TestCaseData data in AEnumerable)
            {
                var result = data.MapArguments(X => X is int arg ? Orderable.Map(arg) : X);

                if (AMapResults && result.HasExpectedResult && result.ExpectedResult is int ret)
                {
                    result.Returns(Orderable.Map(ret));
                }

                yield return result;
            }
        }

        static readonly Orderable _FZero = new Orderable(0);
        static readonly Orderable _FZero2 = new Orderable(0);
        static readonly Orderable _FOne = new Orderable(1);
        static readonly Orderable _FOne2 = new Orderable(1);
    }
}