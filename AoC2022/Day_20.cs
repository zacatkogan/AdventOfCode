using System.Collections.Generic;
using static System.Console;

namespace AdventOfCode
{
    public class Day_20 : BaseDay
    {
        public class Element
        {
            public long Value;
            public override string ToString()
            {
                return "{ " + Value.ToString() + " }";
            }
        }

        public static int Mod(long x, int m)
        {
            int r = (int)(x%m);
            return r<0 ? r+m : r;
        }

        public override object Solve1()
        {
            var data = ParseData(Data).Select(x => new Element() { Value = x }).ToList();
            var results = Decrypt(data, data).ToList();

            var zeroIndex = results.FindIndex(0, x => x.Value == 0);

            return new[] {1000, 2000, 3000}
                .Select(x => results[(zeroIndex + x) % results.Count].Value)
                .Sum();
        }

        public override object Solve2()
        {
            var key = 811589153L;
            var sortKey = ParseData(Data).Select(x => new Element{ Value = x * key }).ToList();

            List<Element> results = sortKey.ToList();

            foreach (var _ in Enumerable.Range(0, 10))
            {
                results = Decrypt(sortKey, results).ToList();
            }

            var zeroIndex = results.FindIndex(0, x => x.Value == 0);
            return new[] {1000, 2000, 3000}
                .Select(x => results[(zeroIndex + x) % results.Count].Value)
                .Sum();
        }

        public IEnumerable<int> ParseData(string data)
        {
            return data.Split("\n").Select(x => int.Parse(x));
        }

        public IEnumerable<Element> Decrypt(IEnumerable<Element> orderKey, IEnumerable<Element> raw)
        {
            var elements = raw.ToList();
            var elementCount = elements.Count - 1;

            foreach (var element in orderKey)
            {
                if (element.Value == 0)
                    continue;
                
                var startingIndex = elements.IndexOf(element);
                elements.RemoveAt(startingIndex);

                //Console.WriteLine($"Considering value {element.Value} at index {startingIndex}");

                var endingIndex = Mod(startingIndex + element.Value, elements.Count);

                //Console.WriteLine($"  Moving to index {startingIndex + element.Value} ({endingIndex})");
                elements.Insert(endingIndex, element);
                //Console.WriteLine($"    Element at new index {endingIndex} : {elements[endingIndex].Value}");
            }

            return elements;

            // var ZeroIndex = elements.FindIndex(0, x => x.Value == 0);
            // var firstValue = elements.ElementAt(Mod(ZeroIndex + 1000, elements.Count));
            // var secondValue = elements.ElementAt(Mod(ZeroIndex + 2000, elements.Count));
            // var thirdValue = elements.ElementAt(Mod(ZeroIndex + 3000, elements.Count));

            // return firstValue.Value + secondValue.Value + thirdValue.Value;
        }

        public string testData = @"1
2
-3
3
-2
0
4";
    }
}
