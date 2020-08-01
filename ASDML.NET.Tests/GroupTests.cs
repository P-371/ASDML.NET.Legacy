using System.IO;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using Xunit;

namespace P371.ASDML.Tests
{
    public class GroupTests
    {
        [Fact]
        public void GroupTest1()
        {
            Parser parser = new Parser("Group{}");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.IsType<Group>(group.NestedObjects[0]);
            group = (Group)group.NestedObjects[0];
            Assert.Empty(group.NestedObjects);
        }

        [Fact]
        public void GroupTest2()
        {
            Parser parser = new Parser(" Group  { }  ");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.IsType<Group>(group.NestedObjects[0]);
            group = (Group)group.NestedObjects[0];
            Assert.Empty(group.NestedObjects);
        }

        [Fact]
        public void GroupTest3()
        {
            Parser parser = new Parser("Group{Group{}}");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.IsType<Group>(group.NestedObjects[0]);
            group = (Group)group.NestedObjects[0];
            Assert.Single(group.NestedObjects);
            Assert.IsType<Group>(group.NestedObjects[0]);
            group = (Group)group.NestedObjects[0];
            Assert.Empty(group.NestedObjects);
        }

        [Fact]
        public void GroupTest4()
        {
            Parser parser = new Parser("Group{.Key Value}");
            Group group = parser.Parse();
            group = (Group)group.NestedObjects[0];
            Assert.Empty(group.NestedObjects);
            Assert.Equal((Text)"Value", group.Properties["Key"]);
        }

        [Fact]
        public void GroupTest5()
        {
            Parser parser = new Parser("Group { .Key1 Value1 .Key2 Value2 }");
            Group group = parser.Parse();
            group = (Group)group.NestedObjects[0];
            Assert.Empty(group.NestedObjects);
            Assert.Equal((Text)"Value1", group.Properties["Key1"]);
            Assert.Equal((Text)"Value2", group.Properties["Key2"]);
        }

        [Fact]
        public void GroupTest6()
        {
            Parser parser = new Parser("Group { .Key1 .Key2 Value2 }");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('.', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(15, exception.Column);
        }

        [Fact]
        public void GroupTest7()
        {
            Parser parser = new Parser("Group { .Key1 Value1 .Key2 }");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('}', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(28, exception.Column);
        }

        [Fact]
        public void GroupTest8()
        {
            Parser parser = new Parser("Group1 (Hello world)");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }

        [Fact]
        public void GroupTest9()
        {
            Parser parser = new Parser("Group1 (Hello world) ");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }

        [Fact]
        public void GroupTest10()
        {
            Parser parser = new Parser("Group1 (Hello world) Group2 { }");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('G', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(22, exception.Column);
        }

        [Fact]
        public void GroupTest11()
        {
            Parser parser = new Parser("[ Group (Hello world] { } )");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal(']', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(21, exception.Column);
        }

        [Fact]
        public void GroupTest12()
        {
            Parser parser = new Parser("Text [Hello world] { }");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('{', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(20, exception.Column);
        }

        [Fact]
        public void GroupTest13()
        {
            Parser parser = new Parser("Group { }Text");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('T', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(10, exception.Column);
        }

        [Fact]
        public void ConstructorTest1()
        {
            Parser parser = new Parser("Group(){}");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            group = (Group)group.NestedObjects[0];
            Assert.Empty(group.ConstructorParameters.NestedObjects);
        }

        [Fact]
        public void ConstructorTest2()
        {
            Parser parser = new Parser("Group ( ) { }");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            group = (Group)group.NestedObjects[0];
            Assert.Empty(group.ConstructorParameters.NestedObjects);
        }

        [Fact]
        public void ConstructorTest3()
        {
            Parser parser = new Parser("Group (1) { }");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            group = (Group)group.NestedObjects[0];
            Assert.Equal(1, group.ConstructorParameters.NestedObjects.Count);
        }

        [Fact]
        public void ConstructorTest4()
        {
            Parser parser = new Parser("Group ( 2 ) { }");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            group = (Group)group.NestedObjects[0];
            Assert.Equal(1, group.ConstructorParameters.NestedObjects.Count);
        }

        [Fact]
        public void ConstructorTest5()
        {
            Parser parser = new Parser("Group (3 4) { }");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            group = (Group)group.NestedObjects[0];
            Assert.Equal(2, group.ConstructorParameters.NestedObjects.Count);
        }

        [Fact]
        public void ConstructorTest6()
        {
            Parser parser = new Parser("Group ( 5 6 7 ) { }");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            group = (Group)group.NestedObjects[0];
            Assert.Equal(3, group.ConstructorParameters.NestedObjects.Count);
        }

        [Fact]
        public void ConstructorTest7()
        {
            Parser parser = new Parser("()");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('(', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(1, exception.Column);
        }

        [Fact]
        public void ConstructorTest8()
        {
            Parser parser = new Parser(" ( X Y ) ");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('(', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(2, exception.Column);
        }
    }
}
