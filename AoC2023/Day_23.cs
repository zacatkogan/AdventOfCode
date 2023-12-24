namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using System.Collections.Concurrent;
    using System.Collections.Immutable;
    using System.Threading.Channels;

    public class Day_23 : BaseDay
    {
        public Dictionary<char, Position> AllowedTravelArrows = new()
        {
            { '>', (1,0) },
            { '<', (-1, 0) },
            { 'v', (0, 1) },
            { '^', (0, -1) },
        };

        public List<Position> Neighbors = new()
        {
            (1, 0), (-1, 0), (0, 1), (0, -1)
        };

        #region Part1
        public override object Solve1()
        {
            //Data = "#.#####################\n#.......#########...###\n#######.#########.#.###\n###.....#.>.>.###.#.###\n###v#####.#v#.###.#.###\n###.>...#.#.#.....#...#\n###v###.#.#.#########.#\n###...#.#.#.......#...#\n#####.#.#.#######.#.###\n#.....#.#.#.......#...#\n#.#####.#.#.#########v#\n#.#...#...#...###...>.#\n#.#.#v#######v###.###v#\n#...#.>.#...>.>.#.###.#\n#####v#.#.###v#.#.###.#\n#.....#...#...#.#.#...#\n#.#########.###.#.#.###\n#...###...#...#...#.###\n###.###.#.###v#####v###\n#...#...#.#.>.>.#.>.###\n#.###.###.#.###.#.#v###\n#.....###...###...#...#\n#####################.#";

            Position start = (1, 0);
            Position end = (DataLines[0].Length - 2, DataLines.Length-1);

            Queue<(Position currentPos, ImmutableHashSet<Position> visited)> queue = new();

            var initialMoves = ImmutableHashSet<Position>.Empty.Add(start).Add((1,1));

            queue.Enqueue(((1,1), initialMoves));

            var largest = 0;

            while (queue.TryDequeue(out var mapState))
            {
                var current = mapState.currentPos;
                var visited = mapState.visited;

                // todo: check if we're at the end.
                if (current == end)
                {
                    largest = int.Max(visited.Count, largest);
                    continue;
                }

                // if we're on a slope, then we have to move down the slope
                var currentChar = DataLines[current.Y][current.X];
                if (AllowedTravelArrows.ContainsKey(currentChar))
                {
                    
                    var next = current + AllowedTravelArrows[currentChar];
                    if (!visited.Contains(next))
                        queue.Enqueue((next, visited.Add(next)));

                    continue;
                }

                foreach (var neighbor in Neighbors)
                {
                    var neighborSquare = current + neighbor;

                    var neighborChar = DataLines[neighborSquare.Y][neighborSquare.X];

                    if (neighborChar == '#')
                    {
                        continue;
                    }

                    if (neighborChar == '.')
                    {
                        if (!visited.Contains(neighborSquare))
                            queue.Enqueue((neighborSquare, visited.Add(neighborSquare)));

                        continue;
                    }

                    if (AllowedTravelArrows[neighborChar] == neighbor)
                    {
                        queue.Enqueue((neighborSquare, visited.Add(neighborSquare)));
                        continue;
                    }
                }
            }
            return largest - 1; // starting square doesn't count

            // answer: 2430
        }

        #endregion

        #region Part2
        public override object Solve2()
        {
            Position start = (1, 0);
            Position end = (DataLines[0].Length - 2, DataLines.Length - 1);

            ConcurrentStack<(Position currentPos, ImmutableHashSet<Position> visited)> queue = new();

            var initialMoves = ImmutableHashSet<Position>.Empty.Add(start).Add((1, 1));

            queue.Push(((1, 1), initialMoves));

            var largest = 0;

            var threads = new List<Thread>();
            for (int i = 0; i < 20; i++)
            {
                var t = new Thread(() => RunQueue(queue, start, end, ref largest));
                t.Start();
                threads.Add(t);
            }

            threads.ForEach(x => x.Join());
            return largest - 1;


            //while (queue.TryPop(out var mapState))
            //{
            //    var current = mapState.currentPos;
            //    var visited = mapState.visited;

            //    // todo: check if we're at the end.
            //    if (current == end)
            //    {
            //        largest = int.Max(visited.Count, largest);
            //        continue;
            //    }


            //    foreach (var neighbor in Neighbors)
            //    {
            //        var neighborSquare = current + neighbor;

            //        var neighborChar = DataLines[neighborSquare.Y][neighborSquare.X];

            //        if (neighborChar == '#')
            //        {
            //            continue;
            //        }

            //        else
            //        {
            //            if (!visited.Contains(neighborSquare))
            //                queue.Push((neighborSquare, visited.Add(neighborSquare)));

            //            continue;
            //        }

            //    }
            //}
            //return largest - 1; // starting square doesn't count
        }

        public static object _lock = new object();

        public void RunQueue(ConcurrentStack<(Position currentPos, ImmutableHashSet<Position> visited)> queue, Position start, Position end, ref int largest)
        {
            int sleepCount = 0;
            while (true)
            {
                while (queue.TryPop(out var mapState))
                {
                    sleepCount = 0;
                    var current = mapState.currentPos;
                    var visited = mapState.visited;

                    // todo: check if we're at the end.
                    if (current == end)
                    {
                        if (visited.Count > largest)
                        {
                            lock (_lock)
                            {
                                largest = int.Max(visited.Count, largest);
                                Console.WriteLine(largest);
                            }
                        }
                        continue;
                    }

                    foreach (var neighbor in Neighbors)
                    {
                        var neighborSquare = current + neighbor;

                        var neighborChar = DataLines[neighborSquare.Y][neighborSquare.X];

                        if (neighborChar == '#')
                        {
                            continue;
                        }

                        else
                        {
                            if (!visited.Contains(neighborSquare))
                                queue.Push((neighborSquare, visited.Add(neighborSquare)));

                            continue;
                        }

                    }
                }

                Thread.Sleep(100);
                sleepCount++;
                if (sleepCount == 100)
                    return;
            }
        }
        #endregion
    }
}
