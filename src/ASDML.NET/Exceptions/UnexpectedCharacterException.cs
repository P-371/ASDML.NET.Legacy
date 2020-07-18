using System;

namespace P371.ASDML.Exceptions
{
    [Serializable]
    public class UnexpectedCharacterException : Exception
    {
        public int Line { get; private set; }
        public int Column { get; private set; }

        public UnexpectedCharacterException(char unexpectedChar, int line, int column)
            : base($"Unexpected '{unexpectedChar}' character ({line}:{column})")
        {
            Line = line;
            Column = column;
        }

        public UnexpectedCharacterException(char unexpectedChar, int line, int column, Exception inner)
            : base($"Unexpected '{unexpectedChar}' character ({line}:{column})", inner)
        {
            Line = line;
            Column = column;
        }
    }
}
