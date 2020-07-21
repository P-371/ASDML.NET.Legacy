using P371.ASDML.Types;
using Xunit;

namespace P371.ASDML.Tests
{
    public class ObjectTests
    {
        [Fact]
        public void Test1()
        {
            Object asdmlNull = null;
            object csNull = null;

            Assert.Equal<Object>(asdmlNull, (Object)csNull);
            Assert.Equal<Object>(asdmlNull, (Object<Object>)csNull);
            Assert.Equal<Object>(asdmlNull, null);
            Assert.Equal<Object>(asdmlNull, asdmlNull);
        }
    }
}
