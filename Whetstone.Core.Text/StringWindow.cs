using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    /// <summary>
    /// Provides methods to read a contiguous window into a <see cref="string"/>.
    /// </summary>
    /// <remarks>
    /// While <see cref="StringBuilder"/>s are used to build strings in memory using a backing
    /// mutable storage, <see cref="StringWindow"/>s are used to dissect strings in memory using
    /// a backing immutable storage.
    /// </remarks>
    [PublicAPI]
    public sealed partial class StringWindow : IEnumerable<char>
    {
        /// <summary>
        /// Create a new <see cref="StringWindow"/>.
        /// </summary>
        /// <param name="AString">The underlying <see cref="string"/>.</param>
        /// <param name="AOffset">The start offset.</param>
        /// <returns>
        /// A new <see cref="StringWindow"/> into <paramref name="AString"/> starting at
        /// <paramref name="AOffset"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AString"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffset"/> is not a valid starting index for <paramref name="AString"/>.
        /// </exception>
        [ContractAnnotation("AString: null => halt")]
        [NotNull]
        public static StringWindow Of([NotNull] string AString, int AOffset = 0)
        {
            Require.NotNull(AString, nameof(AString));
            Require.Index(AString.Length + 1, AOffset, nameof(AOffset));

            return new StringWindow(AString, AOffset, AString.Length - AOffset);
        }

        /// <summary>
        /// Create a new <see cref="StringWindow"/>.
        /// </summary>
        /// <param name="AString">The underlying <see cref="string"/>.</param>
        /// <param name="AOffset">The start offset.</param>
        /// <param name="ALength">The window length.</param>
        /// <returns>
        /// A new <see cref="StringWindow"/> into <paramref name="AString"/> starting at
        /// <paramref name="AOffset"/> with length <paramref name="ALength"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="AString"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALength"/> + <paramref name="AOffset"/> is greater than the length of
        /// <paramref name="AString"/>.
        /// </exception>
        [ContractAnnotation("AString: null => halt")]
        [NotNull]
        public static StringWindow Of([NotNull] string AString, int AOffset, int ALength)
        {
            Require.NotNull(AString, nameof(AString));
            Require.IndexRange(
                AString,
                AOffset,
                nameof(AOffset),
                ALength,
                nameof(ALength)
            );

            return new StringWindow(AString, AOffset, ALength);
        }

        StringWindow([NotNull] string ABase, int AOffset, int ALength)
        {
            Ensure.NotNull(ABase, nameof(ABase));
            Ensure.IndexRange(
                ABase,
                AOffset,
                nameof(AOffset),
                ALength,
                nameof(ALength)
            );

            Base = ABase;
            Offset = AOffset;
            Length = ALength;
        }

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region IEnumerable<char>
        /// <inheritdoc />
        public IEnumerator<char> GetEnumerator()
        {
            for (var I = 0; I < Length; ++I)
            {
                yield return this[I];
            }
        }
        #endregion

        #region System.Object overrides
        /// <inheritdoc />
        [Pure]
        public override string ToString() => String.Excerpt();
        #endregion

        /// <summary>
        /// Get the underlying <see cref="string"/>.
        /// </summary>
        [NotNull]
        public string Base { get; }
        /// <summary>
        /// Get the offset in <see cref="Base"/>.
        /// </summary>
        public int Offset { get; private set; }
        /// <summary>
        /// Get the window length.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Get the contents of this window as a <see cref="string"/>.
        /// </summary>
        /// <remarks>
        /// Also consider using <see cref="Look(StringBuilder)"/>.
        /// </remarks>
        [NotNull]
        public string String => Base.Substring(Offset, Length);

        /// <summary>
        /// Get a character in this window.
        /// </summary>
        /// <param name="AIndex">The character index.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="AIndex"/> does not transform to a valid index in <see cref="Base"/>.
        /// </exception>
        /// <remarks>
        /// <paramref name="AIndex"/> can be negative or greater than <see cref="Length"/>, as long
        /// as it transforms to a valid index in <see cref="Base"/> when added to
        /// <see cref="Offset"/>.
        /// </remarks>
        public char this[int AIndex] => Base[Offset + AIndex];
    }
}
