using System.Linq;

namespace P371.ASDML
{
    internal static class Extensions
    {
        public static bool In<T>(this T @this, params T[] values)
            => values.Contains(@this);
    }
}
