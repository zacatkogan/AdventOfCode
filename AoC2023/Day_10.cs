namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using System.Runtime.ExceptionServices;

    public class Day_10: BaseDay
    {
        public static Position South = new Position(0, 1);
        public static Position North = new Position(0, -1);
        public static Position West = new Position(-1, 0);
        public static Position East = new Position(1, 0);

        Dictionary<char, List<Position>> DirectionMap = new Dictionary<char, List<Position>>()
        {
            {'|', new List<Position>{ North, South } },
            {'-', new List<Position>{ West, East }},
            {'L', new List<Position> { North, East } },
            {'J', new List<Position> { North, West } },
            {'7', new List<Position> { South, West} },
            {'F', new List<Position> { South, East } },
            {'S', new List<Position>() },
            {'.', new List<Position>() },
        };

        public override object Solve1()
        {
            var array = DataLines;
            var lineLength = array[0].Length;
            var numLines = array.Length;

            Position start = new Position(0, 0);
            // start
            for(int i = 0; i < numLines; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    if (array[i][j] == 'S')
                    {
                        start = new Position(j, i);
                    }
                }
            }

            var queue = new Queue<Position>();
            var distanceFromStart = new Dictionary<Position, int>();

            // get the neighbors of Start, and make sure they can actually point back to Start
            var startNeighborsPos = new List<Position> { North, South, East, West }.Select(x => start + x).ToList();

            foreach (var startNeighbor in startNeighborsPos)
            {
                var c = array[startNeighbor.Y][startNeighbor.X];
                var cNeighbors = DirectionMap[c];
                if (cNeighbors.Any(x => startNeighbor + x == start))
                {
                    queue.Enqueue(startNeighbor);
                    distanceFromStart.Add(startNeighbor, 1);
                }
                else
                {
                }
            }

            while (queue.TryDequeue(out Position nextPosition))
            {
                var thisPoint = nextPosition;
                var thisPointDistance = distanceFromStart[nextPosition];
                var thisPointChar = array[thisPoint.Y][thisPoint.X];

                var neighborsOfThisPoint = DirectionMap[thisPointChar];
                foreach (var neighbor in neighborsOfThisPoint)
                {
                    var neighborPoint = thisPoint + neighbor;
                    if (distanceFromStart.ContainsKey(neighborPoint))
                    {
                        if (distanceFromStart[neighborPoint] > thisPointDistance + 1)
                            distanceFromStart[neighborPoint] = thisPointDistance + 1;
                        continue;
                    }
                    queue.Enqueue(neighborPoint);
                    distanceFromStart.Add(neighborPoint, thisPointDistance + 1);
                }
            }

            return distanceFromStart.Max(x => x.Value);
        }

        public override object Solve2()
        {
            var array = DataLines;
            var lineLength = array[0].Length;
            var numLines = array.Length;

            Position start = new Position(0, 0);
            // start
            for (int i = 0; i < numLines; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    if (array[i][j] == 'S')
                    {
                        start = new Position(j, i);
                    }
                }
            }

            var queue = new Queue<Position>();
            var pointsOnLine = new HashSet<Position>();
            pointsOnLine.Add(start);

            // get the neighbors of Start, and make sure they can actually point back to Start
            var startNeighborsPos = new List<Position> { North, South, East, West }.Select(x => start + x).ToList();

            foreach (var startNeighbor in startNeighborsPos)
            {
                var c = array[startNeighbor.Y][startNeighbor.X];
                var cNeighbors = DirectionMap[c];
                if (cNeighbors.Any(x => startNeighbor + x == start))
                {
                    queue.Enqueue(startNeighbor);
                    pointsOnLine.Add(startNeighbor);
                }
            }

            while (queue.TryDequeue(out Position nextPosition))
            {
                var thisPoint = nextPosition;
                var thisPointChar = array[thisPoint.Y][thisPoint.X];

                var neighborsOfThisPoint = DirectionMap[thisPointChar];
                foreach (var neighbor in neighborsOfThisPoint)
                {
                    var neighborPoint = thisPoint + neighbor;
                    if (pointsOnLine.Contains(neighborPoint))
                    {
                        
                        continue;
                    }
                    queue.Enqueue(neighborPoint);
                    pointsOnLine.Add(neighborPoint);
                }
            }

            var arr = DataLines.Select(x => x.ToArray()).ToList();
            for (int i = 0; i < numLines; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    if (!pointsOnLine.Contains((j, i)))
                    {
                        arr[i][j] = ' ';
                    }
                }
            }

            var pointsInShape = new List<Position>();

            for (int i = 0; i < numLines; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    var p = new Position(j, i);

                    if (pointsOnLine.Contains(p))
                    {
                        continue;
                    }

                    // cast a ray up and see how many times it matches a point on the line

                    int crossings = 0;
                    char? prevCrossing = null;

                    for (int y = p.Y; y >= 0; y--)
                    {
                        var c = arr[y][p.X];
                        if (c == 'S') // S is actually a '7'
                            c = '7';

                        // a pipe doesn't count as a crossing
                        if (c == ' ' || c == '.')
                        {
                            prevCrossing = null;
                            continue;
                        }

                        if (c == '|')
                            continue;

                        if (c == '-')
                        {
                            crossings++;
                            prevCrossing = null;
                            continue;
                        }

                        else if (c == 'L' || c == 'J')
                        {
                            prevCrossing = c;
                        }

                        else if ((c == '7' && prevCrossing == 'L')
                            || (c == 'F' && prevCrossing == 'J'))
                        {
                            crossings++;
                            prevCrossing = null;
                        }

                        else if ((c == '7' && prevCrossing == 'J')
                            || (c == 'F' && prevCrossing == 'L'))
                        {
                            prevCrossing = null;
                        }
                        else
                        {
                            Console.WriteLine("something fucked up");
                        }

                    }

                    if (crossings % 2 != 0)
                    {
                        pointsInShape.Add(p);
                    }
                }
            }

            return pointsInShape.Count;
            // 269
        }
    }
}
