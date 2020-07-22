using System;

namespace P371.ASDML.Types
{
    /// <summary>
    /// A Simple Text in ASDML
    /// </summary>
    public sealed class SimpleText : Text
    {
        internal SimpleText(string value) : base(value) { }

        /// <summary>
        /// Creates an ASDML Simple Text Literal from a <see cref="string" />.
        /// This will check if the given text is Simple Text Literal and throws an <see cref="ArgumentException" /> if not
        /// </summary>
        /// <exception cref="ArgumentException">If <paramref name="value" /> is not a Simple Text Literal</exception>
        /// <param name="value">The <see cref="string" /> to convert</param>
        public static implicit operator SimpleText(string value)
            => IsSimple(value)
            ? new SimpleText(value)
            : throw new ArgumentException("The value is not a simple text literal", nameof(value));

        /// <summary>
        /// Converts an ASDML Simple Text Literal to a <see cref="string" />
        /// </summary>
        /// <param name="text">The <see cref="SimpleText" /> to convert</param>
        public static implicit operator string(SimpleText text) => text.Value;
    }
}
