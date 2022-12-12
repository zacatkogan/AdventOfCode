namespace AdventOfCode
{
    public class Day_12 : BaseDay
    {
        public override object Solve1()
        {
            (var map, var start, var end) = GetParsedData();

            return GetLowestSteps(map, start, end);
        }

        public override object Solve2()
        {
            (var map, var starts, var end) = GetParsedData_part2();

            return starts.Select(x => GetLowestSteps(map, x, end)).OrderBy(x => x).First();
        }

        public int GetLowestSteps(int[,] map, Position start, Position end)
        {
            var Neighbors = Position.Directions.ToList(x => x.Value);

            var yMax = map.GetLength(1);
            var xMax = map.GetLength(0);

            var q = new PriorityQueue<Position, int>();
            q.Enqueue(start, 0);

            var visited = new Dictionary<Position, int>();

            while (q.TryDequeue(out Position? position, out int steps))
            {
                if (position == end)
                {
                    return steps;
                }

                if (!visited.TryAdd(position, steps))
                    continue;

                var currentLetter = map[position.X, position.Y];

                var nodes = Neighbors.Select(n => position + n)
                    .Where(n => n.X >= 0 && n.X < xMax
                            && n.Y >= 0 && n.Y < yMax)
                    .Where(n => map[n.X, n.Y] <= (currentLetter + 1))
                    .Where(n => !visited.ContainsKey(n))
                    .ToList();

                foreach (var node in nodes)
                {
                    // the total cost of visiting a node is the cost of the previous node plus its own 
                    var newPriority = steps+1;
                    q.Enqueue(node, newPriority);
                }       
            }
	
            return int.MaxValue;
            throw new Exception();
        }

        public string testData = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";

        (int[,], Position, Position) GetParsedData()
        {
            Position start = (0,0);
            Position end = (0,0);

            var grid = Data.To2dArray((c, pos) => {
                if (c == 'S')
                {
                    start = pos;
                    return 1;
                }
                else if (c == 'E')
                {
                    end = pos;
                    return 26;
                }
                else
                    return (int)(c - 'a' + 1);
            });

            return (grid, start, end);
        }

        (int[,], List<Position>, Position) GetParsedData_part2()
        {
            List<Position> starts = new();
            Position end = (0,0);

            var grid = Data.To2dArray((c, pos) => {
                if (c == 'S' || c == 'a')
                {
                    starts.Add(pos);
                    return 1;
                }
                else if (c == 'E')
                {
                    end = pos;
                    return 26;
                }
                else
                    return (int)(c - 'a' + 1);
            });

            return (grid, starts, end);
        }
    }
}
