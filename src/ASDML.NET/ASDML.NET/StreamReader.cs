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

        public Text ReadText(bool multiLine = false, bool constructor = false)
        {
            PrepareObjectReading();
            if (Peek() == '"')
            {
                Read(); // Quotation mark
                var (text, hit) = ReadUntil(c => c == '\n' ? multiLine : c != '"');
                if (hit != '"')
                {
                    throw EndOfStream ? new EndOfStreamException() : (Exception)UnexpectedCharacter;
                }
                Read(); // Quotation mark
                if (!constructor || Peek() != ')')
                {
                    EnsureWhiteSpaceFollows();
                }
                return text;
            }
            else
            {
                return ReadSimpleText(constructor);
            }
        }

        public SimpleText ReadSimpleText(bool constructor = false)
        {
            PrepareObjectReading();
            char[] disallowed = { '"', '(', ')', '[', ']', '{', '}' };
            char[] disallowedFirst = { '@', '#', '+', '-', '.' };
            if (Peek().In(disallowedFirst) || char.IsDigit(Peek()))
            {
                throw UnexpectedCharacter;
            }
            StringBuilder builder = new StringBuilder();
            do
            {
                builder.Append(Read());
            }
            while (!EndOfStream && !Peek().In(disallowed) && !WhiteSpaceNext);
            if (!constructor || Peek() != ')')
            {
                EnsureWhiteSpaceFollows();
            }
            return builder.ToString();
        }

        public char Peek() => (char)reader.Peek();

        public void SkipWhiteSpaces() => ReadUntil(c => char.IsWhiteSpace(c));

        public void Dispose() => reader.Dispose();
    }
}