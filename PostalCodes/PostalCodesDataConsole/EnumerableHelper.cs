using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PostalCodesDataConsole
{
    public static class EnumerableHelper
    {
        /// <summary>
        /// Does an action for each element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="action">An action to apply to each element.</param>
        /// <returns>An <see cref="IEnumerable{TSource}"/> that contains the same elements as the input sequence.</returns>
        [DebuggerHidden]
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBySequentially<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            var queue = new Queue<TSource>();
            var currentKey = default(TKey);
            var comparer = EqualityComparer<TKey>.Default;

            foreach (var item in source)
            {
                var key = keySelector(item);

                if (!comparer.Equals(key, currentKey))
                {
                    if (queue.Count != 0)
                    {
                        yield return new Grouping<TKey, TSource>(currentKey, queue.ToArray());
                        queue.Clear();
                    }
                    currentKey = key;
                }
                queue.Enqueue(item);
            }

            if (queue.Count != 0)
            {
                yield return new Grouping<TKey, TSource>(currentKey, queue.ToArray());
                queue.Clear();
            }
        }
    }

    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public TKey Key { get; }
        protected IEnumerable<TElement> Values { get; }

        public Grouping(TKey key, IEnumerable<TElement> values)
        {
            Key = key;
            Values = values;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
