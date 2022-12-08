namespace AdventOfCode
{
    public class Day_08 : BaseDay
    {
        string testData = @"30373
25512
65332
33549
35390";

        int[,] ParsedData => ParsedDataLazy.Value;
        Lazy<int[,]> ParsedDataLazy => new Lazy<int[,]>(GetParsedData);

        int[,] GetParsedData()
        {
            var rows = Data.Split("\n");
            var cols = rows[0].Length;

            var grid = new int[rows.Length,cols];
            
            for (int i = 0; i < rows.Length; i++)
            for (int j = 0; j < cols; j++ )
                grid[i,j] = int.Parse(rows[i][j].ToString());

            return grid;
        }
        public override ValueTask<string> Solve_1()
        {
            var trees = GetParsedData();
            var rows = trees.GetLength(0);
            var cols = trees.GetLength(1);

            var visibleTrees = 0;

            // top down, bottom up
            for(int col = 0; col < cols; col++)
            for(int row = 0; row < rows; row++)
            {
                if (IsVisible(row, col))
                    visibleTrees++;
            }
            
            return new(visibleTrees.ToString());
        }

        public bool IsVisible(int row, int col)
        {
            var trees = ParsedData;
            var currentTree = trees[row, col];
            
            var rows = ParsedData.GetLength(0);
            var cols = ParsedData.GetLength(1);

            var up = Enumerable.Range(0, row).All(x=> currentTree > trees[x, col]);
            var down = Enumerable.Range(0, col).All(x => currentTree > trees[row, x]);
            var left = Enumerable.Range(0, rows).Where(x => x > row).All(x => currentTree > trees[x, col]);
            var right = Enumerable.Range(0, cols).Where(x => x > col).All(x => currentTree > trees[row, x]);

            return up | down | left | right;
        }

        public int ScenicScore(int row, int col)
        {
            var trees = ParsedData;
            var currentTree = trees[row, col];

            var rows = ParsedData.GetLength(0);
            var cols = ParsedData.GetLength(1);

            var x1 = 0;
            foreach (var r in Enumerable.Range(0, row).Reverse())
            {
                x1 += 1;
                if (trees[r, col] >= currentTree)
                    break;
            }

            var x2 = 0;
            foreach (var c in Enumerable.Range(0, col).Reverse())
            {
                x2 += 1;
                if (trees[row, c] >= currentTree)
                    break;
            }

            var x3 = 0;
            foreach (var r in Enumerable.Range(0, rows).Where(x => x > row))
            {
                x3 += 1;
                if (trees[r, col] >= currentTree)
                    break;
            }
            
            var x4 = 0;
            foreach (var c in Enumerable.Range(0, cols).Where(x => x > col))
            {
                x4 += 1;
                if (trees[row, c] >= currentTree)
                    break;
            }            
            return x1 * x2 * x3 * x4;
        }

        public override ValueTask<string> Solve_2()
        {   
            var trees = GetParsedData();
            var rows = trees.GetLength(0);
            var cols = trees.GetLength(1);

            var maxScore = 0;

            for(int col = 1; col < cols-1; col++)
            {
                for(int row = 1; row < rows-1; row++)
                {
                    var score = ScenicScore(row, col);
                    if (score > maxScore)
                        maxScore = score;
                }
            }
            
            return new(maxScore.ToString());
        }

    }
}
