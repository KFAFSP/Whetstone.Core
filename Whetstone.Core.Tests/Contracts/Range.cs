using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    [TestFixture]
    [TestOf(typeof(Range<>))]
    [Category("Contracts")]
    [Category("Range")]
    public sealed partial class RangeTTests
    {
        [NotNull]
        [ItemNotNull]
        [MustUseReturnValue]
        public static IEnumerable MapTestCases(
            [NotNull] [ItemNotNull] IEnumerable AEnumerable,
            bool AMapResults = true
        )
        {
            foreach (TestCaseData data in WeakOrderingTests.MapTestCases(AEnumerable, AMapResults))
            {
                var result = data.MapArguments(X => X is Range<int> arg
                    ? Range<Orderable>.Of(
                        Orderable.Map(arg.Lower), arg.IncludesLower,
                        Orderable.Map(arg.Upper), arg.IncludesUpper
                    )
                    : X);

                if (result.HasExpectedResult && result.ExpectedResult is Range<int> res)
                {
                    result.Returns(Range<Orderable>.Of(
                        Orderable.Map(res.Lower), res.IncludesLower,
                        Orderable.Map(res.Upper), res.IncludesUpper
                    ));
                }

                yield return result;
            }
        }

        static readonly Orderable _FZero = new Orderable(0);
        static readonly Orderable _FTwo = new Orderable(2);

        [Test]
        public void Default_IsEmpty()
        {
            Assert.That(default(Range<int>).IsEmpty, Is.True);
        }

        [TestCaseSource(nameof(OfValueTypeEmptinessTestCases))]
        public bool Of_ValueType_Emptiness(int AL, bool AIL, int AU, bool AIU)
            => Range<int>.Of(AL, AIL, AU, AIU).IsEmpty;

        [UsedImplicitly]
        static IEnumerable OfValueTypeEmptinessTestCases
        {
            get
            {
                yield return new TestCaseData(0, false, 0, false).Returns(true);
                yield return new TestCaseData(0, false, 0, true).Returns(true);
                yield return new TestCaseData(0, true, 0, false).Returns(true);
                yield return new TestCaseData(0, true, 0, true).Returns(false);

                yield return new TestCaseData(1, false, 0, false).Returns(true);
                yield return new TestCaseData(1, false, 0, true).Returns(true);
                yield return new TestCaseData(1, true, 0, false).Returns(true);
                yield return new TestCaseData(1, true, 0, true).Returns(true);

                yield return new TestCaseData(0, false, 1, false).Returns(false);
                yield return new TestCaseData(0, false, 1, true).Returns(false);
                yield return new TestCaseData(0, true, 1, false).Returns(false);
                yield return new TestCaseData(0, true, 1, true).Returns(false);
            }
        }

        [TestCaseSource(nameof(OfReferenceTypeEmptinessTestCases))]
        public bool Of_ReferenceType_Emptiness(Orderable AL, bool AIL, Orderable AU, bool AIU)
            => Range<Orderable>.Of(AL, AIL, AU, AIU).IsEmpty;

        [UsedImplicitly]
        static IEnumerable OfReferenceTypeEmptinessTestCases
        {
            get
            {
                foreach (var data in MapTestCases(OfValueTypeEmptinessTestCases))
                    yield return data;

                yield return new TestCaseData(null, false, null, false)
                    .Returns(true);
                yield return new TestCaseData(null, false, null, true)
                    .Returns(true);
                yield return new TestCaseData(null, true, null, false)
                    .Returns(true);
                yield return new TestCaseData(null, true, null, true)
                    .Returns(false);

                yield return new TestCaseData(_FZero, false, null, false)
                    .Returns(true);
                yield return new TestCaseData(null, false, _FZero, false)
                    .Returns(false);
            }
        }

        [Test]
        public void Empty_IsEmpty()
        {
            Assert.That(Range<int>.Empty.IsEmpty, Is.True);
        }

        [TestCaseSource(nameof(CompareWithValueTypeTestCases))]
        public int CompareWith_ValueType(Range<int> ARange, int AValue)
            => ARange.CompareWith(AValue);

        [UsedImplicitly]
        static IEnumerable CompareWithValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(Range<int>.Of(0, false, 2, false), 0).Returns(-1);
                yield return new TestCaseData(Range<int>.Of(0, false, 2, false), 1).Returns(0);
                yield return new TestCaseData(Range<int>.Of(0, false, 2, false), 2).Returns(1);

                yield return new TestCaseData(Range<int>.Of(0, false, 1, true), 0).Returns(-1);
                yield return new TestCaseData(Range<int>.Of(0, false, 1, true), 1).Returns(0);
                yield return new TestCaseData(Range<int>.Of(0, false, 1, true), 2).Returns(1);

                yield return new TestCaseData(Range<int>.Of(1, true, 2, false), 0).Returns(-1);
                yield return new TestCaseData(Range<int>.Of(1, true, 2, false), 1).Returns(0);
                yield return new TestCaseData(Range<int>.Of(1, true, 2, false), 2).Returns(1);

                yield return new TestCaseData(Range<int>.Of(1, true, 1, true), 0).Returns(-1);
                yield return new TestCaseData(Range<int>.Of(1, true, 1, true), 1).Returns(0);
                yield return new TestCaseData(Range<int>.Of(1, true, 1, true), 2).Returns(1);
            }
        }

        [TestCaseSource(nameof(CompareWithReferenceTypeTestCases))]
        public int CompareWith_ReferenceType(Range<Orderable> ARange, Orderable AValue)
            => ARange.CompareWith(AValue);

        [UsedImplicitly]
        static IEnumerable CompareWithReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(CompareWithValueTypeTestCases, false))
                    yield return data;

                yield return new TestCaseData(Range<Orderable>.Of(_FZero, false, _FTwo, false), null).Returns(-1);
                yield return new TestCaseData(Range<Orderable>.Of(null, false, _FTwo, false), null).Returns(-1);
                yield return new TestCaseData(Range<Orderable>.Of(null, true, _FTwo, false), null).Returns(0);
            }
        }

        [TestCaseSource(nameof(EqualsRangeTestCases))]
        public bool Equals_Range(Range<int> ALhs, Range<int> ARhs)
            => ALhs.Equals(ARhs);

        [UsedImplicitly]
        static IEnumerable EqualsRangeTestCases
        {
            get
            {
                yield return new TestCaseData(
                    Range<int>.Empty,
                    default(Range<int>)
                ).Returns(true);
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 1, true),
                    Range<int>.Of(0, false, 1, true)
                ).Returns(true);
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 0, true),
                    Range<int>.Empty
                ).Returns(false);
            }
        }

        [TestCaseSource(nameof(EqualsAnyTestCases))]
        public bool Equals_Any(Range<int> ALhs, object ARhs)
            => ALhs.Equals(ARhs);

        [UsedImplicitly]
        static IEnumerable EqualsAnyTestCases
        {
            get
            {
                foreach (var data in EqualsRangeTestCases)
                    yield return data;

                yield return new TestCaseData(
                    Range<int>.Empty,
                    null
                ).Returns(false);
                yield return new TestCaseData(
                    Range<int>.Empty,
                    Range<string>.Empty
                ).Returns(false);
            }
        }

        [TestCaseSource(nameof(EqualsRangeTestCases))]
        public bool GetHashCode(Range<int> ALhs, Range<int> ARhs)
            => ALhs.GetHashCode() == ARhs.GetHashCode();

        [TestCaseSource(nameof(ToStringValueTypeTestCases))]
        public string ToString_ValueType(Range<int> ARange)
            => ARange.ToString();

        [UsedImplicitly]
        static IEnumerable ToStringValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(Range<int>.Of(0, false, 1, false)).Returns("(0, 1)");
                yield return new TestCaseData(Range<int>.Of(0, false, 1, true)).Returns("(0, 1]");
                yield return new TestCaseData(Range<int>.Of(0, true, 1, false)).Returns("[0, 1)");
                yield return new TestCaseData(Range<int>.Of(0, true, 1, true)).Returns("[0, 1]");
            }
        }

        [TestCaseSource(nameof(EqualsRangeTestCases))]
        public bool OpEquals_Range(Range<int> ALhs, Range<int> ARhs)
            => ALhs == ARhs && !(ALhs != ARhs);
    }

    [TestFixture]
    [TestOf(typeof(Range))]
    [Category("Contracts")]
    [Category("Range")]
    public sealed partial class RangeTests { }
}
