using System;

namespace P371.ASDML.Types
{
    public class MultiLineText : Text
    {
        internal MultiLineText(string value) : base(value) { }

        public static implicit operator MultiLineText(string value)
            => IsMultiline(value)
            ? new MultiLineText(value)
            : throw new ArgumentException("The value is not a multiline text literal", nameof(value));

        public static implicit operator string(MultiLineText text) => text.Value;
    }
}
