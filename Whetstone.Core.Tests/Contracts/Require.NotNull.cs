using System;

using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    public partial class RequireTests
    {
        [Test]
        public void NotNull_Null_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => Require.NotNull((object)null, C_ParamName)
            );

            Assert.That(ex.ParamName, Is.EqualTo(C_ParamName));
        }

        [Test]
        public void NotNull_NotNull_ReturnsValue()
        {
            var param = new object();

            Assert.That(Require.NotNull(param, C_ParamName), Is.SameAs(param));
        }
    }
}
