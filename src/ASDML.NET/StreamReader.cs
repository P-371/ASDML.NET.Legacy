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

        public char Peek() => Peek();

        public void Dispose() => reader.Dispose();
    }
}
