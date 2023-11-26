using AdventOfCode.Utils;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Immutable;

namespace AdventOfCode.AoC2021
{
    public class Day_19 : BaseDay
    {
        // precomputed list of scanners that see other scanners:
        /*
        Scanner 0 sees scanners: 17,20,29
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
        public Dictionary<int, int[]> scannerMappings = new()
        {
            { 0, new[] { 17, 20, 29 } },
            { 1, new[] { 3, 8, 31 } },
            { 2, new[] { 4 } },
            { 3, new[] { 1, 13, 18, 26,} },
            { 4, new[] { 2, 33} },
            { 5, new[] { 8, 19, 31} },
            { 6, new[] { 7, 22,} },
            { 7, new[] { 6, 19, 27, 30} },
            { 8, new[] { 1, 5,} },
            { 9, new[] { 32 } },
            { 10, new[] { 24 } },
            { 11, new[] { 23, 30 } },
            { 12, new[] { 33 } },
            { 13, new[] { 3, 15 } },
            { 14, new[] { 22 } },
            { 15, new[] { 13, 26 } },
            { 16, new[] { 22, 28 } },
            { 17, new[] { 0 } },
            { 18, new[] { 3, 31 } },
            { 19, new[] { 5, 7, 33 } },
            { 20, new[] { 0, 24 } },
            { 21, new[] { 27, 30 } },
            { 22, new[] { 6, 14, 16, 27, 32 } },
            { 23, new[] { 11, 24 } },
            { 24, new[] { 10, 20, 23 } },
            { 25, new[] { 30 } },
            { 26, new[] { 3, 15 } },
            { 27, new[] { 7, 21, 22 } },
            { 28, new[] { 16 } },
            { 29, new[] { 0 } },
            { 30, new[] { 7, 11, 21, 25 } },
            { 31, new[] { 1, 5, 18 } },
            { 32, new[] { 9, 22 } },
            { 33, new[] { 4, 12, 19 } },
        };

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

            public ILookup<Position3d, Position3d> BeaconRelationships => _beaconRelationships ??= GetPositionRelationships(Beacons);
            private ILookup<Position3d, Position3d>? _beaconRelationships;
        }

        public override object Solve1()
        {
            var scanners = ParseInput();

            //var result = ScannerCanSeeScanner(scanners[0], scanners[17]);
            //return result;


            var knownScanners = new Dictionary<Scanner, Position3d>();

            var firstScanner = scanners[0];
            knownScanners.Add(firstScanner, (0, 0, 0));

            // CalculateScannerOverlaps(scanners);

            Dictionary<int, ScannerAbsolutePosition> scannerPositions = GetScannersAbsolutePositions(scanners);

            // now create the cloud of beacons
            var allBeacons = new HashSet<Position3d>();
            foreach (var s in scannerPositions.Values)
            {
                var scanner = scanners[s.ScannerId];

                var beacons = scanner.Beacons;
                foreach (var beacon in beacons)
                {
                    // translate from relative coords of beacon via absolute coords of scanner
                    var relativeInGlobalCoords = beacon.Rotate3d(s.AbsoluteRotationMatrix);
                    allBeacons.Add(s.AbsolutePosition + relativeInGlobalCoords);
                }

            }

            return allBeacons.Count;
        }

        public override object Solve2()
        {
            var scanners = ParseInput();

            var scannersAbsolutePositions = GetScannersAbsolutePositions(scanners);
            var scannersPositionsOnly = scannersAbsolutePositions.Select(x => x.Value.AbsolutePosition).ToList();

            var manhattanDistances = from s1 in scannersPositionsOnly
                            from s2 in scannersPositionsOnly
                            select (s1 - s2).ManhattanDistance();

            var greatestManhattanDistance = manhattanDistances.OrderByDescending(x => x).First();
            return greatestManhattanDistance;
        }

        private Dictionary<int, ScannerAbsolutePosition> GetScannersAbsolutePositions(List<Scanner> scanners)
        {
            var evaluatedScanner = new HashSet<int>();
            var scannersToEvaluate = new Queue<int>();
            scannersToEvaluate.Enqueue(0);
            var scannerPositions = new Dictionary<int, ScannerAbsolutePosition>
            {
                {
                    0,
                    new ScannerAbsolutePosition
                    {
                        ScannerId = 0,
                        AbsolutePosition = (0,0,0),
                        AbsoluteRotationMatrix = VectorHelpers.RotationVectors3d[0],
                    }
                }
            };

            while (scannersToEvaluate.TryDequeue(out int scannerId))
            {
                if (!evaluatedScanner.Add(scannerId))
                    continue;

                var currentScannerPositionAbsolute = scannerPositions[scannerId];

                // get the Scanners that are related to this scanner
                var relatedScanners = scannerMappings[scannerId];
                foreach (var m in relatedScanners)
                {
                    // check if we've already calculated the position of this, and skip if we have
                    if (scannerPositions.ContainsKey(m))
                        continue;

                    // otherwise, calculate the absolute position and add it to the queue to evaluate its connections
                    scannersToEvaluate.Enqueue(m);

                    // need to calculate the vector from S0 to Bn, Bn to S1
                    var relativePosition = ScannerCanSeeScanner(scanners[scannerId], scanners[m]).Value;
                    var relativePositionToAbsolutePositionCoords =
                        relativePosition.relativePositon.Rotate3d(currentScannerPositionAbsolute.AbsoluteRotationMatrix);

                    var newScannerPositionAbsolute = new ScannerAbsolutePosition
                    {
                        ScannerId = m,
                        AbsolutePosition = currentScannerPositionAbsolute.AbsolutePosition + relativePositionToAbsolutePositionCoords,
                        AbsoluteRotationMatrix = VectorHelpers.Multiply(currentScannerPositionAbsolute.AbsoluteRotationMatrix, relativePosition.relativeRotation)
                    };

                    scannerPositions.Add(m, newScannerPositionAbsolute);
                }
            }

            return scannerPositions;
        }

        /// <summary>
        /// this takes a while, so it was processed once and reused for subsequent iterations.
        /// </summary>
        /// <param name="scanners"></param>
        private static void CalculateScannerOverlaps(List<Scanner> scanners)
        {
            var overlappingScanners =
                           (from s1 in scanners.AsParallel()
                            from s2 in scanners.AsParallel()
                            where s1 != s2
                            select (s1, s2, relativePosition: ScannerCanSeeScanner(s1, s2))).AsParallel()
                           .Where(ss => ss.relativePosition != null)
                           .GroupBy(x => x.s1);

            foreach (var overlappingScanner in overlappingScanners)
            {
                var key = overlappingScanner.Key;
                Console.WriteLine($"Scanner {key.Id} sees scanners: {string.Join(",", overlappingScanner.Select(x => x.s2.Id))}");
            }
            return ;
        }

        public record struct ScannerAbsolutePosition
        {
            public int ScannerId;
            public Position3d AbsolutePosition;
            public int[,] AbsoluteRotationMatrix;
        }

        public static (Position3d relativePositon, int[,] relativeRotation)? ScannerCanSeeScanner(Scanner scanner, Scanner candidateScanner)
        {
            // the scanners don't see eachother, but they *do* see at least 12 of the same beacons.
            // so how do we compare the 2 different beacon clouds, given that we don't know the relative distance of the 2 origins?

            // for each beacon, get a vector representing its relationship to every other beacon.
            // if 11 or 12 of these vector-relationships match between scanner1 and scanner2, it's a possible match?
            // we might need to try rotating the candidateScanner through the 24 different orientations
            if (scanner == candidateScanner)
                return null;

            var s1Beacons = scanner.Beacons;
            var s2Beacons = candidateScanner.Beacons;

            foreach (var rotation in VectorHelpers.RotationVectors3d)
            // we may have to rotate the beacon points
            {
                var s2BeaconsRotated = s2Beacons.Rotate3d(rotation).ToList();

                foreach (var beacon2 in s2BeaconsRotated)
                {
                    var beacon2Relationships = s2BeaconsRotated.Select(x => beacon2 - x).ToList();

                    foreach (var beacon1 in s1Beacons)
                    {
                        var beacon1Relationships = s1Beacons.Select(x => beacon1 - x);

                        // check that at least 11 (+ 1 for the current beacon) have the same relationships
                        // get a count
                        // of points in b1
                        // where point in b2 == point in b1
                        // where count >= 12 (since b1 - b1 == b2 - b2 == 0 when b1/b2 measures the distance to itself
                        
                        var count = beacon1Relationships.Count(b1 => beacon2Relationships.Any(b2 => b1 == b2));
                        if (count >= 12)
                        {
                            // calculate the vector from S1 to beacon1, beacon1 to S2 in the coordinate system of S1
                            // S1 to B1 is `beacon1`
                            // B1 to S2 is `-beacon2`
                            return (beacon1 - beacon2, rotation);
                        }
                    }
                }
            }

            return null;
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

        # region helpers
        public void VerifyScannerOverlaps()
        {
            foreach (var mapping in scannerMappings)
            {
                // verify that there is a reciprocal mapping.
                // e.g. if Scanner 0 maps to Scanner 1, Scanner 1 should also map back to Scanner 0

                var currentScanner = mapping.Key;

                foreach (var s in mapping.Value)
                {
                    if (!scannerMappings[s].Contains(currentScanner))
                    {
                        Console.WriteLine($"scanner {s} is missing back-reference to scanner {currentScanner}");
                    }
                }
            }
        }
        #endregion

        public string testData = @"";
    }
}
