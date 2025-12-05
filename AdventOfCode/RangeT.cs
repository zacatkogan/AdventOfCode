using System.Numerics;

namespace AdventOfCode
{
    /// <summary>
    /// Represents an Inclusive Range from Start to Finish
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Range<T> where T: INumber<T>
    {
        public Range(T start, T finish)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(finish, start);

            Start = start;
            Finish = finish;
        }

        public T Start { get; set; }
        public T Finish { get; set; }

        public bool Contains(T value)
        {
            return value >= Start && value <= Finish;
        }

        public override string ToString()
        {
            return $"{Start} - {Finish}";
        }

        public static Range<T> Parse(string value)
        {
            var parts = value.Split("-");
            return new Range<T>(T.Parse(parts[0], null), T.Parse(parts[1], null));
        }

        public bool Overlaps(Range<T> other)
        {
            return Overlaps(this, other);
        }

        public static bool Overlaps(Range<T> left, Range<T> right)
        {
            return left.Contains(right.Start) || left.Contains(right.Finish)
                || right.Contains(left.Start) || right.Contains(left.Finish);
        }

        public Range<T> Combine(Range<T> other)
        {
            this.Start = T.Min(Start, other.Start);
            this.Finish = T.Max(Finish, other.Finish);

            return this;
        }

        public static Range<T> CombineRanges(Range<T> left, Range<T> right)
        {
            return new Range<T>(T.Min(left.Start, right.Start), T.Max(left.Finish, right.Finish));
        }
    }
}
