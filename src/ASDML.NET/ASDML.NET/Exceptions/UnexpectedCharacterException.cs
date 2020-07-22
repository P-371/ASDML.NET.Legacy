using System;

namespace P371.ASDML.Exceptions
{
    /// <summary>
    /// An exception thrown when an unexpected character 
    /// </summary>
    [Serializable]
    public class UnexpectedCharacterException : Exception
    {
        /// <summary>
        /// The unexpected character
        /// </summary>
        public char Character { get; private set; }

        /// <summary>
        /// The line on which <see cref="Character" /> was peeked, starting at the position where the StreamReader started reading
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// The column on which <see cref="Character" /> was peeked, starting at the position where the StreamReader started reading
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        /// Create a <see cref="UnexpectedCharacterException" /> from a given character, line and column number
        /// </summary>
        /// <param name="unexpectedChar">The character that was unexpected</param>
        /// <param name="line">The line where <paramref name="unexpectedChar" /> was peeked</param>
        /// <param name="column">The column where <paramref name="unexpectedChar" /> was peeked</param>
        public UnexpectedCharacterException(char unexpectedChar, int line, int column)
            : base($"Unexpected '{unexpectedChar}' character ({line}:{column})")
        {
            Character = unexpectedChar;
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Create a <see cref="UnexpectedCharacterException" /> from a given character, line and column number and an inner axception
        /// </summary>
        /// <param name="unexpectedChar">The character that was unexpected</param>
        /// <param name="line">The line where <paramref name="unexpectedChar" /> was peeked</param>
        /// <param name="column">The column where <paramref name="unexpectedChar" /> was peeked</param>
        /// <param name="inner">The inner exception</param>
        public UnexpectedCharacterException(char unexpectedChar, int line, int column, Exception inner)
            : base($"Unexpected '{unexpectedChar}' character ({line}:{column})", inner)
        {
            Character = unexpectedChar;
            Line = line;
            Column = column;
        }
    }
}
