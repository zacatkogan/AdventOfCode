namespace AdventOfCode
{
    public record Position
    {
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
}
