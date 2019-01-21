using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented

namespace Whetstone.Core.Contracts
{
    public partial class RangeTests
    {
        static readonly List<int> _FEmptyList = new List<int>();
        static readonly List<int> _FShortList = new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

        [Test]
        public void Span_Int32_OffsetNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = Range.Span(-1, 0)
            );
        }

        [Test]
        public void Span_Int32_LengthNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = Range.Span(0, -1)
            );
        }

        [TestCaseSource(nameof(SpanInt32TestCases))]
        public Range<int> Span_Int32_ReturnsSpan(int AOffset, int ALength)
            => Range.Span(AOffset, ALength);

        [UsedImplicitly]
        static IEnumerable SpanInt32TestCases
        {
            get
            {
                yield return new TestCaseData(0, 0)
                    .Returns(Range.Of(0, true, 0, false));
                yield return new TestCaseData(0, 1)
                    .Returns(Range.Of(0, true, 1, false));
                yield return new TestCaseData(10, 5)
                    .Returns(Range.Of(10, true, 15, false));
            }
        }

        [TestCaseSource(nameof(OffsetLimitLengthInt32TestCases))]
        public (int, int, int) OffsetLimitLength_Int32(Range<int> ARange)
            => (ARange.Offset(), ARange.Limit(), ARange.Length());

        [UsedImplicitly]
        static IEnumerable OffsetLimitLengthInt32TestCases
        {
            get
            {
                yield return new TestCaseData(Range.Of(0, true, 0, false))
                    .Returns((0, 0, 0));
                yield return new TestCaseData(Range.Of(0, true, 1, false))
                    .Returns((0, 1, 1));
                yield return new TestCaseData(Range.Of(10, true, 15, false))
                    .Returns((10, 15, 5));
                yield return new TestCaseData(Range.Of(10, false, 15, true))
                    .Returns((11, 16, 5));
            }
        }

        [Test]
        public void Span_Int64_OffsetNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = Range.Span(-1L, 0L)
            );
        }

        [Test]
        public void Span_Int64_LengthNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = Range.Span(0L, -1L)
            );
        }

        [TestCaseSource(nameof(SpanInt64TestCases))]
        public Range<long> Span_Int64_ReturnsSpan(long AOffset, long ALength)
            => Range.Span(AOffset, ALength);

        [UsedImplicitly]
        static IEnumerable SpanInt64TestCases
        {
            get
            {
                yield return new TestCaseData(0L, 0L)
                    .Returns(Range.Of(0L, true, 0L, false));
                yield return new TestCaseData(0L, 1L)
                    .Returns(Range.Of(0L, true, 1L, false));
                yield return new TestCaseData(10L, 5L)
                    .Returns(Range.Of(10L, true, 15L, false));
            }
        }

        [TestCaseSource(nameof(OffsetLimitLengthInt64TestCases))]
        public (long, long, long) OffsetLimitLength_Int64(Range<long> ARange)
            => (ARange.Offset(), ARange.Limit(), ARange.Length());

        [UsedImplicitly]
        static IEnumerable OffsetLimitLengthInt64TestCases
        {
            get
            {
                yield return new TestCaseData(Range.Of(0L, true, 0L, false))
                    .Returns((0L, 0L, 0L));
                yield return new TestCaseData(Range.Of(0L, true, 1L, false))
                    .Returns((0L, 1L, 1L));
                yield return new TestCaseData(Range.Of(10L, true, 15L, false))
                    .Returns((10L, 15L, 5L));
                yield return new TestCaseData(Range.Of(10L, false, 15L, true))
                    .Returns((11L, 16L, 5L));
            }
        }

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
