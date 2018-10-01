using NUnit.Framework;

namespace Whetstone.Core.Contracts
{
    [TestFixture]
    [TestOf(typeof(Require))]
    [Category("Contracts")]
    public sealed partial class RequireTests
    {
        const string C_ParamName = "AParameter";
    }
}