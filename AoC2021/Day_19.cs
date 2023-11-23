using AdventOfCode.Utils;
using SkiaSharp;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Net.WebSockets;

namespace AdventOfCode.AoC2021
{
    public class Day_19 : BaseDay
    {
        // precomputed list of scanners that see other scanners:
/*
 * Scanner 0 sees scanners: 17,20,29
Scanner 1 sees scanners: 3,8,31
Scanner 2 sees scanners: 4
Scanner 3 sees scanners: 1,13,18,26
Scanner 4 sees scanners: 2,33
Scanner 5 sees scanners: 8,19,31
Scanner 6 sees scanners: 7,22
Scanner 7 sees scanners: 6,19,27,30
Scanner 8 sees scanners: 1,5
Scanner 9 sees scanners: 32
Scanner 10 sees scanners: 24
Scanner 11 sees scanners: 23,30
Scanner 12 sees scanners: 33
Scanner 13 sees scanners: 3,15
Scanner 14 sees scanners: 22
Scanner 15 sees scanners: 13,26
Scanner 16 sees scanners: 22,28
Scanner 17 sees scanners: 0
Scanner 18 sees scanners: 3,31
Scanner 19 sees scanners: 5,7,33
Scanner 20 sees scanners: 0,24
Scanner 21 sees scanners: 27,30
Scanner 22 sees scanners: 6,14,16,27,32
Scanner 23 sees scanners: 11,24
Scanner 24 sees scanners: 10,20,23
Scanner 25 sees scanners: 30
Scanner 26 sees scanners: 3,15
Scanner 27 sees scanners: 7,21,22
Scanner 28 sees scanners: 16
Scanner 29 sees scanners: 0
Scanner 30 sees scanners: 7,11,21,25
Scanner 31 sees scanners: 1,5,18
Scanner 32 sees scanners: 9,22
Scanner 33 sees scanners: 4,12,19
*/

        public List<Scanner> ParseInput()
        {
            List<Scanner> scanners = new List<Scanner>();
            Scanner scanner = null;
            int scannerId = 0;

            foreach (var line in DataLines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                if (line.StartsWith("---"))
                {
                    scanner = new Scanner();
                    scanners.Add(scanner);
                    scanner.Id = scannerId;
                    scannerId++;
                    continue;
                }

                scanner!.Beacons.Add(new Position3d(line.Split(",").Select(int.Parse)));
            }

            return scanners;
        }

        public record Scanner
        {
            public int Id { get; set; }
            public List<Position3d> Beacons { get; set; } = new List<Position3d>();

            public Dictionary<Position3d, int> BeaconDistances
            {
                get => _beaconDistances ??= Beacons.ToDictionary(
                    p => p, p => Math.Abs(p.X) + Math.Abs(p.Y) + Math.Abs(p.Z));
            }
            private Dictionary<Position3d, int>? _beaconDistances;

            public ILookup<Position3d, Position3d> BeaconRelationships => _beaconRelationships ??= GetPositionRelationships(Beacons);
            private ILookup<Position3d, Position3d>? _beaconRelationships;
        }

        public override object Solve1()
        {
            var scanners = ParseInput();

            var knownScanners = new Dictionary<Scanner, Position3d>();

            var firstScanner = scanners[0];
            knownScanners.Add(firstScanner, (0, 0, 0));

            // ScannerCanSeeScanner takes 10 seconds per scanner.
            // doing this in parallel takes an average of 1.6s per scanner, a speed up of 6x

            var overlappingScanners =
                (from s1 in scanners.AsParallel()
                from s2 in scanners.AsParallel()
                where s1 != s2
                    && ScannerCanSeeScanner(s1, s2)
                select (s1, s2)).AsParallel()
                .GroupBy(x => x.s1);

            foreach (var overlappingScanner in overlappingScanners)
            {
                var key = overlappingScanner.Key;
                Console.WriteLine($"Scanner {key.Id} sees scanners: {string.Join(",", overlappingScanner.Select(x => x.s2.Id))}");
            }

            return scanners.Count();
        }

        public override object Solve2()
        {
            throw new NotImplementedException();
        }

        public bool ScannerCanSeeScanner(Scanner scanner, Scanner candidateScanner)
        {
            // the scanners don't see eachother, but they *do* see at least 12 of the same beacons.
            // so how do we compare the 2 different beacon clouds, given that we don't know the relative distance of the 2 origins?

            // for each beacon, get a vector representing its relationship to every other beacon.
            // if 11 or 12 of these vector-relationships match between scanner1 and scanner2, it's a possible match?
            // we might need to try rotating the candidateScanner through the 24 different orientations

            var s1 = scanner.BeaconRelationships;
            var s2 = candidateScanner.BeaconRelationships;

            foreach (var rotation in VectorHelpers.RotationVectors3d)
            // we may have to rotate the beacon points
            {
                foreach (var beacon2 in s2)
                {
                    var rotatedBeacon2 = beacon2.Rotate3d(rotation).ToList();

                    foreach (var beacon1 in s1)
                    {
                        // check that at least 11 (+ 1 for the current beacon) have the same relationships
                        // get a count
                        // of points in b1
                        // where point in b2 == point in b1
                        // where count > 11
                        var count = beacon1.Count(b1 => rotatedBeacon2.Any(b2 => b1 == b2));
                        if (count >= 11)
                            return true;
                    }
                }
            }

            return false;
        }

        public bool CompareDistances(Position3d p1, Position3d p2)
        {
            var p1Ordered = new Position3d(p1.Select(Math.Abs).OrderBy(x => x));
            var p2Ordered = new Position3d(p2.Select(Math.Abs).OrderBy(x => x));

            return p1Ordered == p2Ordered;
        }

        public static ILookup<Position3d, Position3d> GetPositionRelationships(IEnumerable<Position3d> beacons)
        {
            var beaconRelationships =
                from p1 in beacons
                from p2 in beacons
                where p1 != p2
                select (p1, p2);

            return beaconRelationships.ToLookup(r => r.p1, r => r.p1 - r.p2);
        }

        public string testData = @"";
    }
}
