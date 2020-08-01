using System.IO;
using P371.ASDML.Exceptions;
using P371.ASDML.Types;
using Xunit;

namespace P371.ASDML.Tests
{
    public class NumberTests
    {
        Number n1 = 2;
        Number n2 = 7;
        Number n3 = 7;

        [Fact]
        public void Test1()
        {
            Assert.NotEqual(n1, n2);
            Assert.NotEqual(n1, n3);
            Assert.Equal(n2, n3);
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal<Number>(2, n1);
            Assert.Equal<Number>(7, n2);
            Assert.Equal<Number>(7, n3);
        }

        [Fact]
        public void Test3()
        {
            Assert.NotEqual<Number>(7, n1);
            Assert.NotEqual<Number>(2, n2);
            Assert.NotEqual<Number>(5, n3);
        }

        [Fact]
        public void Test4()
        {
            Assert.False(n1 == n2);
            Assert.False(n1 == n3);
            Assert.True(n2 == n3);
        }

        [Fact]
        public void Test5()
        {
            Assert.True(n1 != n2);
            Assert.True(n1 != n3);
            Assert.False(n2 != n3);
        }

        [Fact]
        public void Test6()
        {
            Assert.True(n1 == 2);
            Assert.False(n1 == 7);
            Assert.False(n1 != 2);
            Assert.True(n1 != 7);
        }

        [Fact]
        public void Test7()
        {
            Assert.True(n1 == 2.0);
            Assert.False(n1 == 7.0);
            Assert.False(n1 != 2.0);
            Assert.True(n1 != 7.0);
        }

        [Fact]
        public void Test8()
        {
            Assert.True(n1 == 2.0f);
            Assert.False(n1 == 7.0f);
            Assert.False(n1 != 2.0f);
            Assert.True(n1 != 7.0f);
        }

        [Fact]
        public void Test9()
        {
            Assert.True(n1 <= 2.0);
            Assert.True(n1 >= 2.0);
            Assert.False(n1 < 2.0);
            Assert.False(n1 > 2.0);
        }

        [Fact]
        public void Test10()
        {
            Assert.True(n1 <= 7.0);
            Assert.False(n1 >= 7.0);
            Assert.True(n1 < 7.0);
            Assert.False(n1 > 7.0);
        }

        [Fact]
        public void Test11()
        {
            Assert.False(n1 <= 0.0);
            Assert.True(n1 >= 0.0);
            Assert.False(n1 < 0.0);
            Assert.True(n1 > 0.0);
        }

        [Fact]
        public void Test12()
        {
#pragma warning disable CS1718
            Assert.False(n1 < n1);
            Assert.True(n1 <= n1);
            Assert.True(n1 == n1);
            Assert.False(n1 != n1);
            Assert.True(n1 >= n1);
            Assert.False(n1 > n1);
#pragma warning restore CS1718
        }

        [Fact]
        public void Test13()
        {
            Parser parser = new Parser("7");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((Number)7, group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void Test14()
        {
            Parser parser = new Parser("-2");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((Number)(-2), group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void Test15()
        {
            Parser parser = new Parser("2.71");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((Number)2.71, group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void Test16()
        {
            Parser parser = new Parser("-3.14");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((Number)(-3.14), group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void Test17()
        {
            Parser parser = new Parser("6.67E-11");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((Number)double.Parse("6.67E-11"), group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void Test18()
        {
            Parser parser = new Parser("+6E+23");
            Group group = parser.Parse();
            Assert.Single(group.NestedObjects);
            Assert.Equal((Number)double.Parse("6E+23"), group.NestedObjects[0]);
            Assert.Empty(group.Properties);
            Assert.Empty(group.ConstructorParameters.NestedObjects);
            Assert.Null(group.ID);
        }

        [Fact]
        public void Test19()
        {
            Parser parser = new Parser("+-6.67E-11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('-', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(2, exception.Column);
        }

        [Fact]
        public void Test20()
        {
            Parser parser = new Parser("++6.67E-11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('+', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(2, exception.Column);
        }

        [Fact]
        public void Test21()
        {
            Parser parser = new Parser("-+6.67E-11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('+', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(2, exception.Column);
        }
        
        [Fact]
        public void Test22()
        {
            Parser parser = new Parser("--6.67E-11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('-', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(2, exception.Column);
        }

        [Fact]
        public void Test23()
        {
            Parser parser = new Parser("6.E-11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('E', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(3, exception.Column);
        }

        [Fact]
        public void Test24()
        {
            Parser parser = new Parser("6.67E11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('1', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(6, exception.Column);
        }

        [Fact]
        public void Test25()
        {
            Parser parser = new Parser("6.");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }

        [Fact]
        public void Test26()
        {
            Parser parser = new Parser("6.67E+");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }        

        [Fact]
        public void Test27()
        {
            Parser parser = new Parser("6.67E-");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }

        [Fact]
        public void Test28()
        {
            Parser parser = new Parser("6.67E");
            Assert.Throws<EndOfStreamException>(parser.Parse);
        }

        [Fact]
        public void Test29()
        {
            Parser parser = new Parser("6.67E--11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('-', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(7, exception.Column);
        }

        [Fact]
        public void Test30()
        {
            Parser parser = new Parser("6.67E-+11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('+', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(7, exception.Column);
        }

        [Fact]
        public void Test31()
        {
            Parser parser = new Parser("6.67E+-11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('-', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(7, exception.Column);
        }

        [Fact]
        public void Test32()
        {
            Parser parser = new Parser("6.67E++11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('+', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(7, exception.Column);
        }

        [Fact]
        public void Test33()
        {
            Parser parser = new Parser("-.67E-11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('.', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(2, exception.Column);
        }

        [Fact]
        public void Test34()
        {
            Parser parser = new Parser("6.67X-11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('X', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(5, exception.Column);
        }

        [Fact]
        public void Test35()
        {
            Parser parser = new Parser("6.67.11");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('.', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(5, exception.Column);
        }

        [Fact]
        public void Test36()
        {
            Parser parser = new Parser("6. ");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal(' ', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(3, exception.Column);
        }

        [Fact]
        public void Test37()
        {
            Parser parser = new Parser("6.x");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('x', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(3, exception.Column);
        }

        [Fact]
        public void Test38()
        {
            Parser parser = new Parser("6.67E-11X");
            UnexpectedCharacterException exception = Assert.Throws<UnexpectedCharacterException>(parser.Parse);
            Assert.Equal('X', exception.Character);
            Assert.Equal(1, exception.Line);
            Assert.Equal(9, exception.Column);
        }
    }
}
