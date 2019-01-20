using System;

using NUnit.Framework;

using Whetstone.Core.Contracts;

// ReSharper disable AssignNullToNotNullAttribute

namespace Whetstone.Core.Text
{
    public partial class StringWindowTests
    {
        [Test]
        public void StartsWith_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = _FAll.StartsWith(null)
            );
        }

        [TestCase(0)]
        [TestCase(C_StringLen - 1)]
        public void StartWith_Prefix_ReturnsTrue(int ALength)
        {
            Assert.That(_FAll.StartsWith(C_String.Prefix(ALength)));
        }

        [Test]
        public void StartsWith_NotAPrefix_ReturnsFalse()
        {
            Assert.That(!_FAll.StartsWith(":::"));
            Assert.That(!_FAll.StartsWith("a".Repeat(C_StringLen + 1)));
        }

        [Test]
        public void EndsWith_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = _FAll.EndsWith(null)
            );
        }

        [TestCase(0)]
        [TestCase(C_StringLen - 1)]
        public void EndsWith_Suffix_ReturnsTrue(int ALength)
        {
            Assert.That(_FAll.EndsWith(C_String.Suffix(ALength)));
        }

        [Test]
        public void EndsWith_NotASuffix_ReturnsFalse()
        {
            Assert.That(!_FAll.EndsWith(":::"));
            Assert.That(!_FAll.EndsWith("a".Repeat(C_StringLen + 1)));
        }
    }
}
