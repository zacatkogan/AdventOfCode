namespace AdventOfCode
{
    public class Day_09 : BaseDay
    {
        Dictionary<string, (int x, int y)> Moves = new()
        {
            {"U",  (0, 1)},
            {"D", (0, -1)},
            {"L", (-1, 0)},
            {"R", (1, 0)}
        };

        string testData = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";

        public override ValueTask<string> Solve_1()
        {
            var moves = Data.Split("\n").Select(x => x.Split(" "))
                .Select(x => (dir: x[0], dist:int.Parse(x[1])))
                .ToList();

            var tailVisited = new HashSet<(int, int)>();

            var head = (x:0,y:0);
            var tail = (x:0,y:0);

            tailVisited.Add(tail);
            foreach (var move in moves)
            {
                for (int i = 0; i < move.dist; i++)
                {
                    var moveDir = Moves[move.dir];
                    head = (head.x + moveDir.x, head.y + moveDir.y);

                    var htDiff = (x:head.x - tail.x, y:head.y - tail.y);
                    // move in direction to minimize diff(X,Y)

                    var newTailX = 0;
                    var newTailY = 0;
                    var diagonal = false;

                    if (Math.Sqrt((float)(htDiff.x * htDiff.x + htDiff.y * htDiff.y)) > 2)
                        diagonal = true;

                    if (Math.Abs(htDiff.x) > 1 || diagonal)
                        newTailX = htDiff.x / Math.Abs(htDiff.x);

                    if (Math.Abs(htDiff.y) > 1 || diagonal)
                        newTailY = htDiff.y / Math.Abs(htDiff.y);

                    tail = (tail.x + newTailX, tail.y + newTailY);

                    tailVisited.Add(tail);
                }
            }
            return new(tailVisited.Count.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var visited = new HashSet<(int, int)>();

            var knots = Enumerable.Range(0,10).ToList(x => new pos(0,0));

            var moves = Data.Split("\n")
                .Select(x => x.Split(" "))
                .ToList(x => (dir: x[0], dist:int.Parse(x[1])));

            foreach (var move in moves)
            {
                for(int i = 0; i < move.dist; i++)
                {
                    var moveDir = Moves[move.dir];

                    // move head, then update all the knots
                    knots[0] = new pos(knots[0].x + moveDir.x, knots[0].y + moveDir.y);

                    for (int j = 1; j < knots.Count; j++)
                    {
                        knots[j] = MoveTail(knots[j-1], knots[j]);
                    }

                    var tail = knots.Last();
                    visited.Add((tail.x, tail.y));
                }
            }

            return new(visited.Count.ToString());
        }

        pos MoveTail(pos head, pos tail)
        {
            var htDiff = (x:head.x - tail.x, y:head.y - tail.y);

            var newTailX = 0;
            var newTailY = 0;
            var diagonal = false;

            if (Math.Abs(htDiff.x) + Math.Abs(htDiff.y) > 2)
                diagonal = true;

            if (Math.Abs(htDiff.x) > 1 || diagonal)
                newTailX = htDiff.x / Math.Abs(htDiff.x);

            if (Math.Abs(htDiff.y) > 1 || diagonal)
                newTailY = htDiff.y / Math.Abs(htDiff.y);

            tail = new pos(tail.x + newTailX, tail.y + newTailY);

            return tail;
        }


        class pos
        {
            public pos(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public int x;
            public int y;
        }

    }
}
