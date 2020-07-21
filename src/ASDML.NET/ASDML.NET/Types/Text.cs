namespace P371.ASDML.Types
{
    public class Text : Object<string>
    {
        internal Text(string value) : base(value) { }

        public static bool IsSimple(string text)
        {
            char[] disallowed = { '"', '(', ')', '[', ']', '{', '}' };
            char[] disallowedFirst = { '@', '#', '+', '-', '.' };
            if (text.Length == 0 || text[0].In(disallowedFirst) || char.IsWhiteSpace(text[0]))
            {
                return false;
            }
            for (int i = 1; i < text.Length; i++)
            {
                if (text[i].In(disallowed) || char.IsWhiteSpace(text[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsMultiline(string text) => text.Contains("\n");

        public static implicit operator Text(string value)
            => IsSimple(value) ? new SimpleText(value) : IsMultiline(value) ? new MultiLineText(value) : new Text(value);

        public static implicit operator string(Text text) => text.Value;
    }
}
