using AdventOfCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2020
{
    internal class Day6 : BaseDay
    {
        internal static readonly char[] separator = ['\r', '\n'];

        IEnumerable<IEnumerable<string>> ArraySplit(string[] input)
        {
            var start = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(input[i]))
                {
                    yield return input.AsSpan(start, i - start).ToArray();
                    start = i + 1;
                }
            }

            yield return input.AsSpan(start, input.Length - start).ToArray();
        }

        public override object Solve1()
        {
            return ArraySplit(Data.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                .Select(i => string.Join("", i).ToCharArray().Distinct().Count())
                .Sum();
        }

        public override object Solve2()
        {
            return ArraySplit(Data.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                .Select(i => new {
                    countOfPeople = i.Count(),
                    countOfInputs = string.Join("", i).ToCharArray()
                        .GroupBy(s => s)    // group by input
                        .Select(s => new
                        {
                            input = s.Key,
                            count = s.Count()
                        })
                    .Where(x => x.count == i.Count())

                })
                .Sum(x => x.countOfInputs.Count());
        }
    }
}
