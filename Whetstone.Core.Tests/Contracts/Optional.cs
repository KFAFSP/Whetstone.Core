using System;
using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

// ReSharper disable AssignmentIsFullyDiscarded

namespace Whetstone.Core.Contracts
{
    [TestFixture]
    [TestOf(typeof(Optional<>))]
    [Category("Contracts")]
    [Category("Optional")]
    public sealed class OptionalTTests
    {
        static readonly Optional<int> _FNone = Optional<int>.Absent;
        static readonly Optional<int> _FOne = Optional<int>.Present(1);
        static readonly Optional<int> _FTwo = Optional<int>.Present(2);

        [Test]
        public void Default_IsAbsent()
        {
            Assert.That(default(Optional<int>).IsPresent, Is.False);
        }

        [Test]
        public void Present_IsPresentWithValue()
        {
            Assert.That(Optional<int>.Present(1).Value, Is.EqualTo(1));
        }

        [Test]
        public void Absent_IsAbsent()
        {
            Assert.That(Optional<int>.Absent.IsPresent, Is.False);
        }

        [Test]
        public void ThatIs_Absent_ReturnsAbsent()
        {
            Assert.That(Optional<object>.Absent.ThatIs<string>().IsPresent, Is.False);
        }

        [Test]
        public void ThatIs_MatchingPresent_ReturnsPresentCasted()
        {
            var str = "str";

            Assert.That(Optional<object>.Present(str).ThatIs<string>().Value, Is.SameAs(str));
        }

        [Test]
        public void ThatIs_MismatchingPresent_ReturnsAbsent()
        {
            Assert.That(Optional<string>.Present(null).ThatIs<string>().IsPresent, Is.False);
            Assert.That(Optional<object>.Present(1).ThatIs<string>().IsPresent, Is.False);
        }

        [TestCaseSource(nameof(EqualsOptionalTestCases))]
        public bool Equals_Optional(Optional<int> ALhs, Optional<int> ARhs)
            => ALhs.Equals(ARhs);

        [UsedImplicitly]
        static IEnumerable EqualsOptionalTestCases
        {
            get
            {
                yield return new TestCaseData(_FNone, _FNone)
                    .Returns(true);
                yield return new TestCaseData(_FOne, _FOne)
                    .Returns(true);
                yield return new TestCaseData(_FOne, _FNone)
                    .Returns(false);
                yield return new TestCaseData(_FOne, _FTwo)
                    .Returns(false);
            }
        }

        [TestCaseSource(nameof(EqualsAnyTestCases))]
        public bool Equals_Any(Optional<int> ALhs, object ARhs)
            => ALhs.Equals(ARhs);

        [UsedImplicitly]
        static IEnumerable EqualsAnyTestCases
        {
            get
            {
                foreach (var data in EqualsOptionalTestCases)
                    yield return data;

                yield return new TestCaseData(
                    _FNone,
                    null
                ).Returns(false);
                yield return new TestCaseData(
                    _FNone,
                    Optional<string>.Absent
                ).Returns(false);
            }
        }

        [TestCaseSource(nameof(EqualsOptionalTestCases))]
        public bool GetHashCode(Optional<int> ALhs, Optional<int> ARhs)
            => ALhs.GetHashCode() == ARhs.GetHashCode();

        [TestCaseSource(nameof(ToStringTestCases))]
        public string ToString(object AObject)
            => AObject.ToString();

        [UsedImplicitly]
        static IEnumerable ToStringTestCases
        {
            get
            {
                yield return new TestCaseData(_FNone)
                    .Returns($"Optional<{typeof(int).Name}>.Absent");
                yield return new TestCaseData(_FOne)
                    .Returns(_FOne.Value.ToString());
                yield return new TestCaseData(Optional<string>.Present(null))
                    .Returns("");
            }
        }

        [Test]
        public void GetValue_Absent_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _ = _FNone.Value);
        }

        [Test]
        public void OpImplicit_FromValue()
        {
            Optional<int> opt = 1;
            Assert.That(opt.Value, Is.EqualTo(1));
        }

        [Test]
        public void OpExplicit_ToValueAbsent_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _ = (int) _FNone);
        }

        [Test]
        public void OpExplicit_ToValuePresent_ReturnsValue()
        {
            Assert.That((int) _FOne, Is.EqualTo(_FOne.Value));
        }

        [TestCaseSource(nameof(EqualsOptionalTestCases))]
        public bool OpEquals_Optional(Optional<int> ALhs, Optional<int> ARhs)
            => ALhs == ARhs && !(ALhs != ARhs);
    }

    [TestFixture]
    [TestOf(typeof(Optional))]
    [Category("Contracts")]
    [Category("Optional")]
    public sealed partial class OptionalTests { }
}
