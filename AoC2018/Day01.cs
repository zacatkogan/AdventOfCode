using AdventOfCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    internal class Day01 : BaseDay
    {
        public override object Solve1()
        {
            return DataLines.Select(x => int.Parse(x)).Sum();
        }

        public override object Solve2()
        {
            var ints = DataLines.Select(int.Parse).ToList();
            var freqs = new HashSet<int>();
            var f = 0;
            freqs.Add(0);

            while (true)
            {
                foreach (var i in ints)
                {
                    f += i;

                    if (!freqs.Add(f))
                        return f;
                }
            }
        }
    }
}
