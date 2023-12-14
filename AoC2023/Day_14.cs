namespace AdventOfCode.AoC2023
{
    public class Day_14: BaseDay
    {
        int width = 100;
        int height = 100;

        public override object Solve1()
        {
            var grid = DataLines.Select(x => x.ToCharArray()).ToArray();
            width = height = grid.Length;

            RollNorth(grid);

            return CalculateMass(grid);
        }

        public override object Solve2()
        {
            Dictionary<string, int> cache = new Dictionary<string, int>();

            cache.Add(Data, 0);

            var grid = DataLines.Select(x => x.ToCharArray()).ToArray();

            var iter = 0;

            while (true)
            {
                SpinCycle(grid);
                iter++;

                var str = ToString(grid);
                
                if (!cache.ContainsKey(str))
                    cache.Add(str, iter);
                else
                {
                    var patternStart = cache[str];
                    var repeatStart = iter;

                    var cycleLength = repeatStart - patternStart;

                    long totalIters = 1000000000;
                    totalIters -= patternStart; // remove number of cycles to get to start of repeats
                    var remainingIters = totalIters % cycleLength;

                    for (int i = 0; i < remainingIters; i++)
                        SpinCycle(grid);

                    return CalculateMass(grid);
                }
            }
        }

        public void RollNorth(char[][] input)
        {
            for (int col = 0; col < width; col++)
            {
                var lastImmovable = -1;

                for (int row = 0; row < height; row++)
                {
                    var c = input[row][col];

                    if (c == '#')
                        lastImmovable = row;

                    if (c == 'O')
                    {
                        input[row][col] = '.';
                        lastImmovable++;
                        input[lastImmovable][col] = 'O';
                    }
                }
            }
        }

        public void RollSouth(char[][] input)
        {
            for (int col = 0; col < width; col++)
            {
                var lastImmovable = -1;

                for (int row = 0; row < height; row++)
                {
                    var c = input[width - 1 - row][col];

                    if (c == '#')
                        lastImmovable = row;

                    if (c == 'O')
                    {
                        input[width - 1 - row][col] = '.';
                        lastImmovable++;
                        input[width - 1 - lastImmovable][col] = 'O';
                    }
                }
            }
        }

        public void RollWest(char[][] input)
        {
            for (int row = 0; row < width; row++)
            {
                var lastImmovable = -1;

                for (int col = 0; col < height; col++)
                {
                    var c = input[row][col];

                    if (c == '#')
                        lastImmovable = col;

                    if (c == 'O')
                    {
                        input[row][col] = '.';
                        lastImmovable++;
                        input[row][lastImmovable] = 'O';
                    }
                }
            }
        }

        public void RollEast(char[][] input)
        {
            for (int row = 0; row < width; row++)
            {
                var lastImmovable = -1;

                for (int col = 0; col < height; col++)
                {
                    var c = input[row][width - 1 - col];

                    if (c == '#')
                        lastImmovable = col;

                    if (c == 'O')
                    {
                        input[row][width - 1 - col] = '.';
                        lastImmovable++;
                        input[row][width - 1 - lastImmovable] = 'O';
                    }
                }
            }
        }

        public void SpinCycle(char[][] input)
        {
            RollNorth(input);
            RollWest(input);
            RollSouth(input);
            RollEast(input);
        }

        public long CalculateMass(char[][] input)
        {
            long total = 0;
            for (int i = 0; i < height; i++)
            {
                total += input[i].Count(x => x == 'O') * (width - i);
            }

            return total;
        }

        public string ToString(char[][] input)
        {
            return string.Join('\n', input.Select(x => new string(x)));
        }
    }
}
