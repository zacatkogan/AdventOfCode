namespace AdventOfCode
{
    public class Day_09 : BaseDay
    {
        Dictionary<string, Position> Moves = new()
        {
            {"U",  (0, 1)},
            {"D", (0, -1)},
            {"L", (-1, 0)},
            {"R", (1, 0)}
        };

        public override ValueTask<string> Solve_1()
        {
            var moves = Data.Split("\n").Select(x => x.Split(" "))
                .Select(x => (dir: x[0], dist:int.Parse(x[1])))
                .ToList();

            var tailVisited = new HashSet<(int, int)>();

            Position head = (0,0);
            Position tail = (0,0);

            tailVisited.Add(tail);
            foreach (var move in moves)
            {
                for (int i = 0; i < move.dist; i++)
                {
                    head = head + Moves[move.dir];;

                    var htDiff = head - tail;
                    // move in direction to minimize diff(X,Y)

                    var newTailX = 0;
                    var newTailY = 0;
                    var diagonal = false;

                    if (Math.Sqrt((float)(htDiff.X * htDiff.X + htDiff.Y * htDiff.Y)) > 2)
                        diagonal = true;

                    if (Math.Abs(htDiff.X) > 1 || diagonal)
                        newTailX = htDiff.X / Math.Abs(htDiff.X);

                    if (Math.Abs(htDiff.Y) > 1 || diagonal)
                        newTailY = htDiff.Y / Math.Abs(htDiff.Y);

                    tail = (tail.X + newTailX, tail.Y + newTailY);

                    tailVisited.Add(tail);
                }
            }
            return new(tailVisited.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var visited = new HashSet<(int, int)>();

            var knots = Enumerable.Range(0,10).ToList(x => new Position(0,0));

            var moves = Data.Split("\n")
                .Select(x => x.Split(" "))
                .ToList(x => (dir: x[0], dist:int.Parse(x[1])));

            foreach (var move in moves)
            {
                for(int i = 0; i < move.dist; i++)
                {
                    // move head, then update all the knots
                    knots[0] = knots[0] + Moves[move.dir];

                    for (int j = 1; j < knots.Count; j++)
                    {
                        knots[j] = MoveTail(knots[j-1], knots[j]);
                    }

                    var tail = knots.Last();
                    visited.Add(tail);
                }
            }

            return new(visited.Count.ToString());
        }

        Position MoveTail(Position head, Position tail)
        {
            var htDiff = head - tail;

            var tailMoveX = 0;
            var tailMoveY = 0;
            var diagonal = false;

            if (Math.Abs(htDiff.X) + Math.Abs(htDiff.Y) > 2)
                diagonal = true;

            if (Math.Abs(htDiff.X) > 1 || diagonal)
                tailMoveX = htDiff.X / Math.Abs(htDiff.X);

            if (Math.Abs(htDiff.Y) > 1 || diagonal)
                tailMoveY = htDiff.Y / Math.Abs(htDiff.Y);

            tail = tail + (tailMoveX, tailMoveY);

            return tail;
        }

    }
}
