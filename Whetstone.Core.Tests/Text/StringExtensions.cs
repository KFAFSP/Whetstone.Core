using System;
using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

using Whetstone.Core.Contracts;

// ReSharper disable AssignNullToNotNullAttribute

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

        [TestCaseSource(nameof(ExcerptLongStringTestCases))]
        public string Excerpt_LongString_ReturnsExcerptQuote(string AInput)
            => AInput.Excerpt(10);

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

        [Test]
        public void Repeat_Negative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = 'a'.Repeat(-1)
            );
        }

        [TestCase(0)]
        [TestCase(10)]
        public void Repeat_NotNegative_Repeats(int ACount)
        {
            Assert.That(
                'a'.Repeat(ACount),
                Is.EqualTo(string.Concat(EnumerableFactory.Repeat('a', ACount)))
            );
        }

        [Test]
        public void Repeat2_Null_ThrowsArgumentNullException()
        {
            string str = null;

            Assert.Throws<ArgumentNullException>(
                () => _ = str.Repeat(1)
            );
        }

        [Test]
        public void Repeat2_Negative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = "abc".Repeat(-1)
            );
        }

        [TestCase(0)]
        [TestCase(10)]
        public void Repeat2_NotNegative_Repeats(int ACount)
        {
            Assert.That(
                "abc".Repeat(ACount),
                Is.EqualTo(string.Concat(EnumerableFactory.Repeat("abc", ACount)))
            );
        }

        [Test]
        public void Prefix_Null_ThrowsArgumentNullException()
        {
            string str = null;

            Assert.Throws<ArgumentNullException>(
                () => _ = str.Prefix(0)
            );
        }

        [TestCase("abc", -1)]
        [TestCase("abc", 4)]
        public void Prefix_OutOfRange_ThrowsArgumentOutOfRangeException(string AInput, int ALength)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = AInput.Prefix(ALength)
            );
        }

        [TestCase("abc", 0, ExpectedResult = "")]
        [TestCase("abc", 2, ExpectedResult = "ab")]
        public string Prefix_InRange_ReturnsPrefix(string AInput, int ALength)
            => AInput.Prefix(ALength);

        [Test]
        public void Suffix_Null_ThrowsArgumentNullException()
        {
            string str = null;

            Assert.Throws<ArgumentNullException>(
                () => _ = str.Suffix(0)
            );
        }

        [TestCase("abc", -1)]
        [TestCase("abc", 4)]
        public void Suffix_OutOfRange_ThrowsArgumentOutOfRangeException(string AInput, int ALength)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = AInput.Suffix(ALength)
            );
        }

        [TestCase("abc", 0, ExpectedResult = "")]
        [TestCase("abc", 2, ExpectedResult = "bc")]
        public string Suffix_InRange_ReturnsPrefix(string AInput, int ALength)
            => AInput.Suffix(ALength);
    }
}
