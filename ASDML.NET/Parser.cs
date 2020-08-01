using System.Collections.Generic;
using System.IO;
using System.Text;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using P371.ASDML.Types.Helpers;
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

        private void EnsureGroupWritable(Group group)
        {
            if (group is null || group.ConstructionStep == Done) { }
            else
            {
                throw UnexpectedCharacter;
            }
        }

        /// <summary>
        /// Parses ASDML from the resource supplied at the constructor
        /// </summary>
        /// <returns>A group containing all root groups and properties</returns>
        public Group Parse()
        {
            Group root = Group.CreateRoot();
            Stack<IObjectCollection> groupStack = new Stack<IObjectCollection>();
            groupStack.Push(root);
            string propName = null;
            while (true)
            {
                Group currentGroup = groupStack.Peek() as Group;
                GroupConstructionStep currentStep = currentGroup?.ConstructionStep ?? GroupConstructionStep.Unknown;
                reader.SkipWhiteSpaces();
                if (reader.EndOfStream)
                {
                    if (currentStep != Done)
                    {
                        throw new EndOfStreamException();
                    }
                    break;
                }
                switch (reader.Peek())
                {
                    case '@':
                        EnsureGroupWritable(currentGroup);
                        reader.Read(); // '@'
                        if (reader.Peek() == '"') // Multiline text
                        {
                            Text text = reader.ReadText(true);
                            AutoAdd(groupStack.Peek(), propName, text);
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
                            else if (!reader.EndObject)
                            {
                                throw UnexpectedCharacter;
                            }
                            AutoAdd(groupStack.Peek(), propName, expectedValue);
                        }
                        else
                        {
                            throw UnexpectedCharacter;
                        }
                        break;
                    case '#':
                        // If currentGroup is null, (currentStep = Unknown) > IDDone
                        if (currentStep >= IDDone)
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // '#'
                        currentGroup.ID = reader.ReadSimpleText();
                        currentGroup.ConstructionStep = IDDone;
                        // TODO Add reference resolve
                        break;
                    case '.': // Property
                        if (propName != null || !(groupStack.Peek() is Group))
                        {
                            throw UnexpectedCharacter;
                        }
                        EnsureGroupWritable(currentGroup);
                        reader.Read(); // '.'
                        propName = reader.ReadSimpleText();
                        continue;
                    case '(':
                        // If currentGroup is null, (currentStep = Unknown) > Constructor
                        if (currentStep >= Constructor)
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // '('
                        currentGroup.ConstructionStep = Constructor;
                        groupStack.Push(currentGroup.ConstructorParameters);
                        break;
                    case ')':
                        if (groupStack.Count <= 1 || !(groupStack.Peek() is ConstructorParameters))
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // ')'
                        groupStack.Pop();
                        ((Group)groupStack.Peek()).ConstructionStep = ConstructorDone;
                        break;
                    case '[':
                        EnsureGroupWritable(currentGroup);
                        reader.Read(); // '['
                        Array array = new Array();
                        AutoAdd(groupStack.Peek(), propName, array);
                        groupStack.Push(array);
                        break;
                    case ']':
                        if (groupStack.Count <= 1 || !(groupStack.Peek() is Array))
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // ']'
                        if (!reader.EndObject)
                        {
                            throw UnexpectedCharacter;
                        }
                        groupStack.Pop();
                        break;
                    case '{':
                        if (currentGroup is null || currentStep == Done)
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // '{'
                        currentGroup.ConstructionStep = Done;
                        break;
                    case '}':
                        if (propName != null || groupStack.Count <= 1 || !(groupStack.Peek() is Group))
                        {
                            throw UnexpectedCharacter;
                        }
                        reader.Read(); // '}'
                        if (!reader.EndObject)
                        {
                            throw UnexpectedCharacter;
                        }
                        groupStack.Pop();
                        break;
                    case '+':
                    case '-':
                    case var digit when char.IsDigit(digit): // Number
                        EnsureGroupWritable(currentGroup);
                        Number number = reader.ReadNumber();
                        AutoAdd(groupStack.Peek(), propName, number);
                        break;
                    case '_':
                    case var letter when char.IsLetter(letter): // Simple text / group
                        EnsureGroupWritable(currentGroup);
                        SimpleText simpleText = reader.ReadSimpleText();
                        reader.SkipWhiteSpaces();
                        if (!reader.EndOfStream)
                        {
                            if (reader.Peek().In('(', '#', '{'))
                            {
                                Group group = new Group(simpleText);
                                AutoAdd(groupStack.Peek(), propName, group);
                                groupStack.Push(group);
                                break;
                            }
                        }
                        AutoAdd(groupStack.Peek(), propName, simpleText);
                        break;
                    case '"': // Text
                        EnsureGroupWritable(currentGroup);
                        Text singleLineText = reader.ReadText(currentStep == Constructor);
                        AutoAdd(groupStack.Peek(), propName, singleLineText);
                        break;
                    default:
                        throw UnexpectedCharacter;
                }
                propName = null;
            }
            return groupStack.Count == 1 ? root : throw new EndOfStreamException();
        }

        private void AutoAdd(IObjectCollection collection, string propertyName, Object value)
        {
            if (propertyName != null)
            {
                if (collection is Group group)
                {
                    group.Properties.Add(propertyName, value);
                }
                else
                {
                    throw new InvalidOperationException("This shouldn't have happened. Non-group collections can't have properties");
                }
            }
            else
            {
                collection.NestedObjects.Add(value);
            }
        }
    }
}