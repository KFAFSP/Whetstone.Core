using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public partial class RangeTests
    {
        [Test]
        public void WithLower()
        {
            var range = Range<int>.Of(0, false, 1, false);

            Assert.That(range.WithLower(1), Is.EqualTo(Range<int>.Of(1, false, 1, false)));
            Assert.That(range.WithLower(1, true), Is.EqualTo(Range<int>.Of(1, true, 1, false)));
            Assert.That(range.WithLower(true), Is.EqualTo(Range<int>.Of(0, true, 1, false)));
        }

        [Test]
        public void WithUpper()
        {
            var range = Range<int>.Of(0, false, 1, false);

            Assert.That(range.WithUpper(2), Is.EqualTo(Range<int>.Of(0, false, 2, false)));
            Assert.That(range.WithUpper(2, true), Is.EqualTo(Range<int>.Of(0, false, 2, true)));
            Assert.That(range.WithUpper(true), Is.EqualTo(Range<int>.Of(0, false, 1, true)));
        }
    }
}
