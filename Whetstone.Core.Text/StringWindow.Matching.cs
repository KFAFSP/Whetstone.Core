using System;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    public partial class StringWindow
    {
        /// <summary>
        /// Check if a literal prefixes this window.
        /// </summary>
        /// <param name="ALiteral">The literal.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ALiteral"/> is a prefix; otherwise
        /// <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ALiteral"/> is <see langword="null"/>.
        /// </exception>
        [Pure]
        public bool StartsWith([NotNull] string ALiteral)
        {
            Require.NotNull(ALiteral, nameof(ALiteral));

            if (ALiteral.Length == 0)
            {
                // Empty ALiteral is always a prefix.
                return true;
            }

            if (ALiteral.Length > Length)
            {
                // Window is too short to fit ALiteral.
                return false;
            }

            var offset = Offset;
            foreach (var ch in ALiteral)
            {
                if (this[offset++] != ch)
                {
                    // ALiteral was mismatched.
                    return false;
                }
            }

            // ALiteral was matched.
            return true;
        }

        /// <summary>
        /// Check if a literal suffixes this window.
        /// </summary>
        /// <param name="ALiteral">The literal.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ALiteral"/> is a suffix; otherwise
        /// <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ALiteral"/> is <see langword="null"/>.
        /// </exception>
        [Pure]
        public bool EndsWith([NotNull] string ALiteral)
        {
            Require.NotNull(ALiteral, nameof(ALiteral));

            if (ALiteral.Length == 0)
            {
                // Empty ALiteral is always a prefix.
                return true;
            }

            if (ALiteral.Length > Length)
            {
                // Window is too short to fit ALiteral.
                return false;
            }

            var offset = Length - ALiteral.Length;
            foreach (var ch in ALiteral)
            {
                if (this[offset++] != ch)
                {
                    // ALiteral was mismatched.
                    return false;
                }
            }

            // ALiteral was matched.
            return true;
        }
    }
}
