using System.Text;
using System.IO;
using System.Collections.Generic;
using P371.ASDML.Types;

namespace P371.ASDML
{
    public class Parser
    {
        private StreamReader reader;
        private Stack<Group> groupStack;

        internal Parser(StreamReader streamReader) => reader = streamReader;

        public Parser(Stream stream) : this(streamReader: new StreamReader(stream: stream)) { }

        public Parser(Stream stream, Encoding encoding) : this(streamReader: new StreamReader(stream: stream, encoding: encoding)) { }

        public Parser(string text) : this(streamReader: new StreamReader(reader: new StringReader(s: text))) { }

        public Parser(FileInfo file) : this(streamReader: new StreamReader(reader: File.OpenText(path: file.FullName))) { }

        public void Parse()
        {
            groupStack = new Stack<Group>();
            groupStack.Push(item: new Group(name: "__TMP_ROOT_GROUP__"));
            string propertyName = null, groupName = null;
            while (true)
            {
                if (reader.EndOfStream)
                {
                    break;
                }
                if (reader.SkipWhiteSpaces(skipLineBreak: false))
                {
                    if (groupName != null)
                    {
                        AutoAdd(group: groupStack.Peek(), propertyName: propertyName, value: (SimpleText)groupName);
                    }
                    else if (propertyName != null) // Property with no value
                    {
                        reader.UnexpectedCharacter();
                    }
                    propertyName = groupName = null;
                    reader.Read(); // '\n'
                    continue;
                }
                switch (reader.Peek())
                {
                    case '@':
                        break;
                    case '#':
                        break;
                    case '.': // Property
                        reader.Read(); // '.'
                        if (reader.Peek() != '_' && !char.IsLetter(c: reader.Peek()))
                        {
                            reader.UnexpectedCharacter();
                        }
                        propertyName = reader.ReadSimpleText();
                        continue;
                    case '(':
                    case ')':
                        break; // Error
                    case '{':
                        if (groupName == null)
                        {
                            reader.UnexpectedCharacter();
                        }
                        reader.Read(); // '{'
                        // Todo wait for LF
                        Group group = new Group(name: groupName);
                        AutoAdd(group: groupStack.Peek(), propertyName: propertyName, value: group);
                        groupStack.Push(group);
                        break;
                    case '}':
                        reader.Read(); // '}'
                        groupStack.Pop();
                        break;
                    case '+':
                    case '-':
                    case var digit when char.IsDigit(c: digit): // Number
                        Number number = reader.ReadNumber();
                        AutoAdd(group: groupStack.Peek(), propertyName: propertyName, value: number);
                        if (!reader.SkipWhiteSpaces(skipLineBreak: false))
                        {
                            reader.UnexpectedCharacter();
                        }
                        break;
                    case '_':
                    case var letter when char.IsLetter(c: letter): // Simple text / group
                        groupName = reader.ReadSimpleText();
                        continue;
                    case '"': // Text
                        Text text = reader.ReadText();
                        AutoAdd(group: groupStack.Peek(), propertyName: propertyName, value: text);
                        break;
                    default:
                        break; // ¯\_(ツ)_/¯
                }
                propertyName = groupName = null;
            }
        }

        private void AutoAdd(Group group, string propertyName, Object value)
        {
            if (propertyName == null)
            {
                groupStack.Peek().NestedContent.Add(item: value);
            }
            else
            {
                groupStack.Peek().Properties.Add(key: propertyName, value: value);
            }
        }
    }
}
