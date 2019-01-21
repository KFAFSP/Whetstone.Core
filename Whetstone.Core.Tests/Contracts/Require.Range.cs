using System;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public partial class RequireTests
    {
        [Test]
        public void InRange_OutOfRange_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.InRange(Range<int>.Empty, 0, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [Test]
        public void InRange_InRange_ReturnsValue()
        {
            Assert.That(
                Require.InRange(
                    Range.Of(0, true, 5, false),
                    1,
                    C_ParamName
                ),
                Is.EqualTo(1)
            );
        }

        [Test]
        public void InRange2_OutOfRange_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.InRange(
                    Range.Of(0, true, 5, false),
                    Range.Of(3, true, 9, false),
                    C_ParamName
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [Test]
        public void InRange2_InRange_ReturnsValue()
        {
            var rg = Range.Of(1, true, 3, true);
            Assert.That(
                Require.InRange(
                    Range.Of(0, true, 5, false),
                    rg,
                    C_ParamName
                ),
                Is.EqualTo(rg)
            );
        }

        [TestCase(-1)]
        [TestCase(8)]
        public void Span_OffsetOutOfRange_ThrowsArgumentOutOfRangeException(int AOffset)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Span(
                    0.Span(8),
                    AOffset,
                    nameof(AOffset),
                    0,
                    C_ParamName
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AOffset)));
        }

        [Test]
        public void Span_LengthNegative_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Span(
                    0.Span(8),
                    0,
                    C_ParamName,
                    -1,
                    C_ParamName + "2"
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName + "2"));
        }

        [Test]
        public void Span_LengthOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Span(
                    0.Span(8),
                    7,
                    C_ParamName,
                    2,
                    C_ParamName + "2"
                )
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName + "2"));
        }

        [Test]
        public void Span_Valid_ReturnsSpan()
        {
            Assert.That(
                Require.Span(
                    0.Span(8),
                    1,
                    C_ParamName,
                    4,
                    C_ParamName + "2"
                ),
                Is.EqualTo(1.Span(4))
            );
        }
    }
}
