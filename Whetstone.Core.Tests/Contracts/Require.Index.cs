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
        static readonly string _FString3 = @"012";
        static readonly int[] _FArray3 = {0, 1, 2};
        static readonly List<int> _FList3 = new List<int> {0, 1, 2};

        [TestCaseSource(nameof(IndexOutOfRangeTestCases))]
        public void Index_Int32_OutOfRange_ThrowsArgumentOutOfRangeException(int AValue)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Index(3, AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
        }

        [TestCaseSource(nameof(IndexInRangeTestCases))]
        public void Index_Int32_InRange_ReturnsValue(int AValue)
        {
            Assert.That(
                Require.Index(3, AValue, nameof(AValue)),
                Is.EqualTo(AValue)
            );
        }

        [TestCaseSource(nameof(IndexOutOfRangeTestCases))]
        public void Index_String_OutOfRange_ThrowsArgumentOutOfRangeException(int AValue)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Index(_FString3, AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
        }

        [TestCaseSource(nameof(IndexInRangeTestCases))]
        public void Index_String_InRange_ReturnsValue(int AValue)
        {
            Assert.That(
                Require.Index(_FString3, AValue, nameof(AValue)),
                Is.EqualTo(AValue)
            );
        }

        [Test]
        public void Index_String_Null_ThrowsArgumentOutOfRangeException()
        {
            string str = null;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Index(str, 0, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [TestCaseSource(nameof(IndexOutOfRangeTestCases))]
        public void Index_Array_OutOfRange_ThrowsArgumentOutOfRangeException(int AValue)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Index(_FArray3, AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
        }

        [TestCaseSource(nameof(IndexInRangeTestCases))]
        public void Index_Array_InRange_ReturnsValue(int AValue)
        {
            Assert.That(
                Require.Index(_FArray3, AValue, nameof(AValue)),
                Is.EqualTo(AValue)
            );
        }

        [Test]
        public void Index_Array_Null_ThrowsArgumentOutOfRangeException()
        {
            int[] array = null;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Index(array, 0, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [TestCaseSource(nameof(IndexOutOfRangeTestCases))]
        public void Index_Collection_OutOfRange_ThrowsArgumentOutOfRangeException(int AValue)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Index(_FList3, AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
        }

        [TestCaseSource(nameof(IndexInRangeTestCases))]
        public void Index_Collection_InRange_ReturnsValue(int AValue)
        {
            Assert.That(
                Require.Index(_FList3, AValue, nameof(AValue)),
                Is.EqualTo(AValue)
            );
        }

        [Test]
        public void Index_Collection_Null_ThrowsArgumentOutOfRangeException()
        {
            ICollection<int> collection = null;

            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Index(collection, 0, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [UsedImplicitly]
        static IEnumerable IndexOutOfRangeTestCases
        {
            get
            {
                yield return new TestCaseData(-1);
                yield return new TestCaseData(3);
            }
        }

        [UsedImplicitly]
        static IEnumerable IndexInRangeTestCases
        {
            get
            {
                yield return new TestCaseData(0);
                yield return new TestCaseData(1);
                yield return new TestCaseData(2);
            }
        }
    }
}
