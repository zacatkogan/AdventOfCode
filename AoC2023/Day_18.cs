using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace AdventOfCode.AoC2023
{
    public class Day_18: BaseDay
    {
        public override object Solve1()
        {
            var trench = new HashSet<Position>();
            Position p = (0,0);
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

        public override object Solve2()
        {
            var trench = new HashSet<Position>();
            Position p = (0, 0);
            trench.Add(p);

            foreach (var line in DataLines)
            {
                var splits = line.Split(' ').Last()[2..];
                var lengthEncoded = splits[..5];
                var dir = int.Parse(splits[5..6]);
                var distance = Convert.ToInt32(lengthEncoded, 16);
                var dirMap = new List<string> { "R", "D", "L", "U" };
                var dirVector = Position.Directions[dirMap[dir]];
                
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
