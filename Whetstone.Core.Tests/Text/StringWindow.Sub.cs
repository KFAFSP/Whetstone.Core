using System;
using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

namespace Whetstone.Core.Text
{
    public partial class StringWindowTests
    {
        [TestCaseSource(nameof(SubStringOutOfRangeTestCases))]
        public void Substring_OutOfRange_ThrowsArgumentOutOfRangeException(int AOffset)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = _FAll.Substring(AOffset)
            );
        }

        [UsedImplicitly]
        static IEnumerable SubStringOutOfRangeTestCases
        {
            get
            {
                yield return new TestCaseData(-1);
                yield return new TestCaseData(C_StringLen + 1);
            }
        }

        [TestCaseSource(nameof(SubStringInRangeTestCases))]
        public void Substring_InRange_ReturnsSubstring(int AOffset)
        {
            Assert.That(_FAll.Substring(AOffset), Is.EqualTo(C_String.Substring(AOffset)));
        }

        [UsedImplicitly]
        static IEnumerable SubStringInRangeTestCases
        {
            get
            {
                yield return new TestCaseData(0);
                yield return new TestCaseData(C_StringLen - 1);
            }
        }

        [TestCaseSource(nameof(SubString2OutOfRangeTestCases))]
        public void Substring2_OutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = _FAll.Substring(AOffset, ALength)
            );
        }

        [UsedImplicitly]
        static IEnumerable SubString2OutOfRangeTestCases
        {
            get
            {
                yield return new TestCaseData(-1, 1);
                yield return new TestCaseData(C_StringLen, 1);
                yield return new TestCaseData(0, C_StringLen + 1);
            }
        }

        [TestCaseSource(nameof(SubString2InRangeTestCases))]
        public void Substring2_InRange_ReturnsSubstring(int AOffset, int ALength)
        {
            Assert.That(
                _FAll.Substring(AOffset, ALength),
                Is.EqualTo(C_String.Substring(AOffset, ALength))
            );
        }

        [UsedImplicitly]
        static IEnumerable SubString2InRangeTestCases
        {
            get
            {
                yield return new TestCaseData(0, C_StringLen);
                yield return new TestCaseData(C_StringLen - 1, 1);
            }
        }

        [TestCaseSource(nameof(SubStringOutOfRangeTestCases))]
        public void Subwindow_OutOfRange_ThrowsArgumentOutOfRangeException(int AOffset)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = _FAll.Subwindow(AOffset)
            );
        }

        [TestCaseSource(nameof(SubStringInRangeTestCases))]
        public void Subwindow_InRange_ReturnsSubstring(int AOffset)
        {
            Assert.That(
                _FAll.Subwindow(AOffset).String,
                Is.EqualTo(C_String.Substring(AOffset))
            );
        }

        [TestCaseSource(nameof(SubString2OutOfRangeTestCases))]
        public void Subwindow2_OutOfRange_ThrowsArgumentOutOfRangeException(
            int AOffset,
            int ALength
        )
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = _FAll.Subwindow(AOffset, ALength)
            );
        }

        [TestCaseSource(nameof(SubString2InRangeTestCases))]
        public void Subwindow2_InRange_ReturnsSubstring(int AOffset, int ALength)
        {
            Assert.That(
                _FAll.Subwindow(AOffset, ALength).String,
                Is.EqualTo(C_String.Substring(AOffset, ALength))
            );
        }
    }
}
