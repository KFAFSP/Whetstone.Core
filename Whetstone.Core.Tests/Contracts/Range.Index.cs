using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public partial class RangeTests
    {
        static readonly List<int> _FEmptyList = new List<int>();
        static readonly List<int> _FShortList = new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

        [TestCaseSource(nameof(IndicesInt32TestCases))]
        public Range<int> Indices_Int32_ReturnsIndexRange(int AValue)
            => AValue.Indices();

        [UsedImplicitly]
        static IEnumerable IndicesInt32TestCases
        {
            get
            {
                yield return new TestCaseData(0)
                    .Returns(Range.Of(0, true, 0, false));
                yield return new TestCaseData(10)
                    .Returns(Range.Of(0, true, 10, false));
            }
        }

        [TestCaseSource(nameof(IndicesStringTestCases))]
        public Range<int> Indices_String_ReturnsIndexRange(string AValue)
            => AValue.Indices();

        [UsedImplicitly]
        static IEnumerable IndicesStringTestCases
        {
            get
            {
                yield return new TestCaseData(null)
                    .Returns(Range.Of(0, true, 0, false));
                yield return new TestCaseData("")
                    .Returns(Range.Of(0, true, 0, false));
                yield return new TestCaseData("abc")
                    .Returns(Range.Of(0, true, 3, false));
            }
        }

        [TestCaseSource(nameof(IndicesArrayTestCases))]
        public Range<int> Indices_Array_ReturnsIndexRange(int[] AValue)
            => AValue.Indices();

        [UsedImplicitly]
        static IEnumerable IndicesArrayTestCases
        {
            get
            {
                yield return new TestCaseData(null)
                    .Returns(Range.Of(0, true, 0, false));
                yield return new TestCaseData(new int[0])
                    .Returns(Range.Of(0, true, 0, false));
                yield return new TestCaseData(new int[10])
                    .Returns(Range.Of(0, true, 10, false));
            }
        }

        [TestCaseSource(nameof(IndicesCollectionTestCases))]
        public Range<int> Indices_Collection_ReturnsIndexRange(ICollection AValue)
            => AValue.Indices();

        [UsedImplicitly]
        static IEnumerable IndicesCollectionTestCases
        {
            get
            {
                yield return new TestCaseData(null)
                    .Returns(Range.Of(0, true, 0, false));
                yield return new TestCaseData(_FEmptyList)
                    .Returns(Range.Of(0, true, 0, false));
                yield return new TestCaseData(_FShortList)
                    .Returns(Range.Of(0, true, _FShortList.Count, false));
            }
        }

        [TestCaseSource(nameof(IndicesCollectionTestCases))]
        public Range<int> Indices_CollectionT_ReturnsIndexRange(ICollection<int> AValue)
            => AValue.Indices();

        [TestCaseSource(nameof(LongIndicesLongTestCases))]
        public Range<long> LongIndices_Long_ReturnsIndexRange(long AValue)
            => AValue.LongIndices();

        [UsedImplicitly]
        static IEnumerable LongIndicesLongTestCases
        {
            get
            {
                yield return new TestCaseData(0L)
                    .Returns(Range.Of(0L, true, 0L, false));
                yield return new TestCaseData(10L)
                    .Returns(Range.Of(0L, true, 10L, false));
            }
        }

        [TestCaseSource(nameof(LongIndicesArrayTestCases))]
        public Range<long> LongIndices_Array_ReturnsIndexRange(int[] AValue)
            => AValue.LongIndices();

        [UsedImplicitly]
        static IEnumerable LongIndicesArrayTestCases
        {
            get
            {
                yield return new TestCaseData(null)
                    .Returns(Range.Of(0L, true, 0L, false));
                yield return new TestCaseData(new int[0])
                    .Returns(Range.Of(0L, true, 0L, false));
                yield return new TestCaseData(new int[10])
                    .Returns(Range.Of(0L, true, 10L, false));
            }
        }
    }
}
