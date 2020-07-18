namespace P371.ASDML.Types
{
    public class Text : Object<string>
    {
        internal Text(string value) : base(value: value) { }

        public static bool IsSimple(string text)
        {
            if (text.Length == 0 || (!char.IsLetter(c: text[index: 0]) && text[index: 0] != '_'))
            {
                return false;
            }
            for (int i = 1; i < text.Length; i++)
            {
                if (!char.IsLetterOrDigit(c: text[index: i]) && !text[index: i].In('_', '.'))
                {
                    return false;
                }
            }
            return true;
        }

        public static implicit operator Text(string value) => IsSimple(text: value) ? new SimpleText(value: value) : new Text(value: value);

        public static implicit operator string(Text text) => text.Value;
    }
}
