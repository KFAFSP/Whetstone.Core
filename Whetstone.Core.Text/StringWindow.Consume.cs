using System;
using System.Text;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    public partial class StringWindow
    {
        #region Consume
        int ConsumeInternal([CanBeNull] StringBuilder AOut, int ACount)
        {
            Ensure.That(
                ACount >= 0,
                $@"Count ({ACount}) < 0.",
                @"This indicates a contract violation."
            );
            Ensure.That(
                ACount <= Length,
                $@"Count ({ACount}) > Length ({Length}).",
                @"This indicates a contract violation."
            );

            if (ACount == 0)
            {
                // Quick exit.
                return 0;
            }

            // Take exactly the requested amount.
            AOut?.Append(Base, Offset, ACount);
            Offset += ACount;
            Length -= ACount;

            return ACount;
        }

        /// <summary>
        /// Consume the contents of this window.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <returns>The number of characters consumed.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters will be discarded.
        /// </para>
        /// </remarks>
        public int Consume([CanBeNull] StringBuilder AOut)
            => ConsumeInternal(AOut, Length);

        /// <summary>
        /// Consume a prefix with an exact length.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ALength">The prefix length.</param>
        /// <returns><paramref name="ALength"/> if consumed; otherwise 0.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters will be discarded.
        /// </para>
        /// <para>
        /// Will only take exactly <paramref name="ALength"/> characters. If less are available,
        /// nothing is consumed.
        /// </para>
        /// </remarks>
        public int Consume([CanBeNull] StringBuilder AOut, int ALength)
        {
            Require.NotNegative(ALength, nameof(ALength));

            return ALength <= Length ? ConsumeInternal(AOut, ALength) : 0;
        }

        /// <summary>
        /// Consume a prefix limited by the specified character.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ALimiter">The limiting character.</param>
        /// <returns>The number of characters consumed.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters will be discarded.
        /// </para>
        /// <para>
        /// The limiter will also be consumed. You can discard it by decrementing the length of
        /// <paramref name="AOut"/>.
        /// </para>
        /// <para>
        /// If the limiter is not found, nothing will be consumed, and 0 is returned.
        /// </para>
        /// </remarks>
        public int Consume(
            [CanBeNull] StringBuilder AOut,
            char ALimiter
        )
        {
            var find = IndexOf(ALimiter);

            return find != -1 ? ConsumeInternal(AOut, find + 1) : 0;
        }

        /// <summary>
        /// Consume a prefix limited by any of the specified characters.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ALimiters">The limiting characters.</param>
        /// <returns>The number of characters consumed.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters will be discarded.
        /// </para>
        /// <para>
        /// The limiter will also be consumed. You can discard it by decrementing the length of
        /// <paramref name="AOut"/>.
        /// </para>
        /// <para>
        /// If no limiter is found, nothing will be consumed, and 0 is returned.
        /// </para>
        /// </remarks>
        public int Consume(
            [CanBeNull] StringBuilder AOut,
            [NotNull] params char[] ALimiters
        )
        {
            var find = IndexOfAny(ALimiters);

            return find != -1 ? ConsumeInternal(AOut, find + 1) : 0;
        }

        /// <summary>
        /// Consume a prefix limited by a matching character.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="APredicate">The <see cref="Predicate{T}"/> to match.</param>
        /// <returns>The number of characters consumed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="APredicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// <paramref name="APredicate"/> threw an exception.
        /// </exception>
        /// <remarks>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters will be discarded.
        /// </para>
        /// <para>
        /// The limiter will also be consumed. You can discard it by decrementing the length of
        /// <paramref name="AOut"/>.
        /// </para>
        /// <para>
        /// If no limiter is found, nothing will be consumed, and 0 is returned.
        /// </para>
        /// </remarks>
        public int Consume(
            [CanBeNull] StringBuilder AOut,
            [NotNull] [InstantHandle] Predicate<char> APredicate
        )
        {
            var find = IndexOf(APredicate);

            return find != -1 ? ConsumeInternal(AOut, find + 1) : 0;
        }

        /// <summary>
        /// Consume a known prefix.
        /// </summary>
        /// <param name="ALiteral">The prefix.</param>
        /// <returns>
        /// The length of <paramref name="ALiteral"/> if it was consumed; otherwise 0.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ALiteral"/> is <see langword="null"/>.
        /// </exception>
        public int Consume([NotNull] string ALiteral)
            => StartsWith(ALiteral) ? ConsumeInternal(null, ALiteral.Length) : 0;
        #endregion

        #region ConsumeEnd
        int ConsumeEndInternal([CanBeNull] StringBuilder AOut, int ACount)
        {
            Ensure.That(
                ACount >= 0,
                $@"Count ({ACount}) < 0.",
                @"This indicates a contract violation."
            );
            Ensure.That(
                ACount <= Length,
                $@"Count ({ACount}) > Length ({Length}).",
                @"This indicates a contract violation."
            );

            if (ACount == 0)
            {
                // Quick exit.
                return 0;
            }

            // Take exactly the requested amount.
            Length -= ACount;
            AOut?.Append(Base, Length, ACount);

            return ACount;
        }

        /// <summary>
        /// Consume a suffix with an exact length.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ALength">The suffix length.</param>
        /// <returns><paramref name="ALength"/> if consumed; otherwise 0.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters will be discarded.
        /// </para>
        /// <para>
        /// Will only take exactly <paramref name="ALength"/> characters. If less are available,
        /// nothing is consumed.
        /// </para>
        /// </remarks>
        public int ConsumeEnd([CanBeNull] StringBuilder AOut, int ALength)
        {
            Require.NotNegative(ALength, nameof(ALength));

            return ALength <= Length ? ConsumeEndInternal(AOut, ALength) : 0;
        }

        /// <summary>
        /// Consume a suffix limited by the specified character.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ALimiter">The limiting character.</param>
        /// <returns>The number of characters consumed.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters will be discarded.
        /// </para>
        /// <para>
        /// The limiter will also be consumed.
        /// </para>
        /// <para>
        /// If the limiter is not found, nothing will be consumed, and 0 is returned.
        /// </para>
        /// </remarks>
        public int ConsumeEnd(
            [CanBeNull] StringBuilder AOut,
            char ALimiter
        )
        {
            var find = LastIndexOf(ALimiter);

            return find != -1 ? ConsumeEndInternal(AOut, Length - find) : 0;
        }

        /// <summary>
        /// Consume a suffix limited by any of the specified characters.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="ALimiters">The limiting characters.</param>
        /// <returns>The number of characters consumed.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters will be discarded.
        /// </para>
        /// <para>
        /// The limiter will also be consumed.
        /// </para>
        /// <para>
        /// If no limiter is found, nothing will be consumed, and 0 is returned.
        /// </para>
        /// </remarks>
        public int ConsumeEnd(
            [CanBeNull] StringBuilder AOut,
            [NotNull] params char[] ALimiters
        )
        {
            var find = LastIndexOfAny(ALimiters);

            return find != -1 ? ConsumeEndInternal(AOut, Length - find) : 0;
        }

        /// <summary>
        /// Consume a suffix limited by a matching character.
        /// </summary>
        /// <param name="AOut">Optional output <see cref="StringBuilder"/>.</param>
        /// <param name="APredicate">The <see cref="Predicate{T}"/> to match.</param>
        /// <returns>The number of characters consumed.</returns>
        /// <remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="APredicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// <paramref name="APredicate"/> threw an exception.
        /// </exception>
        /// <para>
        /// If <paramref name="AOut"/> is <see langword="null"/>, the characters will be discarded.
        /// </para>
        /// <para>
        /// The limiter will also be consumed.
        /// </para>
        /// <para>
        /// If no limiter is found, nothing will be consumed, and 0 is returned.
        /// </para>
        /// </remarks>
        public int ConsumeEnd(
            [CanBeNull] StringBuilder AOut,
            [NotNull] [InstantHandle] Predicate<char> APredicate
        )
        {
            var find = LastIndexOf(APredicate);

            return find != -1 ? ConsumeEndInternal(AOut, Length - find) : 0;
        }

        /// <summary>
        /// Consume a known suffix.
        /// </summary>
        /// <param name="ALiteral">The prefix.</param>
        /// <returns>
        /// The length of <paramref name="ALiteral"/> if it was consumed; otherwise 0.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ALiteral"/> is <see langword="null"/>.
        /// </exception>
        public int ConsumeEnd([NotNull] string ALiteral)
            => EndsWith(ALiteral) ? ConsumeEndInternal(null, ALiteral.Length) : 0;
        #endregion

        #region Trimming
        /// <summary>
        /// Trim whitespaces from the start of the window.
        /// </summary>
        /// <returns>The number of characters removed.</returns>
        public int TrimStart() => Consume(null, char.IsWhiteSpace);

        /// <summary>
        /// Trim whitespaces from the end of the window.
        /// </summary>
        /// <returns>The number of characters removed.</returns>
        public int TrimEnd() => ConsumeEnd(null, char.IsWhiteSpace);

        /// <summary>
        /// Trim whitespaces from the start and end of the window.
        /// </summary>
        public void Trim()
        {
            TrimStart();
            TrimEnd();
        }
        #endregion
    }
}
