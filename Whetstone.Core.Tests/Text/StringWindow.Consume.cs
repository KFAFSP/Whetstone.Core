using System;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute

namespace Whetstone.Core.Text
{
    public partial class StringWindowTests
    {
        void AssertConsumed(int ALength)
        {
            Assert.That(_FOut.ToString(), Is.EqualTo(C_String.Prefix(ALength)));
            Assert.That(_FAll.Offset, Is.EqualTo(ALength));
            Assert.That(_FAll.Length, Is.EqualTo(C_StringLen - ALength));
        }

        [Test]
        public void Consume_ConsumesAll()
        {
            Assert.That(_FAll.Consume(_FOut), Is.EqualTo(C_StringLen));
            AssertConsumed(C_StringLen);
        }

        [Test]
        public void Consume2_Negative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = _FAll.Consume(null, -1)
            );
        }

        [Test]
        public void Consume2_NotAvailable_DoesNotConsume()
        {
            Assert.That(_FAll.Consume(null, C_StringLen + 1), Is.Zero);
            AssertConsumed(0);
        }

        [TestCase(1)]
        [TestCase(C_StringLen)]
        public void Consume2_Available_Consumes(int ALength)
        {
            Assert.That(_FAll.Consume(_FOut, ALength), Is.EqualTo(ALength));
            AssertConsumed(ALength);
        }

        [Test]
        public void Consume3_NotPresent_DoesNotConsume()
        {
            Assert.That(_FAll.Consume(null, ':'), Is.Zero);
            AssertConsumed(0);
        }

        [Test]
        public void Consume3_Present_ConsumesToDelimiter()
        {
            Assert.That(_FAll.Consume(_FOut, ','), Is.EqualTo(4));
            AssertConsumed(4);
        }

        [Test]
        public void Consume4_CharsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = _FAll.Consume(null, (char[]) null)
            );
        }

        [Test]
        public void Consume4_NotPresent_DoesNotConsume()
        {
            Assert.That(_FAll.Consume(null, ':', '#'), Is.Zero);
            AssertConsumed(0);
        }

        [Test]
        public void Consume4_Present_ConsumesToDelimiter()
        {
            Assert.That(_FAll.Consume(_FOut, ';', ':'), Is.EqualTo(8));
            AssertConsumed(8);
        }

        [Test]
        public void Consume5_PredicateNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = _FAll.Consume(null, (Predicate<char>) null)
            );
        }

        [Test]
        public void Consume5_NotMatched_DoesNotConsume()
        {
            Assert.That(_FAll.Consume(null, char.IsControl), Is.Zero);
            AssertConsumed(0);
        }

        [Test]
        public void Consume5_Matched_ConsumesToDelimiter()
        {
            Assert.That(_FAll.Consume(_FOut, char.IsLetter), Is.EqualTo(1));
            AssertConsumed(1);
        }

        [Test]
        public void Consume6_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = _FAll.Consume((string) null)
            );
        }

        [TestCase(0)]
        [TestCase(C_StringLen - 1)]
        public void Consume6_Prefix_ConsumesPrefix(int ALength)
        {
            var pfx = C_String.Prefix(ALength);
            Assert.That(_FAll.Consume(pfx), Is.EqualTo(ALength));
            _FOut.Append(pfx);
            AssertConsumed(ALength);
        }

        [Test]
        public void Consume6_NotAPrefix_DoesNotConsume()
        {
            Assert.That(_FAll.Consume(":::"), Is.Zero);
            AssertConsumed(0);
        }

        void AssertEndConsumed(int ALength)
        {
            Assert.That(_FOut.ToString(), Is.EqualTo(C_String.Suffix(ALength)));
            Assert.That(_FAll.Offset, Is.Zero);
            Assert.That(_FAll.Length, Is.EqualTo(C_StringLen - ALength));
        }

        [Test]
        public void ConsumeEnd_Negative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => _ = _FAll.ConsumeEnd(null, -1)
            );
        }

        [Test]
        public void ConsumeEnd_NotAvailable_DoesNotConsume()
        {
            Assert.That(_FAll.ConsumeEnd(null, C_StringLen + 1), Is.Zero);
            AssertEndConsumed(0);
        }

        [TestCase(1)]
        [TestCase(C_StringLen)]
        public void ConsumeEnd_Available_Consumes(int ALength)
        {
            Assert.That(_FAll.ConsumeEnd(_FOut, ALength), Is.EqualTo(ALength));
            AssertEndConsumed(ALength);
        }

        [Test]
        public void ConsumeEnd2_NotPresent_DoesNotConsume()
        {
            Assert.That(_FAll.ConsumeEnd(null, ':'), Is.Zero);
            AssertEndConsumed(0);
        }

        [Test]
        public void ConsumeEnd2_Present_ConsumesToDelimiter()
        {
            Assert.That(_FAll.ConsumeEnd(_FOut, ','), Is.EqualTo(4));
            AssertEndConsumed(4);
        }

        [Test]
        public void ConsumeEnd3_CharsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = _FAll.ConsumeEnd(null, (char[]) null)
            );
        }

        [Test]
        public void ConsumeEnd3_NotPresent_DoesNotConsume()
        {
            Assert.That(_FAll.ConsumeEnd(null, ':', '#'), Is.Zero);
            AssertEndConsumed(0);
        }

        [Test]
        public void ConsumeEnd3_Present_ConsumesToDelimiter()
        {
            Assert.That(_FAll.ConsumeEnd(_FOut, ';', ':'), Is.EqualTo(2));
            AssertEndConsumed(2);
        }

        [Test]
        public void ConsumeEnd4_PredicateNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = _FAll.ConsumeEnd(null, (Predicate<char>) null)
            );
        }

        [Test]
        public void ConsumeEnd4_NotMatched_DoesNotConsume()
        {
            Assert.That(_FAll.ConsumeEnd(null, char.IsControl), Is.Zero);
            AssertEndConsumed(0);
        }

        [Test]
        public void ConsumeEnd4_Matched_ConsumesToDelimiter()
        {
            Assert.That(_FAll.ConsumeEnd(_FOut, char.IsLetter), Is.EqualTo(1));
            AssertEndConsumed(1);
        }

        [Test]
        public void ConsumeEnd5_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _ = _FAll.ConsumeEnd((string) null)
            );
        }

        [TestCase(0)]
        [TestCase(C_StringLen - 1)]
        public void ConsumeEnd5_Suffix_ConsumesPrefix(int ALength)
        {
            var sfx = C_String.Suffix(ALength);
            Assert.That(_FAll.ConsumeEnd(sfx), Is.EqualTo(ALength));
            _FOut.Append(sfx);
            AssertEndConsumed(ALength);
        }

        [Test]
        public void ConsumeEnd5_NotASuffix_DoesNotConsume()
        {
            Assert.That(_FAll.ConsumeEnd(":::"), Is.Zero);
            AssertEndConsumed(0);
        }

        [TestCase(" a ", ExpectedResult = 1)]
        [TestCase(" \t\ra\n", ExpectedResult = 3)]
        public int TrimStart(string AInput)
        {
            var window = StringWindow.Of(AInput);
            var result = window.TrimStart();

            Assert.That(window.String, Is.EqualTo(AInput.TrimStart()));

            return result;
        }

        [TestCase("\na ", ExpectedResult = 1)]
        [TestCase(" a\t\r ", ExpectedResult = 3)]
        public int TrimEnd(string AInput)
        {
            var window = StringWindow.Of(AInput);
            var result = window.TrimEnd();

            Assert.That(window.String, Is.EqualTo(AInput.TrimEnd()));

            return result;
        }

        [Test]
        public void Trim()
        {
            const string C_Test = "\t\r\n a bc asd\n ";
            var window = StringWindow.Of(C_Test);
            window.Trim();

            Assert.That(window.String, Is.EqualTo(C_Test.Trim()));
        }
    }
}
