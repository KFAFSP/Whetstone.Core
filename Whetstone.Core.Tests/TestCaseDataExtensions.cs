using System;
using System.Linq;

using JetBrains.Annotations;

using NUnit.Framework;

namespace Whetstone.Core
{
    internal static class TestCaseDataExtensions
    {
        [NotNull]
        public static TestCaseData WithArguments(
            [NotNull] this TestCaseData AData,
            [NotNull] params object[] AParams
        )
        {
            var with = new TestCaseData(AParams);

            foreach (var key in AData.Properties.Keys)
            {
                with.Properties.Set(key, AData.Properties.Get(key));
            }

            if (AData.HasExpectedResult)
            {
                with.Returns(AData.ExpectedResult);
            }

            return with;
        }

        [NotNull]
        public static TestCaseData MapArguments(
            [NotNull] this TestCaseData AData,
            [NotNull] [InstantHandle] Func<object, object> AMap
        ) => AData.WithArguments(AData.Arguments.Select(AMap).ToArray());
    }
}
