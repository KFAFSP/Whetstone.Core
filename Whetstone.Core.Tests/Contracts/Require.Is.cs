using System;

using NUnit.Framework;

// ReSharper disable ExpressionIsAlwaysNull

namespace Whetstone.Core.Contracts
{
    public partial class RequireTests
    {
        [Test]
        public void Is_Null_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => Require.Is<string>(null, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [Test]
        public void Is_DifferentType_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => Require.Is<string>(new object(), C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [Test]
        public void Is_SameType_ReturnsValue()
        {
            var param = "hello";

            Assert.That(Require.Is<string>(param, C_ParamName), Is.SameAs(param));
        }

        [Test]
        public void IsContrainedBy_NullForValueType_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => Require.IsConstrainedBy<int>(null, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [Test]
        public void IsConstrainedBy_NullForReferenceType_ReturnsNull()
        {
            Assert.That(Require.IsConstrainedBy<string>(null, C_ParamName), Is.Null);
        }

        [Test]
        public void IsConstrainedBy_DifferentType_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => Require.IsConstrainedBy<string>(new object(), C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [Test]
        public void IsConstrainedBy_SameType_ReturnsValue()
        {
            var param = "hello";

            Assert.That(Require.IsConstrainedBy<string>(param, C_ParamName), Is.SameAs(param));
        }
    }
}
