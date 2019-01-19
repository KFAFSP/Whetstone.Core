using System;
using System.IO;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    /// <summary>
    /// Adapter for the <see cref="TextReader"/> class that tracks the current location.
    /// </summary>
    [PublicAPI]
    public sealed class TextLocationAdapter : TextReader
    {
        const char C_LineFeed = '\n';

        /// <summary>
        /// Create a new <see cref="TextLocationAdapter"/>.
        /// </summary>
        /// <param name="AReader">The underlying <see cref="TextReader"/>.</param>
        /// <param name="ALeaveOpen">
        /// If <see langword="true"/>, <paramref name="AReader"/> will not be closed when this
        /// adapter is disposed.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AReader"/> is <see langword="null"/>.
        /// </exception>
        public TextLocationAdapter([NotNull] TextReader AReader, bool ALeaveOpen = false)
        {
            Require.NotNull(AReader, nameof(AReader));

            Base = AReader;
            LeaveOpen = ALeaveOpen;

            Line = 1;
            Character = 1;
        }

        void UpdatePos([NotNull] char[] ABuffer, int AIndex, int ACount)
        {
            Ensure.NotNull(ABuffer, nameof(ABuffer));
            Ensure.IndexRange(
                ABuffer,
                AIndex,
                nameof(AIndex),
                ACount,
                nameof(ACount)
            );

            // Count all and find the last occurrence of C_LineFeed.
            var last = -1;
            for (var I = 0; I < ABuffer.Length; ++I)
            {
                if (ABuffer[I] == C_LineFeed)
                {
                    Line++;
                    last = I;
                }
            }

            if (last == -1)
            {
                // No C_LineFeed was found, so add the characters.
                Character += ACount;
            }
            else
            {
                // Character location depends on distance to last C_LineFeed.
                Character = ACount - last;
            }
        }
        void UdpatePos([NotNull] string ABuffer)
        {
            Ensure.NotNull(ABuffer, nameof(ABuffer));

            var offset = ABuffer.IndexOf(C_LineFeed);
            if (offset == -1)
            {
                // No C_LineFeed was found, so add the characters.
                Character += ABuffer.Length;
                return;
            }

            // Count all and find the last occurrence of C_LineFeed.
            while (true)
            {
                Line++;

                var find = ABuffer.IndexOf(C_LineFeed, offset + 1);
                if (find == -1)
                {
                    break;
                }

                offset = find;
            }

            // Character location depends on distance to last C_LineFeed.
            Character = ABuffer.Length - offset;
        }

        /// <summary>
        /// Set the location manually.
        /// </summary>
        /// <param name="ALine">The line index.</param>
        /// <param name="ACharacter">The character index.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALine"/> is 0 or less.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ACharacter"/> is 0 or less.
        /// </exception>
        public void Synchronize(int ALine, int ACharacter)
        {
            Require.Positive(ALine, nameof(ALine));
            Require.Positive(ACharacter, nameof(ACharacter));

            Line = ALine;
            Character = ACharacter;
        }

        #region TextReader overrides
        /// <inheritdoc />
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Base"/> is disposed.</exception>
        public override int Peek() => Base.Peek();

        /// <inheritdoc />
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Base"/> is disposed.</exception>
        public override int Read()
        {
            var read = Base.Read();
            switch (read)
            {
                case C_LineFeed:
                    // Line feed increases line index and resets character index.
                    Line++;
                    Character = 1;
                    break;

                case -1:
                    // End of stream propagates.
                    return -1;

                default:
                    // Any other character increases character index.
                    Character++;
                    return read;
            }

            return read;
        }
        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ABuffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AIndex"/> or <paramref name="ACount"/> are negative.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Length of <paramref name="ABuffer"/> minus <paramref name="AIndex"/> is less than
        /// <paramref name="ACount"/>.
        /// </exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Base"/> is disposed.</exception>
        public override int Read(char[] ABuffer, int AIndex, int ACount)
        {
            var read = Base.Read(ABuffer, AIndex, ACount);
            // Scan the buffer to update the location.
            UpdatePos(ABuffer, AIndex, read);

            return read;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ABuffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AIndex"/> or <paramref name="ACount"/> are negative.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Length of <paramref name="ABuffer"/> minus <paramref name="AIndex"/> is less than
        /// <paramref name="ACount"/>.
        /// </exception>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Base"/> is disposed.</exception>
        public override int ReadBlock(char[] ABuffer, int AIndex, int ACount)
        {
            var read = Base.ReadBlock(ABuffer, AIndex, ACount);
            // Scan the buffer to update the location.
            UpdatePos(ABuffer, AIndex, read);

            return read;
        }

        /// <inheritdoc />
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="OutOfMemoryException">
        /// Not enough memory for the line buffer.
        /// </exception>
        /// <exception cref="ObjectDisposedException"><see cref="Base"/> is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The next line is longer than <see cref="int.MaxValue"/>.
        /// </exception>
        public override string ReadLine()
        {
            var read = Base.ReadLine();
            // The semantics of this method enforces this new location.
            Line++;
            Character = 1;

            return read;
        }

        /// <inheritdoc />
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="OutOfMemoryException">Not enough memory for the buffer.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Base"/> is disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The remainder is longer than <see cref="int.MaxValue"/>.
        /// </exception>
        public override string ReadToEnd()
        {
            var read = Base.ReadToEnd();
            // Scan the buffer to update the location.
            UdpatePos(read);

            return read;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Will dispose <see cref="Base"/> if <see cref="LeaveOpen"/> is <see langword="false"/>.
        /// </remarks>
        protected override void Dispose(bool ADisposing)
        {
            // Only dispose Base when LeaveOpen is false.
            if (ADisposing && !LeaveOpen)
            {
                Base.Dispose();
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Will always close <see cref="Base"/>, regardless of <see cref="LeaveOpen"/>.
        /// </remarks>
        public override void Close()
        {
            Base.Close();
        }
        #endregion

        /// <summary>
        /// Get the underlying <see cref="TextReader"/>.
        /// </summary>
        [NotNull]
        public TextReader Base { get; }
        /// <summary>
        /// Get a value indicating whether <see cref="Base"/> will be left open on
        /// <see cref="Dispose"/>.
        /// </summary>
        public bool LeaveOpen { get; }

        /// <summary>
        /// Get the current line location.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The indicated location is always the location of the next character that will be read,
        /// which is the one that can be <see cref="Peek"/>ed.
        /// </para>
        /// <para>
        /// Line indices start at 1.
        /// </para>
        /// </remarks>
        public long Line { get; private set; }
        /// <summary>
        /// Get the current character location.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The indicated location is always the location of the next character that will be read,
        /// which is the one that can be <see cref="Peek"/>ed.
        /// </para>
        /// <para>
        /// Character indices start at 1 for each line.
        /// </para>
        /// </remarks>
        public long Character { get; private set; }
    }
}
