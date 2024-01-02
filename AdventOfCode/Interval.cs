using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /// <summary>
    /// functions a bit like Range, but more useful
    /// </summary>
    public class Interval
    {
        public int Start { get; init; }
        public int End { get; init; }
        public long Length => End - Start;

        /// <param name="start">inclusive</param>
        /// <param name="end">exclusive</param>
        public Interval(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(int value)
        {
            return Start <= value && value < End;
        }

        public IEnumerable<Interval> SplitByPoints(IEnumerable<int> points)
        {
            var segments = new List<Interval>();
            var remainingRange = this;

            foreach (var point in points)
            {
                if (remainingRange.Contains(point))
                {
                    segments.Add(new Interval(remainingRange.Start, point));
                    remainingRange = new Interval(point, remainingRange.End);
                }
            }

            segments.Add(remainingRange);

            return segments.Where(x => x.Length != 0);
        }
        public static implicit operator Interval(Range range)
        {
            return new Interval(range.Start.Value, range.End.Value);
        }

        public override string ToString()
        {
            return $"{Start}-{End}";
        }
    }

    public class IntervalLong
    {
        public long Start { get; init; }
        public long End { get; init; }
        public long Length => End - Start;

        /// <param name="start">inclusive</param>
        /// <param name="end">exclusive</param>
        public IntervalLong(long start, long end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(long value)
        {
            return Start <= value && value < End;
        }

        public IEnumerable<IntervalLong> SplitByPoints(IEnumerable<int> points)
        {
            var segments = new List<IntervalLong>();
            var remainingRange = this;

            foreach (var point in points)
            {
                if (remainingRange.Contains(point))
                {
                    segments.Add(new IntervalLong(remainingRange.Start, point));
                    remainingRange = new IntervalLong(point, remainingRange.End);
                }
            }

            segments.Add(remainingRange);

            return segments.Where(x => x.Length != 0);
        }

        public static implicit operator IntervalLong(Range range)
        {
            return new IntervalLong(range.Start.Value, range.End.Value);
        }
    }

    public class IntervalDouble
    {
        public double Start { get; init; }
        public double End { get; init; }
        public double Length => End - Start;

        /// <param name="start">inclusive</param>
        /// <param name="end">exclusive</param>
        public IntervalDouble(double start, double end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(double value)
        {
            return Start <= value && value < End;
        }

    }
}
