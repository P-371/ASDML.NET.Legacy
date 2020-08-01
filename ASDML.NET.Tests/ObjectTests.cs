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

            Assert.Equal<Object>((Object)csNull, asdmlNull);
            Assert.Equal<Object>((Object<Object>)csNull, asdmlNull);
            Assert.Null(asdmlNull);
            Assert.Equal<Object>(asdmlNull, asdmlNull);
        }
    }
}
