using System;
using System.IO;
using System.Text;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using IOStreamReader = System.IO.StreamReader;

namespace P371.ASDML
{
    internal class StreamReader : IDisposable
    {
        private TextReader reader { get; }

        internal bool WhiteSpaceNext => char.IsWhiteSpace(Peek());

        internal UnexpectedCharacterException UnexpectedCharacter => new UnexpectedCharacterException(Peek(), Line, Column);

        public bool EndOfStream => Peek() == unchecked((char)-1);

        public int Line { get; private set; } = 1;

        public int Column { get; private set; } = 1;

        public StreamReader(Stream stream)
        {
            reader = new IOStreamReader(stream);
        }

        public StreamReader(Stream stream, Encoding encoding)
        {
            reader = new IOStreamReader(stream, encoding);
        }

        public StreamReader(TextReader reader)
        {
            this.reader = reader;
        }

        private void PrepareObjectReading()
        {
            SkipWhiteSpaces();
            EnsureNotEndOfStream();
        }

        private void EnsureWhiteSpaceFollows()
        {
            if (!EndOfStream && !WhiteSpaceNext)
            {
                throw UnexpectedCharacter;
            }
        }

        private void EnsureNotEndOfStream()
        {
            if (EndOfStream)
            {
                throw new EndOfStreamException();
            }
        }

        private string ReadDigits()
        {
            EnsureNotEndOfStream();
            StringBuilder builder = new StringBuilder();
            if (!char.IsDigit(Peek()))
            {
                throw UnexpectedCharacter;
            }
            do
            {
                builder.Append(Read());
            }
            while (char.IsDigit(Peek()));
            return builder.ToString();
        }

        public char Read()
        {
            char read = (char)reader.Read();
            if (read == '\n')
            {
                Column = 1;
                Line++;
            }
            else
            {
                Column++;
            }
            return read;
        }

        public (string text, char hit) ReadUntil(Func<char, bool> continueReading)
        {
            if (continueReading == null)
            {
                throw new ArgumentNullException(nameof(continueReading));
            }
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                char peek = Peek();
                if (EndOfStream || !continueReading(peek))
                {
                    return (text: builder.ToString(), hit: peek);
                }
                builder.Append(Read());
            }
        }

        public Number ReadNumber(bool constructor = false)
        {
            StringBuilder builder = new StringBuilder();
            PrepareObjectReading();
            if (Peek().In('+', '-'))
            {
                builder.Append(Read()); // sign
            }
            builder.Append(ReadDigits());
            if (EndOfStream || WhiteSpaceNext || (constructor && Peek() == ')'))
            {
                return builder.ToString();
            }
            if (!Peek().In('.', 'E', 'e'))
            {
                throw UnexpectedCharacter;
            }
            if (Peek() == '.')
            {
                builder.Append(Read()); // '.'
                builder.Append(ReadDigits());
            }
            if (EndOfStream || WhiteSpaceNext || (constructor && Peek() == ')'))
            {
                return builder.ToString();
            }
            if (!Peek().In('E', 'e'))
            {
                throw UnexpectedCharacter;
            }
            builder.Append(Read()); // 'E' || 'e'
            EnsureNotEndOfStream();
            if (!Peek().In('+', '-'))
            {
                throw UnexpectedCharacter;
            }
            builder.Append(Read()); // sign
            builder.Append(ReadDigits());
            if (!constructor || Peek() != ')')
            {
                EnsureWhiteSpaceFollows();
            }
            return builder.ToString();
        }

        public Text ReadText(bool multiLine = false)
        {
            PrepareObjectReading();
            if (Peek() == '"')
            {
                Read(); // Quotation mark
                var (text, hit) = ReadUntil(continueReading: c => c == '\n' ? multiLine : c != '"');
                if (!multiLine && hit != '"')
                {
                    throw UnexpectedCharacter;
                }
                Read(); // Quotation mark
                if (!EndOfStream && !char.IsWhiteSpace(c: Peek()))
                {
                    throw UnexpectedCharacter;
                }
                return text;
            }
            else
            {
                return ReadSimpleText();
            }
        }

        public SimpleText ReadSimpleText()
        {
            PrepareObjectReading();
            if (!char.IsLetter(c: Peek()) && Peek() != '_')
            {
                throw UnexpectedCharacter;
            }
            StringBuilder builder = new StringBuilder();
            do
            {
                builder.Append(Read());
            }
            while (Peek().In('_', '.') || char.IsLetterOrDigit(c: Peek()));
            if (!EndOfStream && !char.IsWhiteSpace(c: Peek()))
            {
                throw UnexpectedCharacter;
            }
            return builder.ToString();
        }

        public char Peek() => (char)reader.Peek();

        public bool SkipWhiteSpaces(bool skipLineBreak = true)
        {
            ReadUntil(continueReading: c => c == '\n' ? skipLineBreak : char.IsWhiteSpace(c));
            return Peek() == '\n';
        }

        public void Dispose() => reader.Dispose();
    }
}
