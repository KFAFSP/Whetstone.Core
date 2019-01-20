using System;
using System.Text;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    public partial class StringWindow
    {
        #region Look
        /// <summary>
        /// Peek characters from the start of the window.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ACount">The number of characters to peek.</param>
        /// <returns>The number of characters peeked.</returns>
        [Pure]
        int LookInternal([CanBeNull] StringBuilder AOut, int ACount)
        {
            Ensure.That(
                ACount >= 0,
                $@"Count ({ACount}) < 0.",
                @"This indicates a contract violation."
            );

            if (ACount == 0)
            {
                // Quick exit.
                return 0;
            }

            // Take as many as possible.
            var length = Math.Min(Length, ACount);
            AOut?.Append(Base, Offset, length);

            return length;
        }

        /// <summary>
        /// Get the entire remaining string in the window.
        /// </summary>
        /// <param name="AOut">The output <see cref="StringBuilder"/>.</param>
        /// <returns><see cref="Length"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AOut"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// Also consider using <see cref="String"/>.
        /// </remarks>
        [Pure]
        public int Look([NotNull] StringBuilder AOut)
        {
            Require.NotNull(AOut, nameof(AOut));

            return LookInternal(AOut, Length);
        }

        /// <summary>
        /// Try to look ahead a specific amount of characters.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ALength">The lookahead length.</param>
        /// <returns>The number of characters seen, up to <paramref name="ALength"/>.</returns>
        /// <remarks>
        /// <para>
        /// Looks greedily for as many characters that are available up to including
        /// <paramref name="ALength"/>, but will also accept less.
        /// </para>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters are discarded.
        /// </para>
        /// </remarks>
        [Pure]
        public int Look([CanBeNull] StringBuilder AOut, int ALength)
        {
            Require.NotNegative(ALength, nameof(ALength));

            return LookInternal(AOut, ALength);
        }
        #endregion

        #region LookEnd
        /// <summary>
        /// Peek characters from the end of the window.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ACount">The number of characters to peek.</param>
        /// <returns>The number of characters peeked.</returns>
        [Pure]
        int LookEndInternal([CanBeNull] StringBuilder AOut, int ACount)
        {
            Ensure.That(
                ACount >= 0,
                $@"Count ({ACount}) < 0.",
                @"This indicates a contract violation."
            );

            if (ACount == 0)
            {
                // Quick exit.
                return 0;
            }

            // Take as many as possible.
            var length = Math.Min(Length, ACount);
            AOut?.Append(Base, Offset + Length - length, length);

            return length;
        }

        /// <summary>
        /// Try to look ahead a specific amount of characters from the end.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ALength">The lookahead length.</param>
        /// <returns>The number of characters seen, up to <paramref name="ALength"/>.</returns>
        /// <remarks>
        /// <para>
        /// Looks greedily for as many characters that are available up to including
        /// <paramref name="ALength"/>, but will also accept less.
        /// </para>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters are discarded.
        /// </para>
        /// </remarks>
        [Pure]
        public int LookEnd([CanBeNull] StringBuilder AOut, int ALength)
        {
            Require.NotNegative(ALength, nameof(ALength));

            return LookEndInternal(AOut, ALength);
        }
        #endregion
    }
}
