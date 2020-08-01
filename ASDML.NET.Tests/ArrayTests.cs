using System.IO;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using Xunit;

namespace P371.ASDML.Tests
{
    public class ArrayTests
    {
        [Fact]
        public void ArrayTest1()
        {
            Parser parser = new Parser("[]");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.IsType<Array>(group.NestedObjects[0]);
            Assert.Empty(((Array)group.NestedObjects[0]).Value);
        }

        [Fact]
        public void ArrayTest2()
        {
            Parser parser = new Parser("[1 2 3]");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.IsType<Array>(group.NestedObjects[0]);
            Assert.Equal(3, ((Array)group.NestedObjects[0]).Value.Length);
        }

        [Fact]
        public void ArrayTest3()
        {
            Parser parser = new Parser(" [ 1 2 3 ] ");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.IsType<Array>(group.NestedObjects[0]);
            Assert.Equal(3, ((Array)group.NestedObjects[0]).Value.Length);
        }

        [Fact]
        public void ArrayTest4()
        {
            Parser parser = new Parser("[1 2 3");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }

        [Fact]
        public void ArrayTest5()
        {
            Parser parser = new Parser("[1 2 3)");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal(')', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(7, exception.Column);
        }

        [Fact]
        public void ArrayTest6()
        {
            Parser parser = new Parser("Text [1 2 3) { }");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal(')', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(12, exception.Column);
        }

        [Fact]
        public void ArrayTest7()
        {
            Parser parser = new Parser("[1 2 3}");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('}', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(7, exception.Column);
        }

        [Fact]
        public void ArrayTest8()
        {
            Parser parser = new Parser("Test[1 2 3]");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('[', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(5, exception.Column);
        }

        [Fact]
        public void ArrayTest9()
        {
            Parser parser = new Parser("[1 2 3]Test");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('T', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(8, exception.Column);
        }

        [Fact]
        public void ArrayTest10()
        {
            Parser parser = new Parser("[[1 2 3]]");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.IsType<Array>(group.NestedObjects[0]);
            Assert.Single(((Array)group.NestedObjects[0]).NestedObjects);
            Assert.IsType<Array>(((Array)group.NestedObjects[0]).NestedObjects[0]);
            Assert.Equal(3, ((Array)((Array)group.NestedObjects[0]).NestedObjects[0]).Value.Length);
        }

        [Fact]
        public void ArrayTest11()
        {
            Parser parser = new Parser("[[1 2 3][4 5 6]]");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('[', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(9, exception.Column);
        }

        [Fact]
        public void ArrayTest12()
        {
            Parser parser = new Parser("[[1 2 3] [ 4 5 6 7 ]]");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.IsType<Array>(group.NestedObjects[0]);
            Assert.Equal(2, (((Array)group.NestedObjects[0]).Value.Length));
            Assert.IsType<Array>(((Array)group.NestedObjects[0]).NestedObjects[0]);
            Assert.IsType<Array>(((Array)group.NestedObjects[0]).NestedObjects[1]);
            Assert.Equal(3, ((Array)((Array)group.NestedObjects[0]).NestedObjects[0]).Value.Length);
            Assert.Equal(4, ((Array)((Array)group.NestedObjects[0]).NestedObjects[1]).Value.Length);
        }
    }
}
