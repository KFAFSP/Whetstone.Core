using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public partial class RangeTTests
    {
        [TestCaseSource(nameof(IntersectValueTypeTestCases))]
        public Range<int> Intersect_ValueType(Range<int> ALhs, Range<int> ARhs)
            => Range<int>.Intersect(ALhs, ARhs);

        [UsedImplicitly]
        static IEnumerable IntersectValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, false),
                    Range<int>.Of(0, false, 1, false)
                ).Returns(Range<int>.Of(0, false, 1, false));
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, false),
                    Range<int>.Of(0, false, 1, true)
                ).Returns(Range<int>.Of(0, false, 1, true));
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, false),
                    Range<int>.Of(0, false, 2, true)
                ).Returns(Range<int>.Of(0, false, 2, false));

                yield return new TestCaseData(
                    Range<int>.Of(2, false, 3, false),
                    Range<int>.Of(1, false, 3, false)
                ).Returns(Range<int>.Of(2, false, 3, false));
                yield return new TestCaseData(
                    Range<int>.Of(2, false, 3, false),
                    Range<int>.Of(1, true, 3, false)
                ).Returns(Range<int>.Of(2, false, 3, false));
                yield return new TestCaseData(
                    Range<int>.Of(1, false, 3, false),
                    Range<int>.Of(1, true, 3, false)
                ).Returns(Range<int>.Of(1, false, 3, false));

                yield return new TestCaseData(
                    Range<int>.Of(0, true, 3, true),
                    Range<int>.Of(1, true, 2, true)
                ).Returns(Range<int>.Of(1, true, 2, true));

                yield return new TestCaseData(
                    Range<int>.Of(1, false, 0, false),
                    Range<int>.Of(2, false, 3, false)
                ).Returns(Range<int>.Of(1, false, 0, false));
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 1, false),
                    Range<int>.Of(3, false, 2, false)
                ).Returns(Range<int>.Of(3, false, 2, false));

                yield return new TestCaseData(
                    Range<int>.Of(0, false, 1, false),
                    Range<int>.Of(2, false, 3, false)
                ).Returns(Range<int>.Of(2, false, 1, false));
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 1, false),
                    Range<int>.Of(2, true, 3, false)
                ).Returns(Range<int>.Of(2, true, 1, false));
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 1, true),
                    Range<int>.Of(2, false, 3, false)
                ).Returns(Range<int>.Of(2, false, 1, true));

                yield return new TestCaseData(
                    Range<int>.Of(2, false, 3, false),
                    Range<int>.Of(0, false, 1, false)
                ).Returns(Range<int>.Of(2, false, 1, false));
                yield return new TestCaseData(
                    Range<int>.Of(2, true, 3, false),
                    Range<int>.Of(0, false, 1, false)
                ).Returns(Range<int>.Of(2, true, 1, false));
                yield return new TestCaseData(
                    Range<int>.Of(2, false, 3, false),
                    Range<int>.Of(0, false, 1, true)
                ).Returns(Range<int>.Of(2, false, 1, true));
            }
        }

        [TestCaseSource(nameof(IntersectReferenceTypeTestCases))]
        public Range<Orderable> Intersect_ReferenceType(
            Range<Orderable> ALhs,
            Range<Orderable> ARhs
        ) => Range<Orderable>.Intersect(ALhs, ARhs);

        [UsedImplicitly]
        static IEnumerable IntersectReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(IntersectValueTypeTestCases))
                    yield return data;

                yield return new TestCaseData(
                    Range<Orderable>.Of(null, false, _FTwo, false),
                    Range<Orderable>.Of(null, false, _FZero, true)
                ).Returns(Range<Orderable>.Of(null, false, _FZero, true));
            }
        }

        [TestCaseSource(nameof(IntersectsValueTypeTestCases))]
        public bool Intersects_ValueType(Range<int> ALhs, Range<int> ARhs)
            => ALhs.Intersects(ARhs);

        [UsedImplicitly]
        static IEnumerable IntersectsValueTypeTestCases
        {
            get
            {
                foreach (TestCaseData data in IntersectValueTypeTestCases)
                    yield return data.Returns(!((Range<int>)data.ExpectedResult).IsEmpty);
            }
        }

        [TestCaseSource(nameof(IntersectsReferenceTypeTestCases))]
        public bool Intersects_ReferenceType(Range<Orderable> ALhs, Range<Orderable> ARhs)
            => ALhs.Intersects(ARhs);

        [UsedImplicitly]
        static IEnumerable IntersectsReferenceTypeTestCases
        {
            get
            {
                foreach (TestCaseData data in IntersectReferenceTypeTestCases)
                    yield return data.Returns(!((Range<Orderable>)data.ExpectedResult).IsEmpty);
            }
        }
    }
}
