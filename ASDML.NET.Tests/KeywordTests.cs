using System.IO;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using Xunit;

namespace P371.ASDML.Tests
{
    public class KeywordTests
    {
        [Fact]
        public void NullTest1()
        {
            Parser parser = new Parser("@nullx");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('x', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(6, exception.Column);
        }

        [Fact]
        public void NullTest2()
        {
            Parser parser = new Parser("@nil");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('i', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(3, exception.Column);
        }

        [Fact]
        public void NullTest3()
        {
            Parser parser = new Parser("");
            Group group = parser.Parse();
            Assert.Empty(group.NestedObjects);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void NullTest4()
        {
            Parser parser = new Parser(" ");
            Group group = parser.Parse();
            Assert.Empty(group.NestedObjects);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void NullTest5()
        {
            Parser parser = new Parser("\n");
            Group group = parser.Parse();
            Assert.Empty(group.NestedObjects);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void NullTest6()
        {
            Parser parser = new Parser("@null");
            Group group = parser.Parse();

            Assert.Single(group.NestedObjects);
            Assert.Null(group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void FalseTest1()
        {
            Parser parser = new Parser("@fals");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }

        [Fact]
        public void FalseTest2()
        {
            Parser parser = new Parser("@false");
            Group group = parser.Parse();

            Assert.Single(group.NestedObjects);
            Assert.Equal((Logical)false, group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void TrueTest1()
        {
            Parser parser = new Parser("@true");
            Group group = parser.Parse();

            Assert.Single(group.NestedObjects);
            Assert.Equal((Logical)true, group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }
    }
}
