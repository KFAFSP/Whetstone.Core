﻿using System;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    /// <summary>
    /// Provides static extension methods for the <see cref="string"/> class.
    /// </summary>
    [PublicAPI]
    public static class StringExtensions
    {
        /// <summary>
        /// Get a printable excerpt from a string.
        /// </summary>
        /// <param name="AString">The string.</param>
        /// <param name="AMaxLength">The maximum excerpt length.</param>
        /// <returns>
        /// An excerpt of the string that contains up to the first <paramref name="AMaxLength"/>
        /// characters in <paramref name="AString"/>, optionally indicating the number of remaining
        /// characters.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AMaxLength"/> is 0 or less.
        /// </exception>
        [Pure]
        [NotNull]
        public static string Excerpt([CanBeNull] this string AString, int AMaxLength = 30)
        {
            Require.Positive(AMaxLength, nameof(AMaxLength));

            if (AString == null)
            {
                // Return the null literal.
                return "null";
            }

            if (AString.Length > AMaxLength)
            {
                // Shorten an excerpt and quote it.
                var excerpt = AString
                    .Substring(0, AMaxLength)
                    .Replace("\"", "\\\"");

                // Return the excerpt and the number of remaining characters.
                return $"\"{excerpt}\" + {AString.Length - AMaxLength} char(s)";
            }

            // Quote the string.
            var str = AString
                .Replace("\"", "\\\"");

            // Return the quoted string.
            return $"\"{str}\"";
        }

        /// <summary>
        /// Get a prefix of the string.
        /// </summary>
        /// <param name="AString">The string.</param>
        /// <param name="ALength">The length of the prefix.</param>
        /// <returns>The prefix with length <paramref name="ALength"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AString"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALength"/> is negative or greater than the length of
        /// <paramref name="AString"/>.
        /// </exception>
        [Pure]
        [NotNull]
        public static string Prefix([NotNull] this string AString, int ALength)
        {
            Require.NotNull(AString, nameof(AString));

            return AString.Substring(0, ALength);
        }

        /// <summary>
        /// Get a suffix of the string.
        /// </summary>
        /// <param name="AString">The string.</param>
        /// <param name="ALength">The length of the suffix.</param>
        /// <returns>The suffix with length <paramref name="ALength"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AString"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALength"/> is negative or greater than the length of
        /// <paramref name="AString"/>.
        /// </exception>
        [Pure]
        [NotNull]
        public static string Suffix([NotNull] this string AString, int ALength)
        {
            Require.NotNull(AString, nameof(AString));

            return AString.Substring(AString.Length - ALength);
        }
    }
}