using System.Runtime.CompilerServices;
using System;
using System.Text;
using System.IO;
using IOStreamReader = System.IO.StreamReader;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;

namespace P371.ASDML
{
    internal class StreamReader : IDisposable
    {
        private TextReader reader { get; }

        public bool EndOfStream => Peek() == unchecked((char)-1);

        public int Line { get; private set; } = 1;

        public int Column { get; private set; } = 1;

        public StreamReader(Stream stream)
        {
            reader = new IOStreamReader(stream: stream);
        }

        public StreamReader(Stream stream, Encoding encoding)
        {
            reader = new IOStreamReader(stream: stream, encoding: encoding);
        }

        public StreamReader(TextReader reader)
        {
            this.reader = reader;
        }

        private void PrepareObjectReading()
        {
            SkipWhiteSpaces();
            if (EndOfStream)
            {
                throw new EndOfStreamException();
            }
        }

        private string ReadDigits()
        {
            StringBuilder builder = new StringBuilder();
            if (!char.IsDigit(c: Peek()))
            {
                UnexpectedCharacter();
            }
            do
            {
                builder.Append(value: Read());
            }
            while (char.IsDigit(c: Peek()));
            return builder.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void UnexpectedCharacter() => throw new UnexpectedCharacterException(unexpectedChar: Peek(), line: Line, column: Column);

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
                throw new ArgumentNullException(paramName: nameof(continueReading));
            }
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                char peek = Peek();
                if (EndOfStream || !continueReading(arg: peek))
                {
                    return (text: builder.ToString(), hit: peek);
                }
                builder.Append(value: Read());
            }
        }

        public Number ReadNumber()
        {
            StringBuilder builder = new StringBuilder();
            PrepareObjectReading();
            if (Peek().In('+', '-'))
            {
                builder.Append(value: Read()); // sign
            }
            builder.Append(value: ReadDigits());
            if (char.IsWhiteSpace(c: Peek()))
            {
                return builder.ToString();
            }
            if (!Peek().In('.', 'E', 'e'))
            {
                UnexpectedCharacter();
            }
            if (Peek() == '.')
            {
                builder.Append(value: Read()); // '.'
                builder.Append(value: ReadDigits());
            }
            if (char.IsWhiteSpace(c: Peek()))
            {
                return builder.ToString();
            }
            if (!Peek().In('E', 'e'))
            {
                UnexpectedCharacter();
            }
            builder.Append(value: Read()); // 'E' || 'e'
            if (!Peek().In('+', '-'))
            {
                UnexpectedCharacter();
            }
            builder.Append(value: Read()); // sign
            builder.Append(value: ReadDigits());
            if (!EndOfStream && !char.IsWhiteSpace(c: Peek()))
            {
                UnexpectedCharacter();
            }
            return builder.ToString();
        }

        public Text ReadText()
        {
            PrepareObjectReading();
            if (Peek() == '"')
            {
                Read(); // Quotation mark
                var (text, _) = ReadUntil(continueReading: c => c != '"');
                Read(); // Quotation mark
                if (!EndOfStream && !char.IsWhiteSpace(c: Peek()))
                {
                    UnexpectedCharacter();
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
                UnexpectedCharacter();
            }
            StringBuilder builder = new StringBuilder();
            do
            {
                builder.Append(value: Read());
            }
            while (Peek().In('_', '.') || char.IsLetterOrDigit(c: Peek()));
            if (!EndOfStream && !char.IsWhiteSpace(c: Peek()))
            {
                UnexpectedCharacter();
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
