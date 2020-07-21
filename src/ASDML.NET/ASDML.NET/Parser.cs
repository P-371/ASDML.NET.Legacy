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
                Group currentGroup = groupStack.Peek();
                GroupConstructionStep currentStep = currentGroup.ConstructionStep;
                reader.SkipWhiteSpaces();
                if (reader.EndOfStream)
                {
                    if (currentGroup.ConstructionStep != Done)
                    {
                        throw new EndOfStreamException();
                    }
                    break;
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
                        if (reader.WhiteSpaceNext || propertyName != null)
                        {
                            throw UnexpectedCharacter;
                        }
                        propertyName = reader.ReadSimpleText(currentStep == Constructor);
                        continue;
                    case '(':
                        if (currentStep >= Constructor)
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // '('
                        currentGroup.ConstructionStep = Constructor;
                        break;
                    case ')':
                        if (currentStep != Constructor)
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // ')'
                        currentGroup.ConstructionStep = ConstructorDone;
                        break;
                    case '{':
                        if (currentStep == Done)
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // '{'
                        currentGroup.ConstructionStep = Done;
                        break;
                    case '}':
                        if (propertyName != null || groupStack.Count <= 1)
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // '}'
                        groupStack.Pop();
                        break;
                    case '+':
                    case '-':
                    case var digit when char.IsDigit(digit): // Number
                        Number number = reader.ReadNumber(currentStep == Constructor);
                        AutoAdd(currentGroup, propertyName, number);
                        break;
                    case '_':
                    case var letter when char.IsLetter(letter): // Simple text / group
                        SimpleText simpleText = reader.ReadSimpleText(currentStep == Constructor);
                        reader.SkipWhiteSpaces();
                        if (!reader.EndOfStream)
                        {
                            if (reader.Peek().In('(', '#', '{'))
                        {
                                Group group = new Group(simpleText);
                                AutoAdd(currentGroup, propertyName, group);
                                groupStack.Push(group);
                                break;
                            }
                        }
                        AutoAdd(currentGroup, propertyName, simpleText);
                        break;
                    case '"': // Text
                        Text singleLineText = reader.ReadText(currentStep == Constructor);
                        AutoAdd(currentGroup, propertyName, singleLineText);
                        break;
                    default:
                        throw UnexpectedCharacter;
                }
                propertyName = null;
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
