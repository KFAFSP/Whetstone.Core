using System;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public partial class RequireTests
    {
		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_Int8_PositiveOrZero_ReturnsValue(sbyte AParam)
		{
			Assert.That(Require.NotNegative(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		public void Positive_Int8_Negative_ThrowsArgumentOutOfRangeException(sbyte AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.NotNegative(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_Int16_PositiveOrZero_ReturnsValue(short AParam)
		{
			Assert.That(Require.NotNegative(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		public void Positive_Int16_Negative_ThrowsArgumentOutOfRangeException(short AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.NotNegative(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_Int32_PositiveOrZero_ReturnsValue(int AParam)
		{
			Assert.That(Require.NotNegative(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		public void Positive_Int32_Negative_ThrowsArgumentOutOfRangeException(int AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.NotNegative(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_Int64_PositiveOrZero_ReturnsValue(long AParam)
		{
			Assert.That(Require.NotNegative(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		public void Positive_Int64_Negative_ThrowsArgumentOutOfRangeException(long AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.NotNegative(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_Float32_PositiveOrZero_ReturnsValue(float AParam)
		{
			Assert.That(Require.NotNegative(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		public void Positive_Float32_Negative_ThrowsArgumentOutOfRangeException(float AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.NotNegative(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_Float64_PositiveOrZero_ReturnsValue(double AParam)
		{
			Assert.That(Require.NotNegative(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		public void Positive_Float64_Negative_ThrowsArgumentOutOfRangeException(double AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.NotNegative(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(0)]
		[TestCase(1)]
		public void NotNegative_Float128_PositiveOrZero_ReturnsValue(decimal AParam)
		{
			Assert.That(Require.NotNegative(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		public void Positive_Float128_Negative_ThrowsArgumentOutOfRangeException(decimal AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.NotNegative(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

    }
}
