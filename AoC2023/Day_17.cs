using System.Collections.Immutable;
using System.Collections.Specialized;

namespace AdventOfCode.AoC2023
{
    public class Day_17: BaseDay
    {
        public static Position South = new Position(0, 1);
        public static Position North = new Position(0, -1);
        public static Position West = new Position(-1, 0);
        public static Position East = new Position(1, 0);

        List<Position> PossibleMoves = new() { South, North, East, West };

        string testData = "2413432311323\n3215453535623\n3255245654254\n3446585845452\n4546657867536\n1438598798454\n4457876987766\n3637877979653\n4654967986887\n4564679986453\n1224686865563\n2546548887735\n4322674655533";

        public override object Solve1()
        {
            Data = testData;

            Move start = new Move()
            {
                Position = (0, 0),
                HeatLoss = 0,
            };

            Position end = (DataLines.Length-1, DataLines.Length-1);

            int bestHeatLoss = int.MaxValue;
            Move bestMove = new Move();

            PriorityQueue<Move, (int dist, int heatloss)> toEvaluate = new(PriorityComparer.Instance);
            Dictionary<Position, int> evaluated = new();

            toEvaluate.Enqueue(start, (0,0));
            while(toEvaluate.TryDequeue(out var move, out _))
            {
                if (move.MoveHistory.Count() > 3)
                {
                    var prevMove = move.MoveHistory.Peek();
                    if (move.MoveHistory.Take(4).All(x => x == prevMove))
                        // can't have more than 3 of the same move in a row
                        continue;
                }

                if (move.HeatLoss > bestHeatLoss)
                {
                    continue;
                }

                if (move.Position == end)
                {
                    if (move.HeatLoss < bestHeatLoss)
                    {
                        bestHeatLoss = move.HeatLoss;
                        bestMove = move;
                        continue;
                    }
                }

                if (!evaluated.ContainsKey(move.Position))
                {
                    evaluated[move.Position] = move.HeatLoss;
                }
                else
                {
                    if (evaluated[move.Position] < move.HeatLoss)
                        continue;
                    else
                        evaluated[move.Position] = move.HeatLoss;
                }

                foreach (var dir in PossibleMoves)
                {
                    var newMove = new Move()
                    {
                        PositionHistory = move.PositionHistory.Push((move.Position, move.HeatLoss, dir)),
                        Position = move.Position + dir,
                        MoveHistory = move.MoveHistory.Push(dir),
                        HeatLoss = move.HeatLoss,
                    };

                    var p = newMove.Position;
                    if (move.PositionHistory.Count() > 0 && p == move.PositionHistory.Peek().Item1)
                        continue;

                    if (p.X < 0 || p.Y < 0 || p.X > end.X || p.Y > end.Y)
                        continue; // out of bounds

                    var newHeatLoss = DataLines[p.Y][p.X];
                    newMove.HeatLoss += int.Parse(newHeatLoss.ToString());
                    toEvaluate.Enqueue(newMove, (Position.ManhattanDistance(end, newMove.Position), newMove.HeatLoss));
                }
            }

            return bestHeatLoss;

            // 941 too low
            // 944 too low
            // 1038 too high
            // 1027 is wrong
            // 991 is wrong
        }

        public override object Solve2()
        {

            return "";
        }

        public class Move
        {
            public ImmutableStack<Position> MoveHistory = [];
            public ImmutableStack<(Position, int, Position)> PositionHistory = [];

            public Position Position { get; set; } = (0, 0);

            public int HeatLoss { get; set; }
        }

        public class PriorityComparer : IComparer<(int dist, int heatloss)>
        {
            public static PriorityComparer Instance = new PriorityComparer();

            public int Compare((int, int) x, (int, int) y)
            {
                if (x.Item1 < y.Item1) return -1;
                if (x.Item1 > y.Item1) return 1;
                else
                {
                    if (x.Item2 < y.Item2) return -1;
                    if (x.Item2 > y.Item2) return 1;
                    else return 0;
                }
            }
        }
    }
}
