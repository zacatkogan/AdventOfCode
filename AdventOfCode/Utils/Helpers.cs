using System.Numerics;

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
    
        public static List<TDest> ToList<TSrc, TDest>(this IEnumerable<TSrc> source, Func<TSrc, TDest> selector)
        {
            return source.Select(selector).ToList();
        }

        public static Queue<T> Enqueue<T>(this Queue<T> queue, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                queue.Enqueue(item);
            }

            return queue;
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            return new Queue<T>(source);
        }

        public static Queue<TOut> ToQueue<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TOut> selector)
        {
            return new Queue<TOut>(source.Select(selector));
        }

        public static IEnumerable<int> ToRange(this string str, string separator = "-")
        {
            var range = str.Split(separator).Select(int.Parse).ToList();
            return Enumerable.Range(range[0], range[1]-range[0] + 1);
        }

        public static T Dequeue<T>(this IList<T> list)
        {
            var value = list[0];
            list.RemoveAt(0);
            return value;
        }

        public static void Enqueue<T>(this IList<T> list, T value)
        {
            list.Add(value);
        }

        public static void Push<T>(this IList<T> list, T value)
        {
            list.Add(value);
        }

        public static T Pop<T>(this IList<T> list)
        {
            var value = list[list.Count-1];
            list.RemoveAt(list.Count-1);
            
            return value;
        }

        public static T Multiply<T>(this IEnumerable<T> sequence) where T: INumber<T>
        {
            return sequence.Aggregate(T.One, (current, next) => (T)current * (T)next);
        }

        public static T[,] To2dArray<T>(this string str, Func<char, T> selector)
        {
            var rows = str.Split("\n");
            var cols = rows[0].Length;

            var grid = new T[rows.Length,cols];
            
            for (int i = 0; i < rows.Length; i++)
            for (int j = 0; j < cols; j++ )
            {
                grid[i, j] = selector(rows[i][j]);
                var c = rows[i][j];
            }

            return (grid);
        }

        public static T[,] To2dArray<T>(this string str, Func<char, Position, T> selector)
        {
            var rows = str.Split("\n");
            var cols = rows[0].Length;

            var grid = new T[rows.Length,cols];
            
            for (int i = 0; i < rows.Length; i++)
            for (int j = 0; j < cols; j++ )
            {
                grid[i, j] = selector(rows[i][j], (i,j));
                var c = rows[i][j];
            }

            return (grid);
        }

        public static IEnumerable<T> Row<T>(this T[,] array, int row)
        {
            var cols = array.GetLength(1);
            for (int i = 0; i < cols; i++)
            {
                yield return array[row, i];
            }
        }

        public static IEnumerable<T> Col<T>(this T[,] array, int col)
        {
            var rows = array.GetLength(0);
            for (int i = 0; i < rows; i++)
            {
                yield return array[i, col];
            }
        }
    
        public static T Get<T>(this T[,] arr, Position p)
        {
            return arr[p.X, p.Y];
        }
    }
}
