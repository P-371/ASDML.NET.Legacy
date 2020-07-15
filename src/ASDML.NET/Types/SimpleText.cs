namespace P371.ASDML.Types
{
    public class SimpleText : Text
    {
        public SimpleText(string value) : base(value) { }

        public static implicit operator SimpleText(string value) => new SimpleText(value: value);

        public static implicit operator string(SimpleText text) => text.Value;
    }
}
