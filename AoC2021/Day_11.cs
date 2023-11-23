using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.AoC2021
{
    public class Day_11 : BaseDay
    {
        public List<(int x, int y)> directions =
                (from x in Enumerable.Range(-1, 3)
                 from y in Enumerable.Range(-1, 3)
                 select (x, y))
                .ToList();

        public override object Solve1()
        {
            int[][] data = DataLines.Select(x => x.ToCharArray()
                .Select(x => int.Parse(x.ToString())).ToArray()).ToArray();

            checked
            {
                return Enumerable.Range(0, 100)
                    .Select(e => Step(data))
                    .Sum();
            }
        }

        public override object Solve2()
        {
            int[][] data = DataLines.Select(x => x.ToCharArray()
                .Select(x => int.Parse(x.ToString())).ToArray()).ToArray();

            var step = 0;
            while (true)
            {
                step++;

                if (Step(data) == 100)
                {
                    return step;
                }
            }
        }

        int Step(int[][] data)
        {
            // jellyfish that need to get recalculated because a neighboring jellyfish flashed
            var toVisit = new Stack<(int x, int y)>();

            // jellyfish that already flashed and can't get flashed again
            var flashed = new HashSet<(int x, int y)>();

            // increment all jellyfish, and look for all the jellyfish that seed the flashing
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    var temp = ++data[i][j];
                    if (temp == 10)
                    {
                        directions.ForEach(d => toVisit.Push((i + d.x, j + d.y)));
                        flashed.Add((i, j));
                    }
                }

            while (toVisit.TryPop(out (int x, int y) result))
            {
                if (flashed.Contains(result))
                    continue;

                //// index out of bounds
                //if (result.x < 0 || result.y < 0 || result.x > 9 || result.y > 9)
                //	continue;

                try
                {
                    var temp = ++data[result.x][result.y];

                    if (temp == 10)
                    {
                        directions.ForEach(d => toVisit.Push((result.x + d.x, result.y + d.y)));
                        flashed.Add((result.x, result.y));
                    }
                }
                catch (IndexOutOfRangeException e) { } // cbf doing range checking
            }

            // reset the jellyfish that flashed to 0
            foreach (var result in flashed)
            {
                data[result.x][result.y] = 0;
            }

            return flashed.Count;
        }
    }
}