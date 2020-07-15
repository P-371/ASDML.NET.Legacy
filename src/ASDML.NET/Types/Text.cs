namespace P371.ASDML.Types
{
    public class Text : Object<string>
    {
        public Text(string value) : base(value) { }

        public static implicit operator Text(string value) => new Text(value: value);

        public static implicit operator string(Text text) => text.Value;
    }
}
