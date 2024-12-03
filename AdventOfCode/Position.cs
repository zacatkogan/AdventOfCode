using System.Collections;
using System.Runtime.CompilerServices;
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

    public record struct PositionLong
    {
        public static long ManhattanDistance(PositionLong a, PositionLong b)
        {
            var distance = a - b;
            return Math.Abs(distance.X) + Math.Abs(distance.Y);
        }

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

        public PositionLong(IEnumerable<long> longs)
        {
            var l = longs.Take(2).ToList();
            X = l[0];
            Y = l[1];
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

        public Position3d(IEnumerable<int> points)
        {
            var p = points.ToList();
            if (p.Count != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(points), "Must contain 3 points");
            }

            X = p[0];
            Y = p[1];
            Z = p[2];
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

        public static Position3d operator -(Position3d p1, Position3d p2)
        {
            return (p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static List<Position3d> Neighbors = new List<Position3d>()
        {
            (0,0,1), (0,1,0), (1,0,0), (0,0,-1), (0,-1,0), (-1,0,0)
        };

        public int ManhattanDistance()
        {
            return this.Sum(Math.Abs);
        }
    
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

    public record struct Position3dLong : IEnumerable<long>
    {
        public Position3dLong(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position3dLong(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position3dLong(IEnumerable<long> v)
        {
            var vv = v.Take(3).ToList();
            X = vv[0];
            Y = vv[1];
            Z = vv[2];
        }

        public long X;
        public long Y;
        public long Z;

        public static implicit operator Position3dLong((long, long, long) point)
        {
            return new Position3dLong(point.Item1, point.Item2, point.Item3);
        }

        public static Position3dLong operator +(Position3dLong p1, Position3dLong p2)
        {
            return (p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static List<Position3d> Neighbors = new List<Position3d>()
        {
            (0,0,1), (0,1,0), (1,0,0), (0,0,-1), (0,-1,0), (-1,0,0)
        };
    
        IEnumerator<long> IEnumerable<long>.GetEnumerator()
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

    public record struct PositionDouble
    {
        public static double ManhattanDistance(PositionDouble a, PositionDouble b)
        {
            var distance = a - b;
            return Math.Abs(distance.X) + Math.Abs(distance.Y);
        }

        public static Dictionary<string, PositionDouble> Directions = new()
        {
            {"U",  (0, 1)},
            {"D", (0, -1)},
            {"L", (-1, 0)},
            {"R", (1, 0)}
        };

        public PositionDouble()
        {
            this.X = 0;
            this.Y = 0;
        }

        public PositionDouble(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public PositionDouble(IEnumerable<double> values)
        {
            var l = values.Take(2).ToList();
            X = l[0];
            Y = l[1];
        }

        public double X;
        public double Y;

        public void Deconstruct(out double x, out double y)
        {
            x = this.X;
            y = this.Y;
        }

        public static PositionDouble operator +(PositionDouble a, PositionDouble b)
        {
            return new PositionDouble(a.X + b.X, a.Y + b.Y);
        }

        public static PositionDouble operator -(PositionDouble a, PositionDouble b)
        {
            return new PositionDouble(a.X - b.X, a.Y - b.Y);
        }

        public static implicit operator PositionDouble((double, double) point)
        {
            return new PositionDouble(point.Item1, point.Item2);
        }

        public static implicit operator (double, double)(PositionDouble pos)
        {
            return (pos.X, pos.Y);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }

}
