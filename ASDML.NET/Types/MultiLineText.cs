using System;

namespace P371.ASDML.Types
{
    /// <summary>
    /// A Multiline Text in ASDML
    /// </summary>
    public sealed class MultiLineText : Text
    {
        internal MultiLineText(string value) : base(value) { }

        /// <summary>
        /// Creates an ASDML Multiline Text Literal from a <see cref="string" />.
        /// This will check if the given text is Multiline Text Literal and throws an <see cref="ArgumentException" /> if not
        /// </summary>
        /// <exception cref="ArgumentException">If <paramref name="value" /> is not a Multiline Text Literal</exception>
        /// <param name="value">The <see cref="string" /> to convert</param>
        public static implicit operator MultiLineText(string value)
            => IsMultiline(value)
            ? new MultiLineText(value)
            : throw new ArgumentException("The value is not a multiline text literal", nameof(value));

        /// <summary>
        /// Converts an ASDML Multiline Text Literal to a <see cref="string" />
        /// </summary>
        /// <param name="text">The <see cref="MultiLineText" /> to convert</param>
        public static implicit operator string(MultiLineText text) => text.Value;
    }
}
