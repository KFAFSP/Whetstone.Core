using System;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    public partial class StringWindow
    {
        /// <summary>
        /// Find the first occurrence of the specified character.
        /// </summary>
        /// <param name="AChar">The character.</param>
        /// <returns>
        /// The index of the first occurrence of <paramref name="AChar"/>; or -1.
        /// </returns>
        [Pure]
        public int IndexOf(char AChar)
        {
            var find = Base.IndexOf(AChar, Offset, Length);

            return find == -1 ? -1 : find - Offset;
        }

        /// <summary>
        /// Find the first occurrence of a matching character.
        /// </summary>
        /// <param name="APredicate">The <see cref="Predicate{T}"/> to match.</param>
        /// <returns>The index of the first match; or -1.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="APredicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// <paramref name="APredicate"/> threw an exception.
        /// </exception>
        [Pure]
        public int IndexOf([NotNull] [InstantHandle] Predicate<char> APredicate)
        {
            Require.NotNull(APredicate, nameof(APredicate));

            for (var I = 0; I < Length; ++I)
            {
                if (APredicate(this[I])) return I;
            }

            return -1;
        }

        /// <summary>
        /// Find the first occurence of any of the specified characters.
        /// </summary>
        /// <param name="AChars">The characters.</param>
        /// <returns>
        /// The index of the first occurrence of any of <paramref name="AChars"/>; or -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AChars"/> is <see langword="null"/>.
        /// </exception>
        [Pure]
        public int IndexOfAny([NotNull] params char[] AChars)
        {
            var find = Base.IndexOfAny(AChars, Offset, Length);

            return find == -1 ? -1 : find - Offset;
        }

        /// <summary>
        /// Find the last occurrence of the specified character.
        /// </summary>
        /// <param name="AChar">The character.</param>
        /// <returns>
        /// The index of the last occurrence of <paramref name="AChar"/>; or -1.
        /// </returns>
        [Pure]
        public int LastIndexOf(char AChar)
        {
            var find = Base.LastIndexOf(AChar, Offset, Length);

            return find == -1 ? -1 : find - Offset;
        }

        /// <summary>
        /// Find the last occurrence of a matching character.
        /// </summary>
        /// <param name="APredicate">The <see cref="Predicate{T}"/> to match.</param>
        /// <returns>The index of the last match; or -1.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="APredicate"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// <paramref name="APredicate"/> threw an exception.
        /// </exception>
        [Pure]
        public int LastIndexOf([NotNull] [InstantHandle] Predicate<char> APredicate)
        {
            Require.NotNull(APredicate, nameof(APredicate));

            for (var I = Length; I >= 0; --I)
            {
                if (APredicate(this[I])) return I;
            }

            return -1;
        }

        /// <summary>
        /// Find the last occurence of any of the specified characters.
        /// </summary>
        /// <param name="AChars">The characters.</param>
        /// <returns>
        /// The index of the last occurrence of any of <paramref name="AChars"/>; or -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AChars"/> is <see langword="null"/>.
        /// </exception>
        [Pure]
        public int LastIndexOfAny([NotNull] params char[] AChars)
        {
            var find = Base.LastIndexOfAny(AChars, Offset, Length);

            return find == -1 ? -1 : find - Offset;
        }
    }
}
