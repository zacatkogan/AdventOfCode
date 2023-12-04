using AdventOfCode;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    internal class Day02 : BaseDay
    {
        public override object Solve1()
        {
            var c2 = 0;
            var c3 = 0;

            foreach (var l in DataLines)
            {
                var g = l.GroupBy(x => x);
                if (g.Any(x => x.Count() == 2))
                    c2 += 1;

                if (g.Any(x => x.Count() == 3))
                    c3 += 1;
            }

            return c2 * c3;
        }

        public override object Solve2()
        {
            var joined = from a in DataLines
            from b in DataLines
            where a != b
                && Enumerable.Zip(a, b).Count(x => x.First != x.Second) == 1
            select (a, b);

            return joined.Select(
                x => string.Join(
                    "", 
                    x.a.Zip(x.b)
                        .Where(c => c.First == c.Second)
                        .Select(c => c.First)))
                .First();
        }
    }
}
