namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using MathNet.Numerics.Optimization;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection.Metadata;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class Day_03: BaseDay
    {
        public override object Solve1()
        {
            var neighbors =(
                from i in Enumerable.Range(-1, 3)
                from j in Enumerable.Range(-1, 3)
                where i != 0 && j != 0
                select new Position(i, j)
                )
                .ToList();

            var maxLength = DataLines[0].Length;

            var symbols = new List<char> { '!', '@', '#', '$', '%', '^', '&', '*', '+', '-', '=', '/' };
            //var numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9' }

            // find all the symbols:
            var symbolPositions = new List<Position>();

            for (int i = 0; i < DataLines.Length; i++)
            {
                var line = DataLines[i];

                for (int j = 0; j < maxLength; j++)
                {
                    if (symbols.Contains(line[j]))
                        symbolPositions.Add(new Position(i, j));
                }
            }

            var numbers = new List<(int number, int line, int start, int end)>();

            // find all the numbers
            for (int i = 0; i < DataLines.Length; i++)
            {
                var line = DataLines[i];

                var matches = Regex.Matches(line, @"\d+");
                foreach (Match match in matches)
                {
                    numbers.Add((int.Parse(match.Value), i, match.Index, match.Index + match.Length));
                }
            }

            var a = numbers[0];
            var b = symbolPositions[0];
            var result = IsAdjacentTo(b, a);

            var parts = numbers.Where(n => symbolPositions.Any(s => IsAdjacentTo(s, n))).ToList();

            return parts.Sum(x => x.number);
        }

        bool IsAdjacentTo(Position pos, (int number, int line, int start, int end) part)
        {
            var line = part.line; var start = part.start; var end = part.end;
            var v = part.number;

            if (pos.X == line && (pos.Y == end || pos.Y == start - 1))
                return true;
            
            if (pos.X - 1 == line || pos.X + 1 == line)
            {
                var possiblePos = (pos.Y - 1, pos.Y, pos.Y + 1);

                for (int i = start; i < end; i++)
                    if (i >= pos.Y - 1 && i <= pos.Y + 1)
                        return true;

                //if (pos.Y <= possibleRange.Item2 && pos.Y >= possibleRange.Item1 )
                //    return true;
            }

            return false;
        }

        public override object Solve2()
        {
            var maxLength = DataLines[0].Length;

            var symbols = new List<char> { '*' };
            //var numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9' }

            // find all the symbols:
            var symbolPositions = new List<Position>();

            for (int i = 0; i < DataLines.Length; i++)
            {
                var line = DataLines[i];

                for (int j = 0; j < maxLength; j++)
                {
                    if (symbols.Contains(line[j]))
                        symbolPositions.Add(new Position(i, j));
                }
            }

            var numbers = new List<(int number, int line, int start, int end)>();

            // find all the numbers
            for (int i = 0; i < DataLines.Length; i++)
            {
                var line = DataLines[i];

                var matches = Regex.Matches(line, @"\d+");
                foreach (Match match in matches)
                {
                    numbers.Add((int.Parse(match.Value), i, match.Index, match.Index + match.Length));
                }
            }

            var sum = 0;

            foreach (var s in symbolPositions)
            {
                var adjacentNos = numbers.Where(x => IsAdjacentTo(s, x)).ToList();
                if (adjacentNos.Count == 2)
                    sum += adjacentNos[0].number * adjacentNos[1].number;

            }

            return sum;
        }

        string TestData = "";
    }
}
