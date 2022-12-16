using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading.Tasks;
namespace AdventOfCode
{

    public class Day_15 : BaseDay
    {
        public class RangeLong
        {
            public RangeLong(long start, long end)
            {
                Start = start;
                End = end;
            }
            public long Start;
            public long End;

            public override string ToString()
            {
                return $"start:{Start}, end:{End}";
            }

            public static bool RangesOverlap(RangeLong a, RangeLong b)
            {
                return Math.Max(a.Start, b.Start) <= Math.Min(a.End, b.End)
                    || (a.Start <= b.Start && a.End >= b.End)
                    || (b.Start <= a.Start && b.End >= a.End);
            }

            public static RangeLong CombineRanges(RangeLong a, RangeLong b)
            {
                return new RangeLong(Math.Min(a.Start, b.Start), Math.Max(a.End, b.End));
            }
        }

        public static long ManhattanDistance(LongPosition p1, LongPosition p2)
        {
            var d = (p1 - p2);
            return Math.Abs(d.X) + Math.Abs(d.Y);
        }

        Regex regex = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

        public class Sensor
        {
            public LongPosition Position;
            public LongPosition BeaconPosition;

            public IEnumerable<LongPosition> InvalidPositionsOnRow(int row)
            {
                var maxDist = ManhattanDistance(Position, BeaconPosition);

                // check if the row is in range of the Sensor
                if (row < Position.Y - maxDist || row > Position.Y+maxDist)
                    yield break;

                // otherwise, return all the points on the row within +- (maxDist - (position.Y-row))
                var xDist = maxDist - Math.Abs(Position.Y - row);
                for (long i = Position.X - xDist; i <= Position.X + xDist; i++)
                {
                    yield return (i, row);
                }
            }

            public RangeLong? GetInvalidRangeForRow(long row)
            {
                long maxDist = ManhattanDistance(Position, BeaconPosition);

                // check if the row is in range of the Sensor
                if (row < Position.Y - maxDist || row > Position.Y+maxDist)
                    return null;

                // otherwise, return all the points on the row within +- (maxDist - (position.Y-row))
                var xDist = maxDist - Math.Abs(Position.Y - row);
                return new RangeLong(Position.X - xDist, Position.X + xDist);
            }
        }

        public IEnumerable<Sensor> ParseData()
        {
            foreach (var row in Data.Split("\n"))
            {
                var matches = regex.Match(row);
                LongPosition sensor = (int.Parse(matches.Groups[1].Value), int.Parse(matches.Groups[2].Value));
                LongPosition beacon = (int.Parse(matches.Groups[3].Value), int.Parse(matches.Groups[4].Value));

                yield return new Sensor() { Position = sensor, BeaconPosition = beacon};
            }

        }

        public override object Solve1()
        {
            var beacons = ParseData().ToList();
            var occupied = beacons.SelectMany(x => new[]{x.Position, x.BeaconPosition});

            var row = 2000000;

            return beacons.SelectMany(x => x.InvalidPositionsOnRow(row))
                .Distinct()
                .Except(occupied)
                .Count();
        }

        public object Solve1_1()
        {
            var beacons = ParseData().ToList();
            var occupied = beacons.SelectMany(x => new[]{x.Position, x.BeaconPosition});

            var row = 2000000;

            var ranges = beacons.Select(x => x.GetInvalidRangeForRow(row)).OrderBy(x => x.Start).ToList();

            while (true)
            {
                var combinedRanges = new List<RangeLong>();
                bool anyCombined = false;
                
            }

                
        }


        public override object Solve2()
        {
            var maxVal = 4_000_000;

            var beacons = ParseData().ToList();
            var occupied = beacons.SelectMany(x => new[]{x.Position, x.BeaconPosition});

            for (long row = 0; row <= 4_000_000; row++)
            {
                if (row % 100000 == 0)
                    Console.WriteLine(row);

                var ranges = beacons.Select(x => x.GetInvalidRangeForRow(row))
                    .Where(x => x != null)
                    .ToList();

                // clip ranges
                ranges.ForEach(x => 
                {
                    if (x.Start < 0)
                        x.Start = 0;
                    if (x.Start > maxVal)
                        x.Start = maxVal;
                    
                    if (x.End < 0)
                        x.End = 0;
                    if (x.End > maxVal)
                        x.End = maxVal;
                });

                // remove empty ranges, and order ascending
                ranges = ranges.Where(x => x.Start != x.End).OrderBy(x => x.Start).ToList();

                var firstRange = ranges.First();
                
                for (int i = 1; i < ranges.Count; i++)
                {
                    if (RangeLong.RangesOverlap(firstRange, ranges[i]))
                    {
                        firstRange = RangeLong.CombineRanges(firstRange, ranges[i]);
                    }
                    else
                    {
                        return row + (firstRange.End+1) * maxVal;
                    }
                }
            }

            return "";
        }

        public string testData = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";
    }
}
