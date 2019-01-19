using System;

using JetBrains.Annotations;

using Whetstone.Core.Contracts;

namespace Whetstone.Core.Text
{
    public partial class StringWindow
    {
        #region Substring
        /// <summary>
        /// Get a substring from this window.
        /// </summary>
        /// <param name="AOffset">The start offset.</param>
        /// <returns>
        /// The substring starting at <paramref name="AOffset"/> running until the end.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffset"/> is not a valid starting index for this window.
        /// </exception>
        [NotNull]
        public string Substring(int AOffset)
        {
            Require.Index(Length + 1, AOffset, nameof(AOffset));

            return Base.Substring(Offset + AOffset, Length);
        }

        /// <summary>
        /// Get a substring from this window.
        /// </summary>
        /// <param name="AOffset">The start offset.</param>
        /// <param name="ALength">The substring length.</param>
        /// <returns>
        /// The substring starting at <paramref name="AOffset"/> with length
        /// <paramref name="ALength"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALength"/> + <paramref name="AOffset"/> is greater than
        /// <see cref="Length"/>.
        /// </exception>
        [NotNull]
        public string Substring(int AOffset, int ALength)
        {
            Require.IndexRange(
                Length,
                AOffset,
                nameof(AOffset),
                ALength,
                nameof(ALength)
            );

            return Base.Substring(Offset + AOffset, ALength);
        }
        #endregion

        #region Subwindow
        /// <summary>
        /// Get a subwindow from this window.
        /// </summary>
        /// <param name="AOffset">The start offset.</param>
        /// <returns>
        /// A new <see cref="StringWindow"/> starting at <paramref name="AOffset"/> running until
        /// the end.
        /// </returns>
        /// <exception cref = "ArgumentOutOfRangeException" >
        /// <paramref name="AOffset"/> is not a valid starting index for this window.
        /// </exception>
        [NotNull]
        public StringWindow Subwindow(int AOffset)
        {
            Require.Index(Length + 1, AOffset, nameof(AOffset));

            return new StringWindow(Base, Offset + AOffset, Length - AOffset);
        }

        /// <summary>
        /// Get a subwindow from this window.
        /// </summary>
        /// <param name="AOffset">The start offset.</param>
        /// <param name="ALength">The substring length.</param>
        /// <returns>
        /// A new <see cref="StringWindow"/> starting at <paramref name="AOffset"/> with length
        /// <paramref name="ALength"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="AOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="ALength"/> + <paramref name="AOffset"/> is greater than
        /// <see cref="Length"/>.
        /// </exception>
        [NotNull]
        public StringWindow Subwindow(int AOffset, int ALength)
        {
            Require.IndexRange(
                Length,
                AOffset,
                nameof(AOffset),
                ALength,
                nameof(ALength)
            );

            return new StringWindow(Base, Offset + AOffset, ALength);
        }
        #endregion
    }
}
