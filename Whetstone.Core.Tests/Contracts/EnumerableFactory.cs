using System;

using NUnit.Framework;

// ReSharper disable IteratorMethodResultIsIgnored
// ReSharper disable AssignNullToNotNullAttribute

namespace Whetstone.Core.Contracts
{
    [TestFixture]
    [Category("Contracts")]
    [TestOf(typeof(EnumerableFactory))]
    public sealed class EnumerableFactoryTests
    {
        [Test]
        public void RepeatWhile_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => 1.RepeatWhile(null));
        }

        [Test]
        public void RepeatWhile_False_Empty()
        {
            Assert.That(1.RepeatWhile(() => false), Is.Empty);
        }

        [Test]
        public void RepeatWhile_Condition_Repeats()
        {
            var c = 3;

            Assert.That(
                1.RepeatWhile(() => c-- > 0),
                Is.EquivalentTo(1.Repeat(c))
            );
        }

        [Test]
        public void RepeatUntil_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => 1.RepeatUntil(null));
        }

        [Test]
        public void RepeatUntil_True_Once()
        {
            Assert.That(1.RepeatUntil(() => true), Is.EquivalentTo(1.Yield()));
        }

        [Test]
        public void RepeatUntil_Condition_Repeats()
        {
            var c = 3;

            Assert.That(
                1.RepeatUntil(() => --c == 0),
                Is.EquivalentTo(1.Repeat(c))
            );
        }
    }
}
