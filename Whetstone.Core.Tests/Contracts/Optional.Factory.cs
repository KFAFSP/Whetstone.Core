using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable AssignmentIsFullyDiscarded

namespace Whetstone.Core.Contracts
{
    public partial class OptionalTests
    {
        [Test]
        public void IfAny_Null_ThrowsArgumentNullException()
        {
            IEnumerable<int> enumerable = null;

            Assert.Throws<ArgumentNullException>(() => _ = enumerable.IfAny());
        }

        [Test]
        public void IfAny_Empty_ReturnsAbsent()
        {
            Assert.That(Enumerable.Empty<int>().IfAny().IsPresent, Is.False);
        }

        [Test]
        public void IfAny_Any_ReturnsFirst()
        {
            Assert.That(Enumerable.Range(1, 3).IfAny().Value, Is.EqualTo(1));
        }
    }
}
