using System;
using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    [TestFixture]
    [Category("Text")]
    [TestOf(typeof(StringExtensions))]
    public sealed class StringExtensionsTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void Excerpt_NonPositiveMaxLength_ThrowsArgumentOutOfRangeException(int AMaxLength)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = "".Excerpt(AMaxLength)
            );
        }

        [TestCaseSource(nameof(ExcerptShortStringTestCases))]
        public string Excerpt_ShortString_ReturnsQuotedString(string AInput)
            => AInput.Excerpt(10);

        [TestCaseSource(nameof(ExcerptLongStringTestCases))]
        public string Excerpt_LongString_ReturnsExcerptQuote(string AInput)
            => AInput.Excerpt(10);

        [UsedImplicitly]
        static IEnumerable ExcerptShortStringTestCases
        {
            get
            {
                yield return new TestCaseData(null)
                    .Returns("null");
                yield return new TestCaseData("")
                    .Returns("\"\"");
                yield return new TestCaseData("null")
                    .Returns("\"null\"");
                yield return new TestCaseData("0123456789")
                    .Returns("\"0123456789\"");
            }
        }

        [UsedImplicitly]
        static IEnumerable ExcerptLongStringTestCases
        {
            get
            {
                yield return new TestCaseData("00000000001")
                    .Returns("\"0000000000\" + 1 char(s)");
                yield return new TestCaseData(string.Concat("0".Repeat(100)))
                    .Returns("\"0000000000\" + 90 char(s)");
            }
        }
    }
}
