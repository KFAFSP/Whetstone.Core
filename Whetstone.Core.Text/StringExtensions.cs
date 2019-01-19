using System;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    /// <summary>
    /// Provides static extension methods for the <see cref="string"/> class.
    /// </summary>
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
                return "null";
            }

            if (AString.Length > AMaxLength)
            {
                var excerpt = AString
                    .Substring(0, AMaxLength)
                    .Replace("\"", "\\\"");

                return $"\"{excerpt}\" + {AString.Length - AMaxLength} char(s)";
            }

            var str = AString
                .Replace("\"", "\\\"");

            return $"\"{str}\"";
        }
    }
}