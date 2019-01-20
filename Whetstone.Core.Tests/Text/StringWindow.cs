using System;
using System.Text;

using NUnit.Framework;

using Whetstone.Core.Contracts;

// ReSharper disable AssignNullToNotNullAttribute

namespace Whetstone.Core.Text
{
    [TestFixture]
    [Category("Text")]
    [TestOf(typeof(StringWindow))]
    public sealed partial class StringWindowTests
    {
        const string C_String = @"abc,d,e;f";
        const int C_StringLen = 9;

        static readonly StringBuilder _FOut = new StringBuilder();
        static StringWindow _FEmpty;
        static StringWindow _FAll;

        [SetUp]
        public void Setup()
        {
            _FOut.Clear();
            _FEmpty = StringWindow.Of(C_String, 0, 0);
            _FAll = StringWindow.Of(C_String);
        }

        [Test]
        public void Of_StringIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = StringWindow.Of(null)
            );
        }

        [TestCase(-1)]
        [TestCase(C_StringLen + 1)]
        public void Of_OffsetOutOfRange_ThrowsArgumentOutOfRangeException(int AOffset)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = StringWindow.Of(C_String, AOffset)
            );
        }

        [TestCase(0)]
        [TestCase(C_StringLen)]
        public void Of_InRange_WindowOfSubstring(int AOffset)
        {
            var window = StringWindow.Of(C_String, AOffset);

            Assert.That(window.Offset, Is.EqualTo(AOffset));
            Assert.That(window.String, Is.EqualTo(C_String.Substring(AOffset)));
        }

        [Test]
        public void Of2_StringIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = StringWindow.Of(null, 0, 0)
            );
        }

        [TestCase(-1, 1)]
        [TestCase(0, -1)]
        public void Of2_OutOfRange_ThrowsArgumentOutOfRangeException(int AOffset, int ALength)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = StringWindow.Of(C_String, AOffset, ALength)
            );
        }

        [TestCase(0, C_StringLen)]
        [TestCase(1, C_StringLen - 1)]
        [TestCase(C_StringLen, 0)]
        public void Of2_InRange_WindowOfSubstring(int AOffset, int ALength)
        {
            var window = StringWindow.Of(C_String, AOffset, ALength);

            Assert.That(window.Offset, Is.EqualTo(AOffset));
            Assert.That(window.Length, Is.EqualTo(ALength));
            Assert.That(
                window.String,
                Is.EqualTo(C_String.Substring(AOffset, ALength))
            );
        }

        [Test]
        public void GetEnumerator_YieldsCharacters()
        {
            var window = StringWindow.Of(C_String, 1, 3);
            CollectionAssert.AreEqual(
                window,
                C_String.Substring(1, 3)
            );
        }

        [Test]
        public void ToString_ReturnsExcerpt()
        {
            var str = "a".Repeat(100);
            var window = StringWindow.Of(str);

            Assert.That(window.ToString(), Is.EqualTo(str.Excerpt()));
        }

        [Test]
        public void this_Get_ReturnsAllBaseCharacters()
        {
            var window = StringWindow.Of(C_String, 1, 4);

            for (var I = 0; I < C_StringLen; ++I)
            {
                Assert.That(window[I - 1], Is.EqualTo(C_String[I]));
            }
        }
    }
}
