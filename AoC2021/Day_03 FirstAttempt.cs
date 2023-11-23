using AdventOfCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2021
{
    internal class Day_03 : BaseDay
    {
        public override object Solve1()
        {
            var input = Data.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<int> counters = Enumerable.Repeat(0, 12).ToList();
            int ctr = 0;

            foreach (var row in input)
            {
                for (int i = 0; i < 12; i++)
                {
                    counters[i] += int.Parse(row[i].ToString());
                }
                ctr++;
            }

            return Convert.ToInt32(string.Join("", counters.Select(c => Math.Round(c / (double)ctr, 0)).Select(c => c.ToString())), 2);
        }

        public override object Solve2()
        {
            var input = Data.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var p2Input = input
                .Select(i => i.ToArray()
                    .Select(c => int.Parse(c.ToString())).ToArray())
                .ToList();

            var oxygenRating = p2Input.ToList();

            var o2 = 0;
            var co2 = 0;

            for (int i = 0; i < 12; i++)
            {
                int ctr2 = 0;
                var sum = oxygenRating.Sum(x => { ctr2++; return x[i]; });
                sum = (int)Math.Round(sum / (double)ctr2, 0, MidpointRounding.AwayFromZero);

                oxygenRating = oxygenRating.Where(x => x[i] == sum).ToList();
                if (oxygenRating.Count == 1)
                {
                    var oxrawRating = oxygenRating[0];
                    var oxbinaryRating = string.Join("", oxrawRating.Select(r => r.ToString()));
                    o2 = Convert.ToInt32(oxbinaryRating, 2);
                    break;
                }
            }

            var co2rating = p2Input.ToList();
            for (int i = 0; i < 12; i++)
            {
                int ctr2 = 0;
                var sum = co2rating.Sum(x => { ctr2++; return x[i]; });
                sum = (int)Math.Round(sum / (double)ctr2, 0, MidpointRounding.AwayFromZero);

                if (sum == 1) sum = 0;
                else sum = 1;

                co2rating = co2rating.Where(x => x[i] == sum).ToList();
                if (co2rating.Count == 1)
                {
                    var co2rawRating = co2rating[0];
                    var co2binaryRating = string.Join("", co2rawRating.Select(r => r.ToString()));
                    co2 = Convert.ToInt32(co2binaryRating, 2);
                }
            }

            return new { o2, co2 };
        }
    }
}