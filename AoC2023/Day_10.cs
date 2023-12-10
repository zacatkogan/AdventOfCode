namespace AdventOfCode.AoC2023
{
    using AdventOfCode;

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
            }

            while (queue.TryDequeue(out Position thisPoint))
            {
                var thisPointDistance = distanceFromStart[thisPoint];
                var thisPointChar = array[thisPoint.Y][thisPoint.X];

                var neighborsOfThisPoint = DirectionMap[thisPointChar];
                foreach (var neighbor in neighborsOfThisPoint)
                {
                    var neighborPoint = thisPoint + neighbor;
                    if (distanceFromStart.ContainsKey(neighborPoint))
                        continue;

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
                        continue;

                    queue.Enqueue(neighborPoint);
                    pointsOnLine.Add(neighborPoint);
                }
            }

            var pointsInside = new List<Position>();

            for (var y = 0; y < numLines; y++)
            {
                var line = DataLines[y];
                var inside = false;
                char prevChar = ' ';

                for (var x = 0; x < numLines; x++)
                {
                    if (!pointsOnLine.Contains((x,y)))
                    {
                        if (inside)
                            pointsInside.Add((x, y));

                        continue;
                    }

                    var c = line[x];
                    if (c == 'S')
                        c = '7';

                    if (c == '-')
                        continue;

                    if (c == '|')
                        inside = !inside;

                    if (c == 'L' || c == 'F') // start of a bend
                        prevChar = c;

                    // end of a bend - opposite sides = crossing
                    if ((prevChar == 'L' && c == '7') || (prevChar == 'F' && c == 'J'))
                        inside = !inside;

                    // end of a bend - same sides = no crossing
                    if ((prevChar == 'L' && c == 'J') || (prevChar == 'F' && c == '&'))
                        continue;
                }
            }

            return pointsInside.Count;
        }
    }
}
