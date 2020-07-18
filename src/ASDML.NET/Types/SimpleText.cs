using System;

namespace P371.ASDML.Types
{
    public class SimpleText : Text
    {
        internal SimpleText(string value) : base(value: value) { }

        public static implicit operator SimpleText(string value)
            => IsSimple(text: value)
            ? new SimpleText(value: value)
            : throw new ArgumentException(message: "The value is not a simple text literal", paramName: nameof(value));

        public static implicit operator string(SimpleText text) => text.Value;
    }
}
