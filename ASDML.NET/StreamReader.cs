using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using IOStreamReader = System.IO.StreamReader;

namespace P371.ASDML
{
    internal class StreamReader : IDisposable
    {
        private TextReader reader { get; }

        internal bool EndObject => EndOfStream || char.IsWhiteSpace(Peek()) || Peek().In("]{}()");

        internal bool WhiteSpaceNext => char.IsWhiteSpace(Peek());

        internal UnexpectedCharacterException UnexpectedCharacter => new UnexpectedCharacterException(Peek(), Line, Column);

        public const char EndOfStreamChar = unchecked((char)-1);

        public bool EndOfStream => Peek() == EndOfStreamChar;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        private char ReadEscaped()
        {
            EnsureNotEndOfStream();
            if (Peek() != '\\')
            {
                throw UnexpectedCharacter;
            }
            Read(); // '\'
            EnsureNotEndOfStream();
            return Peek() switch
            {
                var v when v.In("\"#\\0abfnrtv") => ReadSingleChar(),
                var v when v.In("ux") => ReadHexChars(Read()),
                _ => throw UnexpectedCharacter
            };

            char ReadSingleChar()
            {
                char peeked = Peek() switch
                {
                    '"' => '"',
                    '#' => '#',
                    '\\' => '\\',
                    '0' => '\0',
                    'a' => '\a',
                    'b' => '\b',
                    'f' => '\f',
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    'v' => '\v',
                    _ => throw UnexpectedCharacter
                };
                Read(); // Read after throwing exception so the char and the position will be correct
                return peeked;
            }

            char ReadHexChars(char mode)
            {
                Func<char, bool> allowed = c => char.IsDigit(c) || c.In("AaBbCcDdEeFf");
                int length = mode == 'u' ? 4 : 2;
                char[] chars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    EnsureNotEndOfStream();
                    if (!allowed(Peek()))
                    {
                        throw UnexpectedCharacter;
                    }
                    chars[i] = Read();
                }
                return (char)Convert.ToInt32(new string(chars), 16);
            }
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

        private (string text, char hit) ReadUntil(Func<char, bool, bool> continueWhile, bool parseEscaped)
        {
            if (continueWhile == null)
            {
                throw new ArgumentNullException(nameof(continueWhile));
            }
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                char peeked = Peek();
                bool escapedRead = false;
                if (parseEscaped && peeked == '\\')
                {
                    peeked = ReadEscaped();
                    escapedRead = true;
                }
                if (EndOfStream || !continueWhile(peeked, escapedRead))
                {
                    return (text: builder.ToString(), hit: EndOfStream ? EndOfStreamChar : peeked);
                }
                builder.Append(escapedRead ? peeked : Read());
            }
        }

        public Number ReadNumber()
        {
            StringBuilder builder = new StringBuilder();
            EnsureNotEndOfStream();
            if (Peek().In("+-"))
            {
                builder.Append(Read()); // sign
            }
            builder.Append(ReadDigits());
            if (EndObject)
            {
                return builder.ToString();
            }
            if (!Peek().In(".Ee"))
            {
                throw UnexpectedCharacter;
            }
            if (Peek() == '.')
            {
                builder.Append(Read()); // '.'
                builder.Append(ReadDigits());
            }
            if (EndObject)
            {
                return builder.ToString();
            }
            if (!Peek().In("Ee"))
            {
                throw UnexpectedCharacter;
            }
            builder.Append(Read()); // 'E' || 'e'
            EnsureNotEndOfStream();
            if (!Peek().In("+-"))
            {
                throw UnexpectedCharacter;
            }
            builder.Append(Read()); // sign
            builder.Append(ReadDigits());
            if (!EndObject)
            {
                throw UnexpectedCharacter;
            }
            return builder.ToString();
        }

        public Text ReadText(bool multiLine = false)
        {
            EnsureNotEndOfStream();
            if (Peek() == '"')
            {
                Read(); // Quotation mark
                var (text, hit) = ReadUntil((c, escapedRead) => escapedRead ? true : (c == '\n' ? multiLine : c != '"'), true);
                if (hit != '"')
                {
                    throw EndOfStream ? new EndOfStreamException() : (Exception)UnexpectedCharacter;
                }
                Read(); // Quotation mark
                if (!EndObject)
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
            EnsureNotEndOfStream();
            Func<char, bool> allowed = c => char.IsLetterOrDigit(c) || c.In("._+-");
            Func<char, bool> allowedFirst = c => char.IsLetter(c) || c.In("_");
            if (EndObject || !allowedFirst(Peek()))
            {
                throw UnexpectedCharacter;
            }
            StringBuilder builder = new StringBuilder();
            do
            {
                builder.Append(Read());
            }
            while (!EndObject && allowed(Peek()));
            if (!EndObject)
            {
                throw UnexpectedCharacter;
            }
            return builder.ToString();
        }

        public char Peek() => (char)reader.Peek();

        public void SkipWhiteSpaces() => ReadUntil((c, _) => char.IsWhiteSpace(c), false);

        public void Dispose() => reader.Dispose();
    }
}
