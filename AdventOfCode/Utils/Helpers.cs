using System.Linq;
namespace AdventOfCode
{
    public static class Helpers
    {
        public static IEnumerable<IEnumerable<T>> SplitOnElement<T>(this IEnumerable<T> sequence, T? element)
        {
            var comparer = EqualityComparer<T>.Default;

            var subSequence = new List<T>();
            foreach (var s in sequence)
            {
                if (comparer.Equals(s, element))
                {
                    yield return subSequence.AsEnumerable();
                    subSequence = new();
                }

                else
                    subSequence.Add(s);
            }

            yield return subSequence;
        }
    
        public static IOrderedEnumerable<T> OrderByDescending<T>(this IEnumerable<T> sequence)
        {
            return sequence.OrderByDescending(x => x);
        }

        public static IEnumerable<T> IntersectMany<T>(this IEnumerable<IEnumerable<T>> source)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if(!enumerator.MoveNext())
                    return Enumerable.Empty<T>();

                var ht = enumerator.Current.ToHashSet();

                while (enumerator.MoveNext())
                {
                    ht.IntersectWith(enumerator.Current);
                }   

                return ht;
            }
        }
    }
}
