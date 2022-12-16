namespace AdventOfCode
{
    public record Position
    {
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
}
