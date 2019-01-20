using System;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute

namespace Whetstone.Core.Text
{
    public partial class StringWindowTests
    {
        [Test]
        public void IndexOf_NotPresent_ReturnsMinusOne()
        {
            Assert.That(_FAll.IndexOf(':'), Is.EqualTo(-1));
        }

        [TestCase(',')]
        [TestCase(';')]
        public void IndexOf_Present_ReturnsFirstOccurrence(char AChar)
        {
            Assert.That(_FAll.IndexOf(AChar), Is.EqualTo(C_String.IndexOf(AChar)));
        }

        [Test]
        public void IndexOf2_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = _FAll.IndexOf(null));
        }

        [Test]
        public void IndexOf2_NotMatched_ReturnsMinusOne()
        {
            Assert.That(_FAll.IndexOf(char.IsControl), Is.EqualTo(-1));
        }

        [Test]
        public void IndexOf2_Matched_ReturnsFirstOccurrence()
        {
            Assert.That(_FAll.IndexOf(char.IsLetter), Is.EqualTo(0));
        }

        [Test]
        public void IndexOfAny_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = _FAll.IndexOfAny(null));
        }

        [Test]
        public void IndexOfAny_NotPresent_ReturnsMinusOne()
        {
            Assert.That(_FAll.IndexOfAny(':', '#'), Is.EqualTo(-1));
        }

        [Test]
        public void IndexOfAny_Present_ReturnsFirstOccurrence()
        {
            Assert.That(
                _FAll.IndexOfAny(';', ','),
                Is.EqualTo(C_String.IndexOfAny(new [] {';', ','}))
            );
        }

        [Test]
        public void LastIndexOf_NotPresent_ReturnsMinusOne()
        {
            Assert.That(_FAll.LastIndexOf(':'), Is.EqualTo(-1));
        }

        [TestCase(',')]
        [TestCase(';')]
        public void LastIndexOf_Present_ReturnsFirstOccurrence(char AChar)
        {
            Assert.That(_FAll.LastIndexOf(AChar), Is.EqualTo(C_String.LastIndexOf(AChar)));
        }

        [Test]
        public void LastIndexOf2_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = _FAll.LastIndexOf(null));
        }

        [Test]
        public void LastIndexOf2_NotMatched_ReturnsMinusOne()
        {
            Assert.That(_FAll.LastIndexOf(char.IsControl), Is.EqualTo(-1));
        }

        [Test]
        public void LastIndexOf2_Matched_ReturnsFirstOccurrence()
        {
            Assert.That(_FAll.LastIndexOf(char.IsLetter), Is.EqualTo(C_StringLen - 1));
        }

        [Test]
        public void LastIndexOfAny_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = _FAll.LastIndexOfAny(null));
        }

        [Test]
        public void LastIndexOfAny_NotPresent_ReturnsMinusOne()
        {
            Assert.That(_FAll.LastIndexOfAny(':', '#'), Is.EqualTo(-1));
        }

        [Test]
        public void LastIndexOfAny_Present_ReturnsFirstOccurrence()
        {
            Assert.That(
                _FAll.LastIndexOfAny(';', ','),
                Is.EqualTo(C_String.LastIndexOfAny(new[] { ';', ',' }))
            );
        }
    }
}
