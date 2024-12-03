using AdventOfCode;
using AdventOfCode.Utils;
using System.Diagnostics.Metrics;

namespace AdventOfCode.AoC2024
{
    public class Day_01 : BaseDay
    {
        public override object Solve1()
        {
            var splitLines = DataLines.Select(RegexUtils.GetInts);

            var left = splitLines.Select(x => x[0]).ToList();
            var right = splitLines.Select(x => x[1]).ToList();

            left.Sort();
            right.Sort();

            return Enumerable.Zip(left, right, (l,r) => Math.Abs(l - r)).Sum();
        }

        public override object Solve2()
        {
            var splitLines = DataLines.Select(RegexUtils.GetInts);

            var left = splitLines.Select(x => x[0]).ToList();
            var right = splitLines.Select(x => x[1]).ToList();

            var counts = right.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            int sum = 0;
            left.ForEach(l =>
            {
                var occurrence = counts.GetValueOrDefault(l);
                sum += (l * occurrence);
            });

            return sum;
        }
    }
}
