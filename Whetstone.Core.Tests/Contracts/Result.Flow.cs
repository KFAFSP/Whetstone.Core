using System;

using NUnit.Framework;

// ReSharper disable MustUseReturnValue
// ReSharper disable AssignNullToNotNullAttribute

namespace Whetstone.Core.Contracts
{
    [TestFixture]
    [TestOf(typeof(ResultFlow))]
    [Category("Contracts")]
    [Category("Result")]
    public sealed class ResultFlowTests
    {
        [Test]
        public void AndThen_ContinuationNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Ok().AndThen((Func<int>)null));
        }
        [Test]
        public void AndThen_Erroneous_PropagatesError()
        {
            var error = new Exception();
            Assert.That(Result.Fail(error).AndThen(() => 0).Error.Value, Is.SameAs(error));
        }
        [Test]
        public void AndThen_Successful_ReturnsSuccessfulWithValue()
        {
            Assert.That(Result.Ok().AndThen(() => 1).Value, Is.EqualTo(1));
        }

        [Test]
        public void AndThen2_ContinuationNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Ok(0).AndThen((Func<int, int>)null));
        }
        [Test]
        public void AndThen2_Erroneous_PropagatesError()
        {
            var error = new Exception();
            Assert.That(Result.Fail<int>(error).AndThen(X => 0).Error.Value, Is.SameAs(error));
        }
        [Test]
        public void AndThen2_Successful_ReturnsSuccessfulWithValue()
        {
            Assert.That(Result.Ok(1).AndThen(X => X * 2).Value, Is.EqualTo(2));
        }

        [Test]
        public void AndThenTry_ContinuationNull_ThrowsArgumentNullException()
        {
            Action action = null;
            Assert.Throws<ArgumentNullException>(() => Result.Ok().AndThenTry(action));
        }
        [Test]
        public void AndThenTry_Erroneous_PropagatesError()
        {
            var error = new Exception();
            Assert.That(Result.Fail(error).AndThenTry(() => {}).Error.Value, Is.SameAs(error));
        }
        [Test]
        public void AndThenTry_SuccessfulThrowing_ReturnsErroneousWithError()
        {
            var error = new Exception();
            Assert.That(Result.Ok().AndThenTry(() => throw error).Error.Value, Is.SameAs(error));
        }
        [Test]
        public void AndThenTry_SuccessfulNonThrowing_ReturnsSuccessful()
        {
            Assert.That(Result.Ok().AndThenTry(() => {}).IsSuccess, Is.True);
        }

        [Test]
        public void AndThenTry2_ContinuationNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Ok().AndThenTry((Func<int>)null));
        }
        [Test]
        public void AndThenTry2_Erroneous_PropagatesError()
        {
            var error = new Exception();
            Assert.That(Result.Fail(error).AndThenTry(() => 0).Error.Value, Is.SameAs(error));
        }
        [Test]
        public void AndThenTry2_SuccessfulThrowing_ReturnsErroneousWithError()
        {
            var error = new Exception();
            Assert.That(
                Result.Ok().AndThenTry((Func<int>)(() => throw error)).Error.Value,
                Is.SameAs(error)
            );
        }
        [Test]
        public void AndThenTry2_SuccessfulNonThrowing_ReturnsSuccessfulWithValue()
        {
            Assert.That(Result.Ok().AndThenTry(() => 1).Value, Is.EqualTo(1));
        }

        [Test]
        public void AndThenTry3_ContinuationNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => Result.Ok(0).AndThenTry((Func<int, int>)null)
            );
        }
        [Test]
        public void AndThenTry3_Erroneous_PropagatesError()
        {
            var error = new Exception();
            Assert.That(Result.Fail<int>(error).AndThenTry(X => 0).Error.Value, Is.SameAs(error));
        }
        [Test]
        public void AndThenTry3_SuccessfulThrowing_ReturnsErroneousWithError()
        {
            var error = new Exception();
            Assert.That(
                Result.Ok(1).AndThenTry((Func<int, int>)(X => throw error)).Error.Value,
                Is.SameAs(error)
            );
        }
        [Test]
        public void AndThenTry3_SuccessfulNonThrowing_ReturnsSuccessfulWithValue()
        {
            Assert.That(Result.Ok(1).AndThenTry(X => X * 2).Value, Is.EqualTo(2));
        }
    }
}
