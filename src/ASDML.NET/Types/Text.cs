namespace P371.ASDML.Types
{
    public class Text : Object<string>
    {
        internal Text(string value) : base(value: value) { }

        public static implicit operator Text(string value) => new Text(value: value);

        public static implicit operator string(Text text) => text.Value;
    }
}
