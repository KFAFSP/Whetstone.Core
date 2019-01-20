using System;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute

namespace Whetstone.Core.Text
{
    public partial class StringWindowTests
    {
        void AssertSeen(int ALength)
        {
            Assert.That(_FOut.ToString(), Is.EqualTo(C_String.Prefix(ALength)));
            Assert.That(_FAll.Offset, Is.EqualTo(0));
            Assert.That(_FAll.Length, Is.EqualTo(C_StringLen));
        }

        [Test]
        public void Look_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = _FAll.Look(null)
            );
        }

        [Test]
        public void Look_SeesAll()
        {
            Assert.That(_FAll.Look(_FOut), Is.EqualTo(C_StringLen));
            AssertSeen(C_StringLen);
        }

        [Test]
        public void Look2_Negative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = _FAll.Look(null, -1)
            );
        }

        [TestCase(0)]
        [TestCase(C_StringLen + 1)]
        public void Look2_NotNegative_SeesGreedily(int ALength)
        {
            var expect = Math.Min(C_StringLen, ALength);
            Assert.That(_FAll.Look(_FOut, ALength), Is.EqualTo(expect));
            AssertSeen(expect);
        }

        void AssertEndSeen(int ALength)
        {
            Assert.That(_FOut.ToString(), Is.EqualTo(C_String.Suffix(ALength)));
            Assert.That(_FAll.Offset, Is.EqualTo(0));
            Assert.That(_FAll.Length, Is.EqualTo(C_StringLen));
        }

        [Test]
        public void LookEnd_Negative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = _FAll.LookEnd(null, -1)
            );
        }

        [TestCase(0)]
        [TestCase(C_StringLen + 1)]
        public void LookEnd_NotNegative_SeesGreedily(int ALength)
        {
            var expect = Math.Min(C_StringLen, ALength);
            Assert.That(_FAll.LookEnd(_FOut, ALength), Is.EqualTo(expect));
            AssertEndSeen(expect);
        }
    }
}
