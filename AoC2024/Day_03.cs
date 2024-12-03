using AdventOfCode.Utils;
using System.Text.RegularExpressions;
namespace AdventOfCode.AoC2024
{
    public class Day_03 : BaseDay
    {       
        public override object Solve1()
        {
            var regex = new Regex(@"mul\(\d+,\d+\)");
            
            var total = 0;
            foreach (var line in DataLines)
            {
                var matches = regex.Matches(line);
                foreach (Match match in matches)
                {
                    var ints = match.Value.GetInts();
                    var a = ints[0];
                    var b = ints[1];
                    total += a * b;
                }
            }

            return total;
        }

        public override object Solve2()
        {
            var regex = new Regex(@"(do\(\)|don't\(\)|mul\(\d+,\d+\))");
            var active = true;
            var total = 0;

            foreach (var line in DataLines)
            {
                var matches = regex.Matches(line);
                foreach (Match match in matches)
                {
                    if (match.Value == "don't()")
                    {
                        active = false;
                    }
                    else if (match.Value == "do()")
                    {
                        active = true;
                    }
                    else if (active)
                    {
                        var ints = match.Value.GetInts();
                        var a = ints[0];
                        var b = ints[1];
                        total += a * b;
                    }

                }
            }
            return total;
        }

        public string testData = @"";
    }
}
