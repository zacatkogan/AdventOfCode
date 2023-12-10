using AdventOfCode;
using System.Text;

namespace AoC2018
{
    internal class Day05 : BaseDay
    {
        public override object Solve1()
        {
            var polymer = FullyReactPolymer(Data);
            return polymer.Length;
        }

        public override object Solve2()
        {
            var basePolymer = FullyReactPolymer(Data);

            int shortest = basePolymer.Length;

            for (char c = 'A'; c <= 'Z'; c++)
            {
                var cUpper = c.ToString();
                var cLower = char.ToLower(c).ToString();
                // remove all chars (upper and lower) of current c, then react until finished
                var fixedPolymer = basePolymer.Replace(cUpper, "").Replace(cLower, "");
                fixedPolymer = FullyReactPolymer(fixedPolymer);

                shortest = Math.Min(shortest, fixedPolymer.Length);
            }

            return shortest;
        }

        public string ReactPolymer(string polymer, out bool modified)
        {
            var sb = new StringBuilder();
            modified = false;
            int i = 0;
            for (; i < polymer.Length - 1; i++)
            {
                var c1 = polymer[i];
                var c2 = polymer[i + 1];
                // check if this char is the same as the next char, but in a different case
                if (c1 != c2 && char.ToLower(c1) == char.ToLower(c2))
                {
                    modified = true;
                    i++;
                    continue;
                }

                sb.Append(polymer[i]);
            }
            if (i != polymer.Length)
            {
                sb.Append(polymer[polymer.Length - 1]);
            }

            return sb.ToString();
        }

        public string FullyReactPolymer(string polymer)
        {
            bool modified;
            do
            {
                polymer = ReactPolymer(polymer, out modified);
            } while (modified);

            return polymer;
        }
    }
}
