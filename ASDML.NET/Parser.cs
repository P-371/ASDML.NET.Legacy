using System.Collections.Generic;
using System.IO;
using System.Text;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using static P371.ASDML.GroupConstructionStep;
using InvalidOperationException = System.InvalidOperationException;

namespace P371.ASDML
{
    /// <summary>
    /// The ASDML Parser
    /// </summary>
    public class Parser
    {
        private StreamReader reader;

        internal UnexpectedCharacterException UnexpectedCharacter => reader.UnexpectedCharacter;

        internal Parser(StreamReader streamReader) => reader = streamReader;

        /// <summary>
        /// Creates a new ASDML Parser from the given stream
        /// </summary>
        /// <param name="stream">The stream to parse ASDML form</param>
        public Parser(Stream stream) : this(new StreamReader(stream)) { }

        /// <summary>
        /// Creates a new ASDML Parser from the given stream and encoding
        /// </summary>
        /// <param name="stream">The stream to parse ASDML form</param>
        /// <param name="encoding">The encoding of the stream</param>
        public Parser(Stream stream, Encoding encoding) : this(new StreamReader(stream, encoding)) { }

        /// <summary>
        /// Creates a new ASDML Parser from a <see cref="string" /> containing ASDML
        /// </summary>
        /// <param name="text">the <see cref="string" /> to parse ASDML from</param>
        public Parser(string text) : this(new StreamReader(new StringReader(text))) { }

        /// <summary>
        /// Creates a new ASDML Parser from the given file
        /// </summary>
        /// <param name="file">The file to parse ASDML from</param>
        public Parser(FileInfo file) : this(new StreamReader(File.OpenText(file.FullName))) { }

        /// <summary>
        /// Parses ASDML from the resource supplied at the constructor
        /// </summary>
        /// <returns>A group containing all root groups and properties</returns>
        public Group Parse()
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
                            Text text = reader.ReadText(true, currentStep == Constructor);
                            AutoAdd(currentGroup, propertyName, text);
                        }
                        else if (reader.Peek().In('t', 'f', 'n')) // Logical / @null
                        {
                            var (expectedText, expectedValue) = reader.Peek() switch
                            {
                                't' => ("true", (Logical)true),
                                'f' => ("false", (Logical)false),
                                'n' => ("null", null),
                                _ => throw new InvalidOperationException("This shouldn't have happened. The StreamReader peeked a different character.")
                            };
                            int i = 0;
                            while (!reader.EndOfStream && i < expectedText.Length && reader.Peek() == expectedText[i++])
                            {
                                reader.Read();
                            }
                            if (reader.EndOfStream && i != expectedText.Length)
                            {
                                throw new EndOfStreamException();
                            }
                            else if (!reader.EndOfStream && (!char.IsWhiteSpace(reader.Peek()) || (currentStep == Constructor && reader.Peek() != ')')))
                            {
                                throw UnexpectedCharacter;
                            }
                            AutoAdd(groupStack.Peek(), propertyName, expectedValue);
                        }
                        else
                        {
                            throw UnexpectedCharacter;
                        }
                        break;
                    case '#':
                        if (currentStep >= IDDone)
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // '#'
                        currentGroup.ID = reader.ReadSimpleText(currentStep == Constructor);
                        currentGroup.ConstructionStep = IDDone;
                        // TODO Add reference resolve
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
            return groupStack.Count == 1 ? groupStack.Pop() : throw new EndOfStreamException();
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
