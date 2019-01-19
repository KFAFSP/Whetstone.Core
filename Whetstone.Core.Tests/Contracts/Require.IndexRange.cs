using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using NUnit.Framework;

// ReSharper disable ExpressionIsAlwaysNull

namespace Whetstone.Core.Contracts
{
    public sealed partial class RequireTests
    {
        [TestCaseSource(nameof(IndexRangeOffsetOutOfRangeTestCases))]
        public void IndexRange_Int32_OffsetOutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    3,
                    AOffset,
                    nameof(AOffset),
                    ALength,
                    nameof(ALength)
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AOffset)));
        }

        [TestCaseSource(nameof(IndexRangeLengthOutOfRangeTestCases))]
        public void IndexRange_Int32_LengthOutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    3,
                    AOffset,
                    nameof(AOffset),
                    ALength,
                    nameof(ALength)
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(ALength)));
        }

        [TestCaseSource(nameof(IndexRangeInRangeTestCases))]
        public void IndexRange_Int32_InRange_Returns(int AOffset, int ALength)
        {
            Require.IndexRange(
                3,
                AOffset,
                nameof(AOffset),
                ALength,
                nameof(ALength)
            );
        }

        [TestCaseSource(nameof(IndexRangeOffsetOutOfRangeTestCases))]
        public void IndexRange_String_OffsetOutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    _FString3,
                    AOffset,
                    nameof(AOffset),
                    ALength,
                    nameof(ALength)
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AOffset)));
        }

        [TestCaseSource(nameof(IndexRangeLengthOutOfRangeTestCases))]
        public void IndexRange_String_LengthOutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    _FString3,
                    AOffset,
                    nameof(AOffset),
                    ALength,
                    nameof(ALength)
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(ALength)));
        }

        [TestCaseSource(nameof(IndexRangeInRangeTestCases))]
        public void IndexRange_String_InRange_Returns(int AOffset, int ALength)
        {
            Require.IndexRange(
                _FString3,
                AOffset,
                nameof(AOffset),
                ALength,
                nameof(ALength)
            );
        }

        [Test]
        public void IndexRange_String_NullNonEmpty_ThrowsArgumentOutOfRangeException()
        {
            string str = null;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    str,
                    0,
                    C_ParamName,
                    1,
                    C_ParamName + "2"
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName + "2"));
        }

        [Test]
        public void IndexRange_String_NullEmpty_Returns()
        {
            string str = null;

            Require.IndexRange(
                str,
                0,
                C_ParamName,
                0,
                C_ParamName
            );
        }

        [TestCaseSource(nameof(IndexRangeOffsetOutOfRangeTestCases))]
        public void IndexRange_Array_OffsetOutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    _FArray3,
                    AOffset,
                    nameof(AOffset),
                    ALength,
                    nameof(ALength)
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AOffset)));
        }

        [TestCaseSource(nameof(IndexRangeLengthOutOfRangeTestCases))]
        public void IndexRange_Array_LengthOutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    _FArray3,
                    AOffset,
                    nameof(AOffset),
                    ALength,
                    nameof(ALength)
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(ALength)));
        }

        [TestCaseSource(nameof(IndexRangeInRangeTestCases))]
        public void IndexRange_Array_InRange_Returns(int AOffset, int ALength)
        {
            Require.IndexRange(
                _FArray3,
                AOffset,
                nameof(AOffset),
                ALength,
                nameof(ALength)
            );
        }

        [Test]
        public void IndexRange_Array_NullNotEmpty_ThrowsArgumentOutOfRangeException()
        {
            int[] array = null;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    array,
                    0,
                    C_ParamName,
                    1,
                    C_ParamName + "2"
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName + "2"));
        }

        [Test]
        public void IndexRange_Array_NullEmpty_Returns()
        {
            int[] array = null;

            Require.IndexRange(
                array,
                0,
                C_ParamName,
                0,
                C_ParamName
            );
        }

        [TestCaseSource(nameof(IndexRangeOffsetOutOfRangeTestCases))]
        public void IndexRange_Collection_OffsetOutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    _FList3,
                    AOffset,
                    nameof(AOffset),
                    ALength,
                    nameof(ALength)
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AOffset)));
        }

        [TestCaseSource(nameof(IndexRangeLengthOutOfRangeTestCases))]
        public void IndexRange_Collection_LengthOutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    _FList3,
                    AOffset,
                    nameof(AOffset),
                    ALength,
                    nameof(ALength)
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(ALength)));
        }

        [TestCaseSource(nameof(IndexRangeInRangeTestCases))]
        public void IndexRange_Collection_InRange_Returns(int AOffset, int ALength)
        {
            Require.IndexRange(
                _FList3,
                AOffset,
                nameof(AOffset),
                ALength,
                nameof(ALength)
            );
        }

        [Test]
        public void IndexRange_Collection_NullNotEmpty_ThrowsArgumentOutOfRangeException()
        {
            ICollection<int> collection = null;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.IndexRange(
                    collection,
                    0,
                    C_ParamName,
                    1,
                    C_ParamName + "2"
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName + "2"));
        }

        [Test]
        public void IndexRange_Collection_NullEmpty_Returns()
        {
            ICollection<int> collection = null;

            Require.IndexRange(
                collection,
                0,
                C_ParamName,
                0,
                C_ParamName
            );
        }

        [UsedImplicitly]
        static IEnumerable IndexRangeOffsetOutOfRangeTestCases
        {
            get
            {
                yield return new TestCaseData(-1, 1);
            }
        }

        [UsedImplicitly]
        static IEnumerable IndexRangeLengthOutOfRangeTestCases
        {
            get
            {
                yield return new TestCaseData(0, -1);
                yield return new TestCaseData(0, 4);
                yield return new TestCaseData(1, 3);
                yield return new TestCaseData(2, 2);
                yield return new TestCaseData(3, 1);
            }
        }

        [UsedImplicitly]
        static IEnumerable IndexRangeInRangeTestCases
        {
            get
            {
                for (var offset = 0; offset < 3; ++offset)
                for (var length = 3 - offset; length >= 0; --length)
                    yield return new TestCaseData(offset, length);

                yield return new TestCaseData(4, 0);
            }
        }
    }
}
