using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public partial class WeakOrderingTests
    {
        [TestCaseSource(nameof(CompareValueTypeTestCases))]
        public int Compare_ValueType(int ALhs, int ARhs)
            => WeakOrdering.Compare(ALhs, ARhs);

        [UsedImplicitly]
        static IEnumerable CompareValueTypeTestCases
        {
            get
            {
                yield return new TestCaseData(0, 0)
                    .Returns(0.CompareTo(0));
                yield return new TestCaseData(1, 0)
                    .Returns(1.CompareTo(0));
                yield return new TestCaseData(0, 1)
                    .Returns(0.CompareTo(1));
            }
        }

        [TestCaseSource(nameof(CompareReferenceTypeTestCases))]
        public int Compare_ReferenceType(Orderable ALhs, Orderable ARhs)
            => WeakOrdering.Compare(ALhs, ARhs);

        [UsedImplicitly]
        static IEnumerable CompareReferenceTypeTestCases
        {
            get
            {
                foreach (var mapped in MapTestCases(CompareValueTypeTestCases, false))
                    yield return mapped;

                yield return new TestCaseData(null, null)
                    .Returns(0);
                yield return new TestCaseData(_FZero, null)
                    .Returns(1);
                yield return new TestCaseData(null, _FZero)
                    .Returns(-1);
            }
        }
    }
}
