namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using MathNet.Numerics;

    public class Day_11: BaseDay
    {
        public override object Solve1()
        {
            var emptyRows = DataLines
                .Select((row, index) => (row, index))
                .Where(x => x.row.All(x => x == '.'))
                .Select(x => x.index)
                .ToList();

            var emptyColumns = new List<int>();

            var arr = Data.To2dArray(x => x);
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                if (arr.Col(i).All(x => x == '.'))
                    emptyColumns.Add(i);
            }

            var stars = new List<Position>();

            foreach (var line in DataLines.Select((x, i) => (row:x, i)))
            {
                for (int x = 0; x < line.row.Length; x++)
                {
                    if (line.row[x] == '#')
                        stars.Add((x, line.i));
                }
            }

            var starPairs = new List<(Position, Position)>();

            for (int i = 0; i < stars.Count - 1; i++)
            {
                for (int j = i + 1; j < stars.Count; j++)
                {
                    starPairs.Add((stars[i], stars[j]));
                }
            }

            var distances = 0L;

            foreach (var p in starPairs)
            {
                distances += CalculateDistances(p.Item1, p.Item2, emptyRows, emptyColumns, 2);
            }

            return distances;
        }

        public override object Solve2()
        {
            var emptyRows = DataLines
                .Select((row, index) => (row, index))
                .Where(x => x.row.All(x => x == '.'))
                .Select(x => x.index)
                .ToList();

            var emptyColumns = new List<int>();

            var arr = Data.To2dArray(x => x);
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                if (arr.Col(i).All(x => x == '.'))
                    emptyColumns.Add(i);
            }

            var stars = new List<Position>();

            foreach (var line in DataLines.Select((x, i) => (row: x, i)))
            {
                for (int x = 0; x < line.row.Length; x++)
                {
                    if (line.row[x] == '#')
                        stars.Add((x, line.i));
                }
            }

            var starPairs = new List<(Position, Position)>();

            for (int i = 0; i < stars.Count - 1; i++)
            {
                for (int j = i + 1; j < stars.Count; j++)
                {
                    starPairs.Add((stars[i], stars[j]));
                }
            }

            var distances = 0L;

            foreach (var p in starPairs)
            {
                distances += CalculateDistances(p.Item1, p.Item2, emptyRows, emptyColumns, 1_000_000);
            }

            return distances;
        }

        public long CalculateDistances(Position star1, Position star2, List<int> emptyRows, List<int> emptyColumns, int gapMultiplier)
        {
            var minX = Math.Min(star1.X, star2.X);
            var maxX = Math.Max(star1.X, star2.X);

            var minY = Math.Min(star1.Y, star2.Y);
            var maxY = Math.Max(star1.Y, star2.Y);

            var numEmptyCols = emptyColumns.Count(col => col > minX && col < maxX);
            var numEmptyRows = emptyRows.Count(row => row > minY && row < maxY);

            return maxX - minX + maxY - minY +((numEmptyCols + numEmptyRows) * (gapMultiplier - 1));
        }
    }
}
