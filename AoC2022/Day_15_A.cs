using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading.Tasks;
namespace AdventOfCode
{

    public class Day_15_A : BaseDay
    {
        public override int GetProblemDay() => 15;

        Regex regex = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

        public class Sensor
        {
            public PositionLong Position;
            public PositionLong BeaconPosition;

            public IEnumerable<PositionLong> GetOutsideRing()
            {
                var dist = PositionLong.ManhattanDistance(Position, BeaconPosition) + 1;

                for (int i = 0; i < dist; i++)
                {
                    yield return Position + (dist - i, i);
                    yield return Position + (-i, dist - i);
                    yield return Position + (-dist + i, -i);
                    yield return Position + (i, -dist + i);
                }
            }

            public bool ContainsPoint(PositionLong p)
            {
                var dist = PositionLong.ManhattanDistance(Position, BeaconPosition);
                var pointDistance = PositionLong.ManhattanDistance(Position, p);

                return pointDistance <= dist;
            }
        }

        public IEnumerable<Sensor> ParseData()
        {
            foreach (var row in Data.Split("\n"))
            {
                var matches = regex.Match(row);
                PositionLong sensor = (int.Parse(matches.Groups[1].Value), int.Parse(matches.Groups[2].Value));
                PositionLong beacon = (int.Parse(matches.Groups[3].Value), int.Parse(matches.Groups[4].Value));

                yield return new Sensor() { Position = sensor, BeaconPosition = beacon};
            }

        }

        public override object Solve1()
        {
            return "";
        }

        public override object Solve2()
        {
            var maxVal = 4_000_000;

            var beacons = ParseData().ToList();

            var beaconRings = beacons.Select(x => x.GetOutsideRing().Where(p => p.X >= 0 & p.X <=maxVal & p.Y >= 0 & p.Y <= maxVal));

            var hs = new HashSet<PositionLong>();
                        
                


            var unorderedHits = beaconRings.Reverse()
                .SelectMany(x => x)
            ;

            foreach (var candidate in unorderedHits)
            {
                if (beacons.Any(b => b.ContainsPoint(candidate)))
                    continue;
                
                return candidate.X * 4000000 + candidate.Y;
            }

            return "";
        }
    }
}
