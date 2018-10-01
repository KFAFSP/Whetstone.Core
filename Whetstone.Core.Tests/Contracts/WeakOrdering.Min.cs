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
        [TestCaseSource(nameof(MinValueTypeTestCases))]
        public int Min_ValueType(int ALhs, int ARhs)
            => WeakOrdering.Min(ALhs, ARhs);

        [UsedImplicitly]
        static IEnumerable MinValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(0, 0)
                    .Returns(0);
                yield return new TestCaseData(1, 0)
                    .Returns(0);
                yield return new TestCaseData(0, 1)
                    .Returns(0);
            }
        }

        [TestCaseSource(nameof(MinReferenceTypeTestCases))]
        public Orderable Min_ReferenceType(Orderable ALhs, Orderable ARhs)
            => WeakOrdering.Min(ALhs, ARhs);

        [UsedImplicitly]
        static IEnumerable MinReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(MinValueTypeTestCases))
                    yield return data;

                yield return new TestCaseData(_FZero, _FZero2)
                    .Returns(_FZero);

                yield return new TestCaseData(null, null)
                    .Returns(null);
                yield return new TestCaseData(_FZero, null)
                    .Returns(null);
                yield return new TestCaseData(null, _FZero)
                    .Returns(null);
            }
        }

        [Test]
        public void MinIndex_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => WeakOrdering.MinIndex((int[])null)
            );
        }

        [Test]
        public void MinIndex_Empty_ReturnsMinusOne()
        {
            Assert.That(WeakOrdering.MinIndex(new int[0]), Is.EqualTo(-1));
        }

        [TestCaseSource(nameof(MinIndexValueTypeTestCases))]
        public int MinIndex_ValueType(int A0, int A1, int A2)
            => WeakOrdering.MinIndex(A0, A1, A2);

        [UsedImplicitly]
        static IEnumerable MinIndexValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(1, 1, 0)
                    .Returns(2);
                yield return new TestCaseData(1, 0, 0)
                    .Returns(1);
            }
        }

        [TestCaseSource(nameof(MinIndexReferenceTypeTestCases))]
        public int MinIndex_ReferenceType(Orderable A0, Orderable A1, Orderable A2)
            => WeakOrdering.MinIndex(A0, A1, A2);

        [UsedImplicitly]
        static IEnumerable MinIndexReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(MinIndexValueTypeTestCases, false))
                    yield return data;

                yield return new TestCaseData(_FOne, _FOne2, null)
                    .Returns(2);
                yield return new TestCaseData(null, null, _FOne)
                    .Returns(0);
            }
        }

        [Test]
        public void Min2_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => WeakOrdering.Min((IEnumerable<int>)null)
            );
        }

        [Test]
        public void Min2_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => WeakOrdering.Min(Enumerable.Empty<int>())
            );
        }

        [TestCaseSource(nameof(MinNValueTypeTestCases))]
        public int Min2_ValueType(int A0, int A1, int A2)
            => WeakOrdering.Min(new []{A0, A1, A2} as IEnumerable<int>);

        [UsedImplicitly]
        static IEnumerable MinNValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(0, 1, 1).Returns(0);
                yield return new TestCaseData(1, 1, 1).Returns(1);
                yield return new TestCaseData(1, 1, 0).Returns(0);
            }
        }

        [TestCaseSource(nameof(MinNReferenceTypeTestCases))]
        public Orderable Min2_ReferenceType(Orderable A0, Orderable A1, Orderable A2)
            => WeakOrdering.Min(new[] { A0, A1, A2 } as IEnumerable<Orderable>);

        [UsedImplicitly]
        static IEnumerable MinNReferenceTypeTestCases
        {
            get
            {
                foreach (var data in MapTestCases(MinNValueTypeTestCases))
                    yield return data;

                yield return new TestCaseData(null, _FZero, _FOne)
                    .Returns(null);
                yield return new TestCaseData(_FZero, _FOne, null)
                    .Returns(null);
            }
        }

        [Test]
        public void Min3_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => WeakOrdering.Min((int[])null)
            );
        }

        [Test]
        public void Min3_Empty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => WeakOrdering.Min(new int[0])
            );
        }

        [TestCaseSource(nameof(MinNValueTypeTestCases))]
        public int Min3_ValueType(int A0, int A1, int A2)
            => WeakOrdering.Min(A0, A1, A2);

        [TestCaseSource(nameof(MinNReferenceTypeTestCases))]
        public Orderable Min3_ReferenceType(Orderable A0, Orderable A1, Orderable A2)
            => WeakOrdering.Min(A0, A1, A2);
    }
}
