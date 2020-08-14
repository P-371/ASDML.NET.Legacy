using System.Collections.Generic;
using System.Linq;

namespace P371.ASDML
{
    internal static class Extensions
    {
        public static bool In<T>(this T @this, IEnumerable<T> values)
            => values.Contains(@this);
    }
}
