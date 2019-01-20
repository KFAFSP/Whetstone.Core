using System;
using System.Collections;

using JetBrains.Annotations;

using NUnit.Framework;

// ReSharper disable ExceptionNotDocumented
// ReSharper disable AssignmentIsFullyDiscarded
// ReSharper disable ImpureMethodCallOnReadonlyValueField
// ReSharper disable AssignNullToNotNullAttribute

namespace Whetstone.Core.Contracts
{
    [TestFixture]
    [TestOf(typeof(Result))]
    [Category("Contracts")]
    [Category("Result")]
    public sealed class ResultTests
    {
        static readonly Exception _FError = new Exception();
        static readonly Result _FSuccessful = Result.Ok();
        static readonly Result _FErroneous = Result.Fail(_FError);

        [Test]
        public void Default_IsErroneousWithDefaultError()
        {
            var def = default(Result);

            Assert.That(def.IsSuccess, Is.False);
            Assert.That(def.Error.Value, Is.InstanceOf<Exception>());
        }

        [Test]
        public void Ok_IsSuccessful()
        {
            Assert.That(Result.Ok().IsSuccess, Is.True);
        }

        [Test]
        public void Fail_IsErroneousWithError()
        {
            var error = new Exception();
            var result = Result.Fail(error);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error.Value, Is.SameAs(error));
        }

        [Test]
        public void ThrowIfError_Successful_NoOp()
        {
            _FSuccessful.ThrowIfError();
        }

        [Test]
        public void ThrowIfError_Erroneous_ThrowsError()
        {
            Assert.Throws(Is.SameAs(_FError), () => _FErroneous.ThrowIfError());
        }

        [Test]
        public void OnSuccess_HandlerNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _FSuccessful.OnSuccess(null));
        }

        [Test]
        public void OnSuccess_Successful_CallsHandler()
        {
            var called = false;
            _FSuccessful.OnSuccess(() => called = true);
            Assert.That(called, Is.True);
        }

        [Test]
        public void OnSuccess_Erroneous_DoesNotCallHandler()
        {
            _FErroneous.OnSuccess(Assert.Fail);
        }

        [Test]
        public void OnError_HandlerNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _FSuccessful.OnError(null));
        }

        [Test]
        public void OnError_Successful_DoesNotCallHandler()
        {
            _FSuccessful.OnError(X => Assert.Fail());
        }

        [Test]
        public void OnError_Erroneous_CallsHandlerWithError()
        {
            var called = false;
            _FErroneous.OnError(X =>
            {
                called = true;
                Assert.That(X, Is.SameAs(_FError));
            });
            Assert.That(called, Is.True);
        }

        [Test]
        public void OnError2_HandlerNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _FSuccessful.OnError<InvalidOperationException>(null)
            );
        }

        [Test]
        public void OnError2_Successful_DoesNotCallHandler()
        {
            _FSuccessful.OnError<InvalidOperationException>(X => Assert.Fail());
        }

        [Test]
        public void OnError2_ErroneousWithMismatching_DoesNotCallHandler()
        {
            _FErroneous.OnError<InvalidOperationException>(X => Assert.Fail());
        }

        [Test]
        public void OnError2_ErroneousWithMatching_CallsHandlerWithError()
        {
            var error = new InvalidOperationException();
            var called = false;
            Result.Fail(error).OnError<InvalidOperationException>(X =>
            {
                called = true;
                Assert.That(X, Is.SameAs(error));
            });
            Assert.That(called, Is.True);
        }

        [TestCaseSource(nameof(EqualsResultTestCases))]
        public bool Equals_Optional(Result ALhs, Result ARhs)
            => ALhs.Equals(ARhs);

        [UsedImplicitly]
        static IEnumerable EqualsResultTestCases
        {
            get
            {
                yield return new TestCaseData(_FSuccessful, Result.Ok())
                    .Returns(true);
                yield return new TestCaseData(_FErroneous, _FErroneous)
                    .Returns(true);
                yield return new TestCaseData(_FSuccessful, _FErroneous)
                    .Returns(false);
                yield return new TestCaseData(default(Result), default(Result))
                    .Returns(false);
            }
        }

        [TestCaseSource(nameof(EqualsAnyTestCases))]
        public bool Equals_Any(Result ALhs, object ARhs)
            => ALhs.Equals(ARhs);

        [UsedImplicitly]
        static IEnumerable EqualsAnyTestCases
        {
            get
            {
                foreach (var data in EqualsResultTestCases)
                    yield return data;

                yield return new TestCaseData(
                    _FSuccessful,
                    null
                ).Returns(false);
                yield return new TestCaseData(
                    _FSuccessful,
                    Result<int>.Ok(0)
                ).Returns(false);
            }
        }

        [TestCaseSource(nameof(EqualsResultTestCases))]
        public bool GetHashCode(Result ALhs, Result ARhs)
            => ALhs.GetHashCode() == ARhs.GetHashCode();

        [TestCaseSource(nameof(ToStringTestCases))]
        public string ToString(object AObject)
            => AObject.ToString();

        [UsedImplicitly]
        static IEnumerable ToStringTestCases
        {
            get
            {
                yield return new TestCaseData(_FErroneous)
                    .Returns($"ERROR({_FError.Message})");
                yield return new TestCaseData(_FSuccessful)
                    .Returns("OK");
            }
        }

        [Test]
        public void OpImplicit_ToBoolSuccessful_IsTrue()
        {
            bool value = _FSuccessful;
            Assert.That(value, Is.True);
        }

        [Test]
        public void OpImplicit_ToBoolErroneous_IsFalse()
        {
            bool value = _FErroneous;
            Assert.That(value, Is.False);
        }

        [Test]
        public void OpImplicit_FromFalse_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _ = (Result)false);
        }

        [Test]
        public void OpImplicit_FromTrue_ReturnsSuccessful()
        {
            Result res = true;
            Assert.That(res.IsSuccess, Is.True);
        }

        [Test]
        public void OpImplicit_FromNullException_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = (Result) ((Exception) null));
        }

        [Test]
        public void OpImplicit_FromException_ReturnsErroneousWithError()
        {
            Result res = _FError;

            Assert.That(res.IsSuccess, Is.False);
            Assert.That(res.Error.Value, Is.SameAs(_FError));
        }
    }

    [TestFixture]
    [TestOf(typeof(Result<>))]
    [Category("Contracts")]
    [Category("Result")]
    public sealed class ResultTTests
    {
        static readonly Exception _FError = new Exception();
        static readonly Result<int> _FOne = Result.Ok(1);
        static readonly Result<int> _FTwo = Result.Ok(2);
        static readonly Result<int> _FErroneous = Result.Fail<int>(_FError);

        [Test]
        public void Default_IsErroneousWithDefaultError()
        {
            var def = default(Result<int>);

            Assert.That(def.IsSuccess, Is.False);
            Assert.That(def.Error.Value, Is.InstanceOf<Exception>());
        }

        [Test]
        public void Ok_IsSuccessfulWithValue()
        {
            Assert.That(Result.Ok(1).IsSuccess, Is.True);
            Assert.That(Result.Ok(1).Value, Is.EqualTo(1));
        }

        [Test]
        public void Fail_IsErroneousWithError()
        {
            var error = new Exception();
            var result = Result.Fail<int>(error);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error.Value, Is.SameAs(error));
        }

        [Test]
        public void ThrowIfError_Successful_NoOp()
        {
            _FOne.ThrowIfError();
        }

        [Test]
        public void ThrowIfError_Erroneous_ThrowsError()
        {
            Assert.Throws(Is.SameAs(_FError), () => _FErroneous.ThrowIfError());
        }

        [Test]
        public void OnSuccess_HandlerNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _FOne.OnSuccess(null));
        }

        [Test]
        public void OnSuccess_Successful_CallsHandlerWithValue()
        {
            var called = false;
            _FOne.OnSuccess(X =>
            {
                called = true;
                Assert.That(X, Is.EqualTo(1));
            });
            Assert.That(called, Is.True);
        }

        [Test]
        public void OnSuccess_Erroneous_DoesNotCallHandler()
        {
            _FErroneous.OnSuccess(X => Assert.Fail());
        }

        [Test]
        public void OnError_HandlerNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _FOne.OnError(null));
        }

        [Test]
        public void OnError_Successful_DoesNotCallHandler()
        {
            _FOne.OnError(X => Assert.Fail());
        }

        [Test]
        public void OnError_Erroneous_CallsHandlerWithError()
        {
            var called = false;
            _FErroneous.OnError(X =>
            {
                called = true;
                Assert.That(X, Is.SameAs(_FError));
            });
            Assert.That(called, Is.True);
        }

        [Test]
        public void OnError2_HandlerNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _FOne.OnError<InvalidOperationException>(null)
            );
        }

        [Test]
        public void OnError2_Successful_DoesNotCallHandler()
        {
            _FOne.OnError<InvalidOperationException>(X => Assert.Fail());
        }

        [Test]
        public void OnError2_ErroneousWithMismatching_DoesNotCallHandler()
        {
            _FErroneous.OnError<InvalidOperationException>(X => Assert.Fail());
        }

        [Test]
        public void OnError2_ErroneousWithMatching_CallsHandlerWithError()
        {
            var error = new InvalidOperationException();
            var called = false;
            Result.Fail<int>(error).OnError<InvalidOperationException>(X =>
            {
                called = true;
                Assert.That(X, Is.SameAs(error));
            });
            Assert.That(called, Is.True);
        }

        [TestCaseSource(nameof(EqualsResultTestCases))]
        public bool Equals_Optional(Result<int> ALhs, Result<int> ARhs)
            => ALhs.Equals(ARhs);

        [UsedImplicitly]
        static IEnumerable EqualsResultTestCases
        {
            get
            {
                yield return new TestCaseData(_FOne, Result.Ok(1))
                    .Returns(true);
                yield return new TestCaseData(_FErroneous, _FErroneous)
                    .Returns(true);
                yield return new TestCaseData(_FOne, _FErroneous)
                    .Returns(false);
                yield return new TestCaseData(_FOne, _FTwo)
                    .Returns(false);
                yield return new TestCaseData(default(Result<int>), default(Result<int>))
                    .Returns(false);
            }
        }

        [TestCaseSource(nameof(EqualsAnyTestCases))]
        public bool Equals_Any(Result<int> ALhs, object ARhs)
            => ALhs.Equals(ARhs);

        [UsedImplicitly]
        static IEnumerable EqualsAnyTestCases
        {
            get
            {
                foreach (var data in EqualsResultTestCases)
                    yield return data;

                yield return new TestCaseData(
                    _FOne,
                    null
                ).Returns(false);
                yield return new TestCaseData(
                    _FOne,
                    Result<uint>.Ok(1)
                ).Returns(false);
                yield return new TestCaseData(
                    _FOne,
                    Result.Ok()
                ).Returns(false);
            }
        }

        [TestCaseSource(nameof(EqualsResultTestCases))]
        public bool GetHashCode(Result<int> ALhs, Result<int> ARhs)
            => ALhs.GetHashCode() == ARhs.GetHashCode();

        [TestCaseSource(nameof(ToStringTestCases))]
        public string ToString(object AObject)
            => AObject.ToString();

        [UsedImplicitly]
        static IEnumerable ToStringTestCases
        {
            get
            {
                yield return new TestCaseData(_FErroneous)
                    .Returns($"ERROR({_FError.Message})");
                yield return new TestCaseData(_FOne)
                    .Returns("OK(1)");
            }
        }

        [Test]
        public void GetValue_Successful_ReturnsValue()
        {
            Assert.That(_FOne.Value, Is.EqualTo(1));
        }

        [Test]
        public void GetValue_Erroneous_ThrowsError()
        {
            Assert.Throws(Is.SameAs(_FError), () => _ = _FErroneous.Value);
        }

        [Test]
        public void OpImplicit_ToBoolSuccessful_IsTrue()
        {
            bool value = _FOne;
            Assert.That(value, Is.True);
        }

        [Test]
        public void OpImplicit_ToBoolErroneous_IsFalse()
        {
            bool value = _FErroneous;
            Assert.That(value, Is.False);
        }

        [Test]
        public void OpImplicit_FromValue_ReturnsSuccessfulWithValue()
        {
            Result<int> res = 1;
            Assert.That(res, Is.EqualTo(_FOne));
        }

        [Test]
        public void OpExplicit_ToValueSuccessful_ReturnsValue()
        {
            Assert.That((int) _FOne, Is.EqualTo(1));
        }

        [Test]
        public void OpExplicit_ToValueErroneous_ThrowsError()
        {
            Assert.Throws(Is.SameAs(_FError), () => _ = (int)_FErroneous);
        }

        [Test]
        public void OpImplicit_FromNullException_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = (Result<int>)((Exception)null));
        }

        [Test]
        public void OpImplicit_FromException_ReturnsErroneousWithError()
        {
            Result<int> res = _FError;

            Assert.That(res.IsSuccess, Is.False);
            Assert.That(res.Error.Value, Is.SameAs(_FError));
        }
    }
}
