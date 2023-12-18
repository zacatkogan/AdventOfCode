namespace AdventOfCode.AoC2023
{
    public class Day_18: BaseDay
    {
        public override object Solve1()
        {
            var p = new Position(0, 0);
            var lineSegments = new List<LineSegment>();
            foreach (var line in DataLines)
            {
                var splits = line.Split(' ');
                var dir = splits[0];
                var distance = int.Parse(splits[1]);

                var dirVector = Position.Directions[dir];

                var ls = new LineSegment
                {
                    Start = p,
                    End = p + (dirVector * distance),
                    Length = distance,
                    Direction = dir,
                    DirectionVector = dirVector,
                };
                lineSegments.Add(ls);
                p = ls.End;
            }

            return CalculateAreaShoelace(lineSegments);
        }

        public override object Solve2()
        {
            //Data = "R 6 (#70c710)\nD 5 (#0dc571)\nL 2 (#5713f0)\nD 2 (#d2c081)\nR 2 (#59c680)\nD 2 (#411b91)\nL 5 (#8ceee2)\nU 2 (#caa173)\nL 1 (#1b58a2)\nU 2 (#caa171)\nR 2 (#7807d2)\nU 3 (#a77fa3)\nL 2 (#015232)\nU 2 (#7a21e3)";

            Position p = (0, 0);

            var lineSegments = new List<LineSegment>();
            var dirMap = new List<string> { "R", "D", "L", "U" };

            foreach (var line in DataLines)
            {
                var splits = line.Split(' ').Last()[2..];
                var lengthEncoded = splits[..5];
                var dir = int.Parse(splits[5..6]);
                var distance = Convert.ToInt32(lengthEncoded, 16);

                var dirName = dirMap[dir];
                var dirVector = Position.Directions[dirMap[dir]];

                var ls = new LineSegment
                {
                    Start = p,
                    End = p + (dirVector * distance),
                    Length = distance,
                    Direction = dirName,
                    DirectionVector = dirVector,
                };
                lineSegments.Add(ls);
                p = ls.End;
            }

            return CalculateAreaShoelace(lineSegments);
        }

        public long CalculateAreaShoelace(IEnumerable<LineSegment> lineSegments)
        {
            // use the Shoelace formula to get the interior area
            long cumulativeSize = 0;
            foreach (var ls in lineSegments)
            {
                var p1 = ls.Start;
                var p2 = ls.End;

                cumulativeSize += ((long)p1.X * p2.Y) - ((long)p1.Y * p2.X);
            }

            // add the Perimeter
            var perimeter = lineSegments.Sum(x => x.Length);

            // no idea why the `+1`, but that's what works.
            return (Math.Abs(cumulativeSize) + perimeter) / 2 + 1;
        }

        public class LineSegment
        {
            public Position Start;
            public Position End;
            public string Direction;
            public Position DirectionVector;
            public int Length;

            public override string ToString()
            {
                return $"{Start} - {End}: {Direction}";
            }
        }

        public long Solve1Original()
        {
            var trench = new HashSet<Position>();
            Position p = (0, 0);
            trench.Add(p);

            foreach (var line in DataLines)
            {
                var splits = line.Split(' ');
                var dir = splits[0];
                var distance = int.Parse(splits[1]);

                var dirVector = Position.Directions[dir];
                for (int i = 0; i < distance; i++)
                {
                    p += dirVector;
                    trench.Add(p);
                }
            }

            // dig out the interior of the trench
            var minX = trench.Min(p => p.X);
            var maxX = trench.Max(p => p.X);
            var minY = trench.Min(p => p.Y);
            var maxY = trench.Max(p => p.Y);

            // do a flood-fill from a point next to (0,0)
            //var visited = new HashSet<Position>();
            var queue = new Queue<Position>();
            queue.Enqueue((1, 1));

            while (queue.TryDequeue(out var position))
            {
                foreach (var dir in Position.Directions.Values)
                {
                    if (position.X < minX || position.X > maxX || position.Y < minY || position.Y > maxY)
                        ;

                    var next = position + dir;
                    if (trench.Add(next))
                    {
                        queue.Enqueue(next);
                    }
                }
            }

            return trench.Count;
        }
    }
}
