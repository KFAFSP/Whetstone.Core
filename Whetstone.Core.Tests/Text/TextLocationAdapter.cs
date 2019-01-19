using System;
using System.Collections;
using System.IO;

using JetBrains.Annotations;

using NUnit.Framework;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    [TestFixture]
    [Category("Text")]
    [TestOf(typeof(TextLocationAdapter))]
    public sealed class TextLocationAdapterTests
    {
        [Test]
        public void Constructor_InitialLocation()
        {
            using (var tla = new TextLocationAdapter(TextReader.Null))
            {
                Assert.That(tla.Line, Is.EqualTo(1));
                Assert.That(tla.Character, Is.EqualTo(1));
            }
        }

        [TestCase(-1, 1)]
        [TestCase(0, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        public void Synchronize_NotPostive_ThrowsArgumentOutOfRangeException(int ALine, int AChar)
        {
            using (var tla = new TextLocationAdapter(TextReader.Null))
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => tla.Synchronize(ALine, AChar)
                );
            }
        }

        [Test]
        public void Synchronize_Positive_SetsLocation()
        {
            using (var tla = new TextLocationAdapter(TextReader.Null))
            {
                tla.Synchronize(10, 10);

                Assert.That(tla.Line, Is.EqualTo(10));
                Assert.That(tla.Character, Is.EqualTo(10));
            }
        }

        [TestCase("", ExpectedResult = -1)]
        [TestCase("a", ExpectedResult = 'a')]
        public int Peek_Delegates(string AInput)
        {
            using (var tla = new TextLocationAdapter(new StringReader(AInput)))
            {
                var peek = tla.Peek();

                Assert.That(tla.Line, Is.EqualTo(1));
                Assert.That(tla.Character, Is.EqualTo(1));

                return peek;
            }
        }

        [TestCaseSource(nameof(ReadOnceTestCases))]
        public (int, long, long) Read_Once_DelegatesAndProcesses(string AInput)
        {
            using (var tla = new TextLocationAdapter(new StringReader(AInput)))
            {
                var read = tla.Read();

                return (read, tla.Line, tla.Character);
            }
        }

        [TestCaseSource(nameof(ReadTestCases))]
        public (long, long) Read_DelegatesAndProcesses(string AInput)
        {
            using (var tla = new TextLocationAdapter(new StringReader(AInput)))
            {
                var buffer = new char[AInput.Length];
                var read = tla.Read(buffer, 0, buffer.Length);

                Assert.That(read, Is.EqualTo(buffer.Length));
                Assert.That(string.Concat(buffer), Is.EqualTo(AInput));

                return (tla.Line, tla.Character);
            }
        }

        [TestCaseSource(nameof(ReadTestCases))]
        public (long, long) ReadBlock_DelegatesAndProcesses(string AInput)
        {
            using (var tla = new TextLocationAdapter(new StringReader(AInput)))
            {
                var buffer = new char[AInput.Length];
                var read = tla.ReadBlock(buffer, 0, buffer.Length);

                Assert.That(read, Is.EqualTo(buffer.Length));
                Assert.That(string.Concat(buffer), Is.EqualTo(AInput));

                return (tla.Line, tla.Character);
            }
        }

        [Test]
        public void ReadLine_DelegatesAndProcesses()
        {
            using (var tla = new TextLocationAdapter(new StringReader("abc\r\ndef")))
            {
                var line = tla.ReadLine();

                Assert.That(line, Is.EqualTo("abc"));
                Assert.That(tla.Line, Is.EqualTo(2));
                Assert.That(tla.Character, Is.EqualTo(1));
            }
        }

        [TestCaseSource(nameof(ReadTestCases))]
        public (long, long) ReadToEnd_DelegatesAndProcesses(string AInput)
        {
            using (var tla = new TextLocationAdapter(new StringReader(AInput)))
            {
                var read = tla.ReadToEnd();

                Assert.That(read, Is.EqualTo(AInput));

                return (tla.Line, tla.Character);
            }
        }

        [Test]
        public void Dispose_LeaveOpen_LeavesOpen()
        {
            using (var outer = new StringReader("abc"))
            {
                using (var tla = new TextLocationAdapter(outer, true)) { }

                outer.Read();
            }
        }

        [Test]
        public void Dispose_NotLeaveOpen_Closes()
        {
            using (var outer = new StringReader("abc"))
            {
                using (var tla = new TextLocationAdapter(outer, false)) { }

                Assert.Throws<ObjectDisposedException>(
                    () => _ = outer.Read()
                );
            }
        }

        [Test]
        public void Close_ClosesBase()
        {
            using (var tla = new TextLocationAdapter(new StringReader("abc")))
            {
                tla.Close();

                Assert.Throws<ObjectDisposedException>(
                    () => _ = tla.Base.Read()
                );
            }
        }

        [UsedImplicitly]
        static IEnumerable ReadOnceTestCases
        {
            get
            {
                yield return new TestCaseData("")
                    .Returns((-1, 1L, 1L));
                yield return new TestCaseData("a")
                    .Returns(((int)'a', 1L, 2L));
                yield return new TestCaseData("\n")
                    .Returns(((int)'\n', 2L, 1L));
            }
        }

        [UsedImplicitly]
        static IEnumerable ReadTestCases
        {
            get
            {
                yield return new TestCaseData("")
                    .Returns((1L, 1L));
                yield return new TestCaseData("abc")
                    .Returns((1L, 4L));
                yield return new TestCaseData("a\na\r")
                    .Returns((2L, 3L));
                yield return new TestCaseData("\naaaa\naaa\n")
                    .Returns((4L, 1L));
            }
        }
    }
}
