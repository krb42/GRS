using System;
using System.Collections.Generic;

namespace GRS_DBUP.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Immediately executes the given action on each element in the source sequence
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var element in source)
            {
                action(element);
            }
        }
    }
}
