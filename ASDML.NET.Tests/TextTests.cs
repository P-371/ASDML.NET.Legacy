using System.IO;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using Xunit;

namespace P371.ASDML.Tests
{
    public class TextTests
    {
        const string s1 = "Text 1";
        const string s2 = "Text 2";
        Text t1 = s1;
        Text t2 = s2;
        Text t3 = s2;

        [Fact]
        public void TextTest1()
        {
            Assert.NotEqual(t1, t2);
            Assert.NotEqual(t1, t3);
            Assert.Equal(t2, t3);
        }

        [Fact]
        public void TextTest2()
        {
            Assert.Equal<Text>(t1, s1);
            Assert.Equal<Text>(t2, s2);
            Assert.Equal<Text>(t3, s2);
        }

        [Fact]
        public void TextTest3()
        {
            Assert.NotEqual<Text>("Test 1", t1);
            Assert.NotEqual<Text>("Test 2", t2);
            Assert.NotEqual<Text>("Test 3", t3);
        }

        [Fact]
        public void TextTest4()
        {
            Assert.False(t1 == t2);
            Assert.False(t1 == t3);
            Assert.True(t2 == t3);
        }

        [Fact]
        public void TextTest5()
        {
            Assert.True(t1 != t2);
            Assert.True(t1 != t3);
            Assert.False(t2 != t3);
        }

        [Fact]
        public void TextTest6()
        {
            Assert.True(t1 == s1);
            Assert.True(t2 == s2);
            Assert.True(t3 == s2);
        }

        [Fact]
        public void TextTest7()
        {
            Assert.False(t1 == "Test 4");
            Assert.False(t2 == "Test 5");
            Assert.False(t3 == "Test 6");
        }

        [Fact]
        public void TextTest8()
        {
            Assert.False(t1 != s1);
            Assert.False(t2 != s2);
            Assert.False(t3 != s2);
        }

        [Fact]
        public void TextTest9()
        {
            Assert.True(t1 != "Test 7");
            Assert.True(t2 != "Test 8");
            Assert.True(t3 != "Test 9");
        }

        [Fact]
        public void TextTest10()
        {
            Parser parser = new Parser("\"This is a text\"");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((Text)"This is a text", group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void TextTest11()
        {
            Parser parser = new Parser("\"This is a text");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }

        [Fact]
        public void TextTest12()
        {
            Parser parser = new Parser("ASDML \"");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }

        [Fact]
        public void TextTest13()
        {
            Parser parser = new Parser("\"This is a\"text");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('t', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(12, exception.Column);
        }

        [Fact]
        public void SimpleTest1()
        {
            Parser parser = new Parser("Hello");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((SimpleText)"Hello", group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void SimpleTest2()
        {
            Parser parser = new Parser("x42@+.#-");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('@', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(4, exception.Column);
        }

        [Fact]
        public void SimpleTest3()
        {
            Parser parser = new Parser("_");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((SimpleText)"_", group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void SimpleTest4()
        {
            Parser parser = new Parser("ASDML\"");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('"', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(6, exception.Column);
        }

        [Fact]
        public void SimpleTest5()
        {
            Parser parser = new Parser("ASDML\"asdml");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('"', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(6, exception.Column);
        }

        [Fact]
        public void MultilineTest1()
        {
            Parser parser = new Parser(@"@""Multiline
text""");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((MultiLineText)"Multiline\ntext", group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void MultilineTest2()
        {
            Parser parser = new Parser(@"@""Multiline
text");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }
    }
}
