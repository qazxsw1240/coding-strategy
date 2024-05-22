#nullable enable


using System.Collections.Generic;

namespace CodingStrategy.Utility
{
    public static class Enumerables
    {
        public static IEnumerable<(int, T)> ToIndexed<T>(this IEnumerable<T> enumerable)
        {
            int index = 0;
            foreach (T item in enumerable)
            {
                yield return (index++, item);
            }
        }
    }
}
