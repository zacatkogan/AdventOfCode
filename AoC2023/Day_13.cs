namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using System.Reflection;

    public class Day_13: BaseDay
    {
        public override object Solve1()
        {
            return EnumerateBoards()
                .Select(x => FindReflectionPlaneWithSmudge(x, 0))
                .Sum(x => x.hor * 100 + x.vert);
        }

        public override object Solve2()
        {
            return EnumerateBoards()
                .Select(x => FindReflectionPlaneWithSmudge(x, 1))
                .Sum(x => x.hor * 100 + x.vert);
        }

        IEnumerable<string[]> EnumerateBoards()
        {
            var cleanedData = Data.Replace("\r", "");
            var alteredData = cleanedData.Replace("\n\n", "|");

            return alteredData.Split('|').Select(x => x.Split("\n"));
        }

        int FindReflectionPlaneHorizontalWithSmudge(string[] board, int errors)
        {
            // split the board in half from side to side, then compare
            // e.g.:
            // ###
            // ---
            // ###
            for (int i = 1; i < board.Length; i++)
            {
                var start = board[..i];
                Array.Reverse(start);

                var end = board[i..];

                if (start.Length == 0 || end.Length == 0)
                    continue;

                // find the number of differences
                var diffs = 0;
                foreach (var rowPair in Enumerable.Zip(start, end))
                {
                    (var s1, var s2) = rowPair;

                    var thisLineDiffs = Enumerable.Zip(s1, s2).Count(x => x.First != x.Second);
                    diffs += thisLineDiffs;

                    if (thisLineDiffs > errors)
                        break;
                }

                if (diffs == errors)
                    return i;
            }

            return 0;
        }

        int FindReflectionPlaneVerticalWithSmudge(string[] board, int errors)
        {
            var arr = String.Join("\n", board).To2dArray(x => x);
            var rowLength = arr.GetLength(1);

            var cols = new List<string>();

            for (int i = 0; i < rowLength; i++)
            {
                cols.Add(new string(arr.Col(i).ToArray()));
            }

            for (int i = 1; i < cols.Count; i++)
            {
                var start = cols[..i];
                start.Reverse();

                var end = cols[i..];

                if (start.Count == 0 || end.Count == 0)
                    continue;

                // find the number of differences in each paired line.
                // if the total number of differences is equal to `errors`, this is our target arrangement
                // anything else is wrong.
                var diffs = 0;
                foreach (var rowPair in Enumerable.Zip(start, end))
                {
                    (var s1, var s2) = rowPair;

                    var thisLineDiffs = Enumerable.Zip(s1, s2).Count(x => x.First != x.Second);
                    diffs += thisLineDiffs;

                    if (thisLineDiffs > errors)
                        break;
                }

                if (diffs == errors)
                    return i;
            }

            return 0;
        }

        (int hor, int vert) FindReflectionPlaneWithSmudge(string[] board, int errors)
        {
            var hor = FindReflectionPlaneHorizontalWithSmudge(board, errors);
            var vert = FindReflectionPlaneVerticalWithSmudge(board, errors);

            if ((hor == 0 && vert == 0) || (hor != 0 && vert != 0))
                ;

            return (hor, vert);
        }        
    }
}
