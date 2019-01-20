using System;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented
// ReSharper disable AssignmentIsFullyDiscarded
// ReSharper disable AssignNullToNotNullAttribute

namespace Whetstone.Core.Contracts
{
    [TestFixture]
    [TestOf(typeof(OptionalFlow))]
    [Category("Contracts")]
    [Category("Optional")]
    public sealed class OptionalFlowTests
    {
        static readonly Optional<int> _FNone = Optional<int>.Absent;
        static readonly Optional<int> _FOne = Optional<int>.Present(1);
        static readonly Optional<int> _FTwo = Optional<int>.Present(2);

        [Test]
        public void That_PredicateNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = _FNone.That(null));
        }

        [Test]
        public void That_Absent_ReturnsAbsent()
        {
            Assert.That(_FNone.That(X => true).IsPresent, Is.False);
        }

        [Test]
        public void That_MismatchingPresent_ReturnsAbsent()
        {
            Assert.That(_FOne.That(X => false).IsPresent, Is.False);
        }

        [Test]
        public void That_MatchingPresent_ReturnsValue()
        {
            Assert.That(_FOne.That(X => true).Value, Is.EqualTo(1));
        }

        [Test]
        public void Forward_ActionNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _FNone.Forward(null));
        }

        [Test]
        public void Forward_Absent_DoesNotInvokeAndReturnsFalse()
        {
            Assert.That(!_FNone.Forward(X => Assert.Fail()));
        }

        [Test]
        public void Forward_Present_InvokesWithValueAndReturnsTrue()
        {
            var invoked = false;

            Assert.That(_FOne.Forward(X =>
            {
                invoked = true;
                Assert.That(X, Is.EqualTo(1));
            }));
            Assert.That(invoked, Is.True);
        }

        [Test]
        public void Map_FuncNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = _FNone.Map((Func<int, bool>)null));
        }

        [Test]
        public void Map_Absent_DoesNotInvokeAndReturnsAbsent()
        {
            Assert.That(
                _FNone.Map(X =>
                {
                    Assert.Fail();
                    return true;
                }).IsPresent,
                Is.False
            );
        }

        [Test]
        public void Map_Present_InvokesAndReturnsPresentResult()
        {
            Assert.That(
                _FTwo.Map(X => X % 2 == 0).Value,
                Is.True
            );
        }

        [Test]
        public void OrDefault_Absent_ReturnsDefault()
        {
            Assert.That(
                _FNone.OrDefault(),
                Is.EqualTo(default(int))
            );
        }

        [Test]
        public void OrDefault_Present_ReturnsValue()
        {
            Assert.That(
                _FOne.OrDefault(),
                Is.EqualTo(1)
            );
        }
    }
}
