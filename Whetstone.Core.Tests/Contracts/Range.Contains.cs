using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public partial class RangeTTests
    {
        [TestCaseSource(nameof(ContainsValueTypeTestCases))]
        public bool Contains_ValueType(Range<int> ARange, int AValue)
            => ARange.Contains(AValue);

        [UsedImplicitly]
        static IEnumerable ContainsValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(Range<int>.Of(0, false, 0, false), 0).Returns(false);

                yield return new TestCaseData(Range<int>.Of(0, false, 2, false), 0).Returns(false);
                yield return new TestCaseData(Range<int>.Of(0, true, 2, false), 0).Returns(true);

                yield return new TestCaseData(Range<int>.Of(0, false, 2, false), 2).Returns(false);
                yield return new TestCaseData(Range<int>.Of(0, false, 2, true), 2).Returns(true);

                yield return new TestCaseData(Range<int>.Of(0, false, 2, false), 1).Returns(true);
                yield return new TestCaseData(Range<int>.Of(0, false, 2, true), 1).Returns(true);
                yield return new TestCaseData(Range<int>.Of(0, true, 2, false), 1).Returns(true);
                yield return new TestCaseData(Range<int>.Of(0, true, 2, true), 1).Returns(true);
            }
        }

        [TestCaseSource(nameof(ContainsReferenceTypeTestCases))]
        public bool Contains_ReferenceType(Range<Orderable> ARange, Orderable AValue)
            => ARange.Contains(AValue);

        [UsedImplicitly]
        static IEnumerable ContainsReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(ContainsValueTypeTestCases))
                    yield return data;

                yield return new TestCaseData(
                    Range<Orderable>.Of(_FZero, false, _FTwo, false), null
                ).Returns(false);
                yield return new TestCaseData(
                    Range<Orderable>.Of(null, false, _FTwo, false), null
                ).Returns(false);
                yield return new TestCaseData(
                    Range<Orderable>.Of(null, true, _FTwo, false), null
                ).Returns(true);
            }
        }

        [TestCaseSource(nameof(Contains2ValueTypeTestCases))]
        public bool Contains2_ValueType(Range<int> ALhs, Range<int> ARhs)
            => ALhs.Contains(ARhs);

        [UsedImplicitly]
        static IEnumerable Contains2ValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, false),
                    Range<int>.Of(0, false, 2, false)
                ).Returns(true);
                yield return new TestCaseData(
                    Range<int>.Of(0, true, 2, true),
                    Range<int>.Of(1, true, 1, true)
                ).Returns(true);

                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, false),
                    Range<int>.Of(-1, false, 2, false)
                ).Returns(false);
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, false),
                    Range<int>.Of(0, true, 2, false)
                ).Returns(false);
                yield return new TestCaseData(
                    Range<int>.Of(0, true, 2, false),
                    Range<int>.Of(-2, false, 2, false)
                ).Returns(false);
                yield return new TestCaseData(
                    Range<int>.Of(0, true, 2, false),
                    Range<int>.Of(-1, true, 2, false)
                ).Returns(false);

                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, false),
                    Range<int>.Of(0, false, 3, false)
                ).Returns(false);
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, false),
                    Range<int>.Of(0, false, 2, true)
                ).Returns(false);
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, true),
                    Range<int>.Of(0, false, 4, false)
                ).Returns(false);
                yield return new TestCaseData(
                    Range<int>.Of(0, false, 2, true),
                    Range<int>.Of(0, false, 3, true)
                ).Returns(false);
            }
        }

        [TestCaseSource(nameof(Contains2ReferenceTypeTestCases))]
        public bool Contains2_ReferenceType(Range<Orderable> ALhs, Range<Orderable> ARhs)
            => ALhs.Contains(ARhs);

        [UsedImplicitly]
        static IEnumerable Contains2ReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(Contains2ValueTypeTestCases))
                    yield return data;

                yield return new TestCaseData(
                    Range<Orderable>.Of(null, false, _FTwo, false),
                    Range<Orderable>.Of(null, false, _FZero, false)
                ).Returns(true);

                yield return new TestCaseData(
                    Range<Orderable>.Of(null, false, _FTwo, false),
                    Range<Orderable>.Of(_FZero, true, _FZero, true)
                ).Returns(true);
                yield return new TestCaseData(
                    Range<Orderable>.Of(_FZero, true, null, true),
                    Range<Orderable>.Of(_FZero, true, _FZero, true)
                ).Returns(false);
                yield return new TestCaseData(
                    Range<Orderable>.Of(null, true, null, true),
                    Range<Orderable>.Of(_FZero, true, _FZero, true)
                ).Returns(false);
            }
        }
    }
}
