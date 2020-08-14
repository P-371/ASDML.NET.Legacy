namespace P371.ASDML.Types
{
    /// <summary>
    /// A Text in ASDML
    /// </summary>
    public class Text : Object<string>
    {
        internal Text(string value) : base(value) { }

        /// <summary>
        /// Checks if the given <see cref="string" /> is a Simple Text Literal
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>The given text is a Simple Text Literal or not</returns>
        public static bool IsSimple(string text)
        {
            if (text.Length == 0 || text[0].In("@#+-.") || char.IsWhiteSpace(text[0]))
            {
                return false;
            }
            for (int i = 1; i < text.Length; i++)
            {
                if (text[i].In("()[]{}") || char.IsWhiteSpace(text[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the given <see cref="string" /> is a Multiline Text Literal
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>The given text is a Multiline Text Literal or not</returns>
        public static bool IsMultiline(string text) => text.Contains("\n");

        /// <summary>
        /// Creates an ASDML Text Literal from a <see cref="string" />.
        /// This will check if the given text is Simple Text Literal or Multiline Text Literal and return the correct type
        /// </summary>
        /// <param name="value">The <see cref="string" /> to convert</param>
        public static implicit operator Text(string value)
            => IsSimple(value) ? new SimpleText(value) : IsMultiline(value) ? new MultiLineText(value) : new Text(value);

        /// <summary>
        /// Converts an ASDML Text Literal to a <see cref="string" />
        /// </summary>
        /// <param name="text">The <see cref="Text" /> to convert</param>
        public static implicit operator string(Text text) => text.Value;
    }
}
