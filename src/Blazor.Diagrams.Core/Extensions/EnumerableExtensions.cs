using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(int index, T element)> LoopWithIndex<T>(this IEnumerable<T> enumerable)
        {
            var i = 0;
            foreach (var element in enumerable)
            {
                yield return (i, element);
                i++;
            }
        }
    }
}
