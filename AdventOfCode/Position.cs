using System.Collections;
namespace AdventOfCode
{
    public record Position : IEnumerable<int>
    {
        public static int ManhattanDistance(Position a, Position b)
        {
            var distance = a - b;
            return Math.Abs(distance.X) + Math.Abs(distance.Y);
        }

        public static Dictionary<string, Position> Directions = new()
        {
            {"U",  (0, 1)},
            {"D", (0, -1)},
            {"L", (-1, 0)},
            {"R", (1, 0)}
        };

        public Position()
        {
            this.X = 0;
            this.Y = 0;
        }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X;
        public int Y;

        public void Deconstruct(out int x, out int y)
        {
            x = this.X;
            y = this.Y;
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }
        
        public static Position operator *(Position a, int b)
        {
            return new Position(a.X * b, a.Y * b);
        }

        public static implicit operator Position((int, int) point)
        {
            return new Position(point.Item1, point.Item2);
        }

        public static implicit operator (int, int)(Position pos)
        {
            return (pos.X,pos.Y);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            yield return X;
            yield return Y;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return X;
            yield return Y;
        }
    }

    public record PositionLong
    {
        public static Dictionary<string, PositionLong> Directions = new()
        {
            {"U",  (0, 1)},
            {"D", (0, -1)},
            {"L", (-1, 0)},
            {"R", (1, 0)}
        };

        public PositionLong()
        {
            this.X = 0;
            this.Y = 0;
        }

        public PositionLong(long x, long y)
        {
            this.X = x;
            this.Y = y;
        }
        public long X;
        public long Y;

        public void Deconstruct(out long x, out long y)
        {
            x = this.X;
            y = this.Y;
        }

        public static PositionLong operator +(PositionLong a, PositionLong b)
        {
            return new PositionLong(a.X + b.X, a.Y + b.Y);
        }

        public static PositionLong operator -(PositionLong a, PositionLong b)
        {
            return new PositionLong(a.X - b.X, a.Y - b.Y);
        }

        public static implicit operator PositionLong((long, long) point)
        {
            return new PositionLong(point.Item1, point.Item2);
        }

        public static implicit operator (long, long)(PositionLong pos)
        {
            return (pos.X,pos.Y);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }

    public record struct Position3d : IEnumerable<int>
    {
        public Position3d(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X;
        public int Y;
        public int Z;

        public static implicit operator Position3d((int, int, int) point)
        {
            return new Position3d(point.Item1, point.Item2, point.Item3);
        }

        public static Position3d operator +(Position3d p1, Position3d p2)
        {
            return (p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static List<Position3d> Neighbors = new List<Position3d>()
        {
            (0,0,1), (0,1,0), (1,0,0), (0,0,-1), (0,-1,0), (-1,0,0)
        };
    
        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
        }
    }
}
