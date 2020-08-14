using P371.ASDML.Types;
using Xunit;

namespace P371.ASDML.Tests
{
    public class LogicalTests
    {
        Logical l1 = true;
        Logical l2 = false;
        Logical l3 = false;

        [Fact]
        public void LogicalTest1()
        {
            Assert.NotEqual(l1, l2);
            Assert.NotEqual(l1, l3);
            Assert.Equal(l2, l3);
        }

        [Fact]
        public void LogicalTest2()
        {
            Assert.Equal<Logical>(true, l1);
            Assert.Equal<Logical>(false, l2);
            Assert.Equal<Logical>(false, l3);
        }

        [Fact]
        public void LogicalTest3()
        {
            Assert.NotEqual<Logical>(false, l1);
            Assert.NotEqual<Logical>(true, l2);
            Assert.NotEqual<Logical>(true, l3);
        }

        [Fact]
        public void LogicalTest4()
        {
            Assert.False(l1 == l2);
            Assert.False(l1 == l3);
            Assert.True(l2 == l3);
        }

        [Fact]
        public void LogicalTest5()
        {
            Assert.True(l1 != l2);
            Assert.True(l1 != l3);
            Assert.False(l2 != l3);
        }

        [Fact]
        public void LogicalTest6()
        {
            Assert.True(l1 == true);
            Assert.True(l2 == false);
            Assert.True(l3 == false);
        }

        [Fact]
        public void LogicalTest7()
        {
            Assert.False(l1 == false);
            Assert.False(l2 == true);
            Assert.False(l3 == true);
        }

        [Fact]
        public void LogicalTest8()
        {
            Assert.False(l1 != true);
            Assert.False(l2 != false);
            Assert.False(l3 != false);
        }

        [Fact]
        public void LogicalTest9()
        {
            Assert.True(l1 != false);
            Assert.True(l2 != true);
            Assert.True(l3 != true);
        }
    }
}
