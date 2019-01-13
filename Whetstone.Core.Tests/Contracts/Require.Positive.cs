using System;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public sealed partial class RequireTests
    {
		[TestCase(1)]
		public void Positive_Int8_Positive_ReturnsValue(sbyte AParam)
		{
			Assert.That(Require.Positive(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_Int8_NegativeOrZero_ThrowsArgumentOutOfRangeException(sbyte AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.Positive(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(1)]
		public void Positive_Int16_Positive_ReturnsValue(short AParam)
		{
			Assert.That(Require.Positive(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_Int16_NegativeOrZero_ThrowsArgumentOutOfRangeException(short AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.Positive(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(1)]
		public void Positive_Int32_Positive_ReturnsValue(int AParam)
		{
			Assert.That(Require.Positive(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_Int32_NegativeOrZero_ThrowsArgumentOutOfRangeException(int AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.Positive(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(1)]
		public void Positive_Int64_Positive_ReturnsValue(long AParam)
		{
			Assert.That(Require.Positive(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_Int64_NegativeOrZero_ThrowsArgumentOutOfRangeException(long AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.Positive(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(1)]
		public void Positive_Float32_Positive_ReturnsValue(float AParam)
		{
			Assert.That(Require.Positive(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_Float32_NegativeOrZero_ThrowsArgumentOutOfRangeException(float AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.Positive(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(1)]
		public void Positive_Float64_Positive_ReturnsValue(double AParam)
		{
			Assert.That(Require.Positive(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_Float64_NegativeOrZero_ThrowsArgumentOutOfRangeException(double AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.Positive(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

		[TestCase(1)]
		public void Positive_Float128_Positive_ReturnsValue(decimal AParam)
		{
			Assert.That(Require.Positive(AParam, nameof(AParam)), Is.EqualTo(AParam));
		}

		[TestCase(-1)]
		[TestCase(0)]
		public void Positive_Float128_NegativeOrZero_ThrowsArgumentOutOfRangeException(decimal AParam)
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(
				() => Require.Positive(AParam, nameof(AParam))
			);

			Assert.That(ex.ParamName, Is.EqualTo(nameof(AParam)));
		}

    }
}
