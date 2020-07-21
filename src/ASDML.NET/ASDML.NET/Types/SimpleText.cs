using System;

namespace P371.ASDML.Types
{
    public sealed class SimpleText : Text
    {
        internal SimpleText(string value) : base(value) { }

        public static implicit operator SimpleText(string value)
            => IsSimple(value)
            ? new SimpleText(value)
            : throw new ArgumentException("The value is not a simple text literal", nameof(value));

        public static implicit operator string(SimpleText text) => text.Value;
    }
}
