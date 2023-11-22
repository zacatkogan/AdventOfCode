using AdventOfCode;
using AdventOfCode.Utils;

namespace AdventOfCode.AoC2021
{
    internal class Day_04 : BaseDay
    {
        public override object Solve1()
        {
            var splitData = Data.Split('\n');

            var bingoInput = splitData.First().ToString().Split(",").Select(int.Parse); // comma delimited string of inputs
            var boards = ParseData(splitData.Skip(2)).ToList(); // first 2 rows are bingo input, rest are boards.

            foreach (var bingoNumber in bingoInput)
            {
                boards.ForEach(b => b.CheckNumber(bingoNumber));

                // check for bingos
                var bingoBoard = boards.SingleOrDefault(x => x.CheckBingo());
                if (bingoBoard != null)
                {
                    var total = Enumerable.Range(0, 5).SelectMany(x => Enumerable.Range(0, 5), (y, z) => (y, z)).Select(x => bingoBoard.Field[x.y][x.z]).Where(x => x != -1).Sum();
                    return (total * bingoNumber);
                }
            }

            return "";
        }

        public override object Solve2()
        {
            var splitInput = Data.Split('\n');
            var bingoInput = splitInput[0].Split(",").Select(int.Parse); // comma delimited string of inputs
            var boards = ParseData(splitInput.Skip(2)).ToList(); // first 2 rows are bingo input, rest are boards.

            foreach (var bingoNumber in bingoInput)
            {
                boards.ForEach(b => b.CheckNumber(bingoNumber));

                if (boards.Count == 1 && boards.Single().CheckBingo())
                {
                    var losingBoard = boards.First();
                    var total = Enumerable.Range(0, 5).SelectMany(x => Enumerable.Range(0, 5), (y, z) => (y, z)).Select(x => losingBoard.Field[x.y][x.z]).Where(x => x != -1).Sum();//
                    return (total * bingoNumber);
                }

                // check for bingos
                var completedBoards = boards.Where(x => x.CheckBingo()).ToList();

                boards = boards.Except(completedBoards).ToList();
            }

            return "";
        }

        public IEnumerable<BingoBoard> ParseData(IEnumerable<string> input)
        {
            List<string> temp = input.ToList();

            while (true)
            {
                yield return new BingoBoard(temp.Take(5).Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)));

                temp = temp.Skip(5).ToList();
                if (temp.Count == 0)
                    yield break;
                temp = temp.Skip(1).ToList();
            }
        }

        public class BingoBoard
        {
            public int[][] Field;

            public BingoBoard(IEnumerable<IEnumerable<string>> input)
            {
                Field = input.Select(i => i.Select(int.Parse).ToArray()).ToArray();
            }

            public void CheckNumber(int input)
            {
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                    {
                        if (Field[i][j] == input)
                        {
                            Field[i][j] = -1;
                            return;
                        }
                    }
            }

            public bool CheckBingo()
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Field[i].Sum() == -5)
                        return true;
                }

                for (int i = 0; i < 5; i++)
                {
                    int ctr = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        ctr += Field[j][i];
                    }
                    if (ctr == -5)
                        return true;
                }

                return false;
            }
        }
    }
}

