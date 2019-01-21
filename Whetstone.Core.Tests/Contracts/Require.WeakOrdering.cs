using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public partial class RequireTests
    {
        [Test]
        public void AtLeast_Less_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.AtLeast(0, -1, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        public void AtLeast_GreaterOrEqual_ReturnsValue(int AMinimum, int AValue)
        {
            Assert.That(
                Require.AtLeast(AMinimum, AValue, nameof(AValue)),
                Is.EqualTo(AValue)
            );
        }

        [TestCase(0, 0)]
        [TestCase(0, -1)]
        public void GreaterThan_LessOrEqual_ThrowsArgumentOutOfRangeException(
            int AMinimum,
            int AValue
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.GreaterThan(AMinimum, AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
        }

        [Test]
        public void GreaterThan_GreaterThan_ReturnsValue()
        {
            Assert.That(
                Require.GreaterThan(0, 1, C_ParamName),
                Is.EqualTo(1)
            );
        }

        [Test]
        public void AtMost_Greater_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.AtMost(0, 1, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [TestCase(0, 0)]
        [TestCase(0, -1)]
        public void AtMost_LessOrEqual_ReturnsValue(int AMaximum, int AValue)
        {
            Assert.That(
                Require.AtMost(AMaximum, AValue, nameof(AValue)),
                Is.EqualTo(AValue)
            );
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        public void LessThan_GreaterOrEqual_ThrowsArgumentOutOfRangeException(
            int AMinimum,
            int AValue
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.LessThan(AMinimum, AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
        }

        [Test]
        public void LessThan_LessThan_ReturnsValue()
        {
            Assert.That(
                Require.LessThan(0, -1, C_ParamName),
                Is.EqualTo(-1)
            );
        }

        [TestCase(1, 0, 0)]
        [TestCase(0, -1, 1)]
        [TestCase(0, 2, 1)]
        public void Between_Outside_ThrowsArgumentOutOfRangeException(
            int AMinimum,
            int AValue,
            int AMaximum
        )
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Between(AMinimum, AValue, nameof(AValue), AMaximum)
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 0, 1)]
        [TestCase(0, 1, 1)]
        public void Between_Inside_ReturnsValue(
            int AMinimum,
            int AValue,
            int AMaximum
        )
        {
            Assert.That(
                Require.Between(AMinimum, AValue, nameof(AValue), AMaximum),
                Is.EqualTo(AValue)
            );
        }
    }
}
