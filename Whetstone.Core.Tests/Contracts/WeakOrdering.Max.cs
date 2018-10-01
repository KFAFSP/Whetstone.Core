using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Whetstone.Core.Contracts
{
    public partial class WeakOrderingTests
    {
        [TestCaseSource(nameof(MaxValueTypeTestCases))]
        public int Max_ValueType(int ALhs, int ARhs)
            => WeakOrdering.Max(ALhs, ARhs);

        [UsedImplicitly]
        static IEnumerable MaxValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(0, 0)
                    .Returns(0);
                yield return new TestCaseData(1, 0)
                    .Returns(1);
                yield return new TestCaseData(0, 1)
                    .Returns(1);
            }
        }

        [TestCaseSource(nameof(MaxReferenceTypeTestCases))]
        public Orderable Max_ReferenceType(Orderable ALhs, Orderable ARhs)
            => WeakOrdering.Max(ALhs, ARhs);

        [UsedImplicitly]
        static IEnumerable MaxReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(MaxValueTypeTestCases))
                    yield return data;

                yield return new TestCaseData(_FZero, _FZero2)
                    .Returns(_FZero);

                yield return new TestCaseData(null, null)
                    .Returns(null);
                yield return new TestCaseData(_FZero, null)
                    .Returns(_FZero);
                yield return new TestCaseData(null, _FZero)
                    .Returns(_FZero);
            }
        }

        [Test]
        public void MaxIndex_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => WeakOrdering.MaxIndex((int[])null)
            );
        }

        [Test]
        public void MaxIndex_Empty_ReturnsMinusOne()
        {
            Assert.That(WeakOrdering.MaxIndex(new int[0]), Is.EqualTo(-1));
        }

        [TestCaseSource(nameof(MaxIndexValueTypeTestCases))]
        public int MaxIndex_ValueType(int A0, int A1, int A2)
            => WeakOrdering.MaxIndex(A0, A1, A2);

        [UsedImplicitly]
        static IEnumerable MaxIndexValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(0, 0, 1)
                    .Returns(2);
                yield return new TestCaseData(0, 1, 1)
                    .Returns(1);
            }
        }

        [TestCaseSource(nameof(MaxIndexReferenceTypeTestCases))]
        public int MaxIndex_ReferenceType(Orderable A0, Orderable A1, Orderable A2)
            => WeakOrdering.MaxIndex(A0, A1, A2);

        [UsedImplicitly]
        static IEnumerable MaxIndexReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(MaxIndexValueTypeTestCases, false))
                    yield return data;

                yield return new TestCaseData(_FOne, _FOne2, null)
                    .Returns(0);
                yield return new TestCaseData(null, null, null)
                    .Returns(0);
            }
        }

        [Test]
        public void Max2_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => WeakOrdering.Max((IEnumerable<int>)null)
            );
        }

        [Test]
        public void Max2_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => WeakOrdering.Max(Enumerable.Empty<int>())
            );
        }

        [TestCaseSource(nameof(MaxNValueTypeTestCases))]
        public int Max2_ValueType(int A0, int A1, int A2)
            => WeakOrdering.Max(new []{A0, A1, A2} as IEnumerable<int>);

        [UsedImplicitly]
        static IEnumerable MaxNValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(1, 0, 0).Returns(1);
                yield return new TestCaseData(0, 0, 0).Returns(0);
                yield return new TestCaseData(0, 0, 1).Returns(1);
            }
        }

        [TestCaseSource(nameof(MaxNReferenceTypeTestCases))]
        public Orderable Max2_ReferenceType(Orderable A0, Orderable A1, Orderable A2)
            => WeakOrdering.Max(new[] { A0, A1, A2 } as IEnumerable<Orderable>);

        [UsedImplicitly]
        static IEnumerable MaxNReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(MaxNValueTypeTestCases))
                    yield return data;

                yield return new TestCaseData(null, null, null)
                    .Returns(null);
                yield return new TestCaseData(_FZero, _FOne, null)
                    .Returns(_FOne);
            }
        }

        [Test]
        public void Max3_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => WeakOrdering.Max((int[])null)
            );
        }

        [Test]
        public void Max3_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => WeakOrdering.Max(new int[0])
            );
        }

        [TestCaseSource(nameof(MaxNValueTypeTestCases))]
        public int Max3_ValueType(int A0, int A1, int A2)
            => WeakOrdering.Max(A0, A1, A2);

        [TestCaseSource(nameof(MaxNReferenceTypeTestCases))]
        public Orderable Max3_ReferenceType(Orderable A0, Orderable A1, Orderable A2)
            => WeakOrdering.Max(A0, A1, A2);
    }
}
