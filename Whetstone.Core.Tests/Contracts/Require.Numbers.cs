using System;

using NUnit.Framework;

// NOTE: ReSharper does not deal well with these generate files.
// ReSharper disable errors
// ReSharper disable All

namespace Whetstone.Core.Contracts
{
    public partial class RequireTests
    {
		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_sbyte_NotNegative_ReturnsValue(sbyte AValue)
		{
			Assert.That(Require.NotNegative(AValue, nameof(AValue)), Is.EqualTo(AValue));
		}

		[Test]
		public void NotNegative_sbyte_Negative_ThrowsArgumentOutOfRangeException()
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.NotNegative((sbyte)-1, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
		}
		[Test]
		public void Positive_sbyte_Positive_ReturnsValue()
		{
			const sbyte value = 1;

			Assert.That(Require.Positive(value, nameof(value)), Is.EqualTo(value));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_sbyte_NullOrLess_ThrowsArgumentOutOfRangeException(sbyte AValue)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Positive(AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
		}
		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_short_NotNegative_ReturnsValue(short AValue)
		{
			Assert.That(Require.NotNegative(AValue, nameof(AValue)), Is.EqualTo(AValue));
		}

		[Test]
		public void NotNegative_short_Negative_ThrowsArgumentOutOfRangeException()
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.NotNegative((short)-1, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
		}
		[Test]
		public void Positive_short_Positive_ReturnsValue()
		{
			const short value = 1;

			Assert.That(Require.Positive(value, nameof(value)), Is.EqualTo(value));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_short_NullOrLess_ThrowsArgumentOutOfRangeException(short AValue)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Positive(AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
		}
		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_int_NotNegative_ReturnsValue(int AValue)
		{
			Assert.That(Require.NotNegative(AValue, nameof(AValue)), Is.EqualTo(AValue));
		}

		[Test]
		public void NotNegative_int_Negative_ThrowsArgumentOutOfRangeException()
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.NotNegative((int)-1, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
		}
		[Test]
		public void Positive_int_Positive_ReturnsValue()
		{
			const int value = 1;

			Assert.That(Require.Positive(value, nameof(value)), Is.EqualTo(value));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_int_NullOrLess_ThrowsArgumentOutOfRangeException(int AValue)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Positive(AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
		}
		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_long_NotNegative_ReturnsValue(long AValue)
		{
			Assert.That(Require.NotNegative(AValue, nameof(AValue)), Is.EqualTo(AValue));
		}

		[Test]
		public void NotNegative_long_Negative_ThrowsArgumentOutOfRangeException()
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.NotNegative((long)-1, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
		}
		[Test]
		public void Positive_long_Positive_ReturnsValue()
		{
			const long value = 1;

			Assert.That(Require.Positive(value, nameof(value)), Is.EqualTo(value));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_long_NullOrLess_ThrowsArgumentOutOfRangeException(long AValue)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => Require.Positive(AValue, nameof(AValue))
            );

            Assert.That(ex.ParamName, Is.EqualTo(nameof(AValue)));
		}
    }
}
