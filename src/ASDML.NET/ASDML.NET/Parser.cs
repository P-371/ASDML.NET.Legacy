using System.Collections.Generic;
using System.IO;
using System.Text;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using static P371.ASDML.GroupConstructionStep;
using InvalidOperationException = System.InvalidOperationException;

namespace P371.ASDML
{
    public class Parser
    {
        private StreamReader reader;

        internal UnexpectedCharacterException UnexpectedCharacter => reader.UnexpectedCharacter;

        internal Parser(StreamReader streamReader) => reader = streamReader;

        public Parser(Stream stream) : this(new StreamReader(stream)) { }

        public Parser(Stream stream, Encoding encoding) : this(new StreamReader(stream, encoding)) { }

        public Parser(string text) : this(new StreamReader(new StringReader(text))) { }

        public Parser(FileInfo file) : this(new StreamReader(File.OpenText(file.FullName))) { }

        public void Parse()
        {
            Stack<Group> groupStack = new Stack<Group>();
            groupStack.Push(Group.CreateRoot());
            string propertyName = null;
            while (true)
            {
                if (reader.EndOfStream)
                {
                    break;
                }
                if (reader.SkipWhiteSpaces(skipLineBreak: false)) // Line break
                {
                    if (groupName != null)
                    {
                        AutoAdd(group: groupStack.Peek(), propertyName: propertyName, value: (SimpleText)groupName);
                    }
                    else if (propertyName != null) // Property with no value
                    {
                        throw UnexpectedCharacter;
                    }
                    propertyName = groupName = null;
                    reader.Read(); // '\n'
                    continue;
                }
                switch (reader.Peek())
                {
                    case '@':
                        reader.Read(); // '@'
                        if (reader.Peek() == '"') // Multiline text
                        {
                            Text text = reader.ReadText(multiLine: true);
                            AutoAdd(group: groupStack.Peek(), propertyName: propertyName, value: text);
                        }
                        else if (reader.Peek().In('t', 'f', 'n')) // Logical / @null
                        {
                            var (text, _) = reader.ReadUntil(continueReading: c => !char.IsWhiteSpace(c: c));
                            AutoAdd(group: groupStack.Peek(), propertyName: propertyName, value: text switch
                            {
                                "true" => (Logical)true,
                                "false" => (Logical)false,
                                "null" => Object.Null,
                                _ => throw UnexpectedCharacter
                            });
                        }
                        else
                        {
                            throw UnexpectedCharacter;
                        }
                        break;
                    case '#':
                        break;
                    case '.': // Property
                        reader.Read(); // '.'
                        if (reader.Peek() != '_' && !char.IsLetter(c: reader.Peek()))
                        {
                            throw UnexpectedCharacter;
                        }
                        propertyName = reader.ReadSimpleText();
                        continue;
                    case '(':
                    case ')':
                        break; // Error
                    case '{':
                        if (groupName == null)
                        {
                            throw UnexpectedCharacter;
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
                            throw UnexpectedCharacter;
                        }
                        break;
                    case '_':
                    case var letter when char.IsLetter(c: letter): // Simple text / group
                        groupName = reader.ReadSimpleText();
                        continue;
                    case '"': // Text
                        Text singleLineText = reader.ReadText();
                        AutoAdd(group: groupStack.Peek(), propertyName: propertyName, value: singleLineText);
                        break;
                    default:
                        throw UnexpectedCharacter;
                }
                propertyName = groupName = null;
            }
        }

        private void AutoAdd(Group group, string propertyName, Object value)
        {
            switch (group.ConstructionStep)
            {
                case Constructor:
                    group.ConstructorParameters.Add(value);
                    break;
                case Done:
            if (propertyName == null)
            {
                        group.NestedContent.Add(value);
            }
            else
            {
                        group.Properties.Add(propertyName, value);
                    }
                    break;
                default:
                    throw new System.InvalidOperationException();
            }
        }
    }
}
