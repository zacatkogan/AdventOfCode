namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;

    public class Day_05 : BaseDay
    {
        public override object Solve1()
        {
            var groups = Data.Split("\n\n");
            var seedsToPlant = groups[0].GetLongs().ToList();

            var mappings = groups.Select(gr =>
                gr.Split("\n").Skip(1)
                    .Select(line => line.GetLongs().ToList())
                    .Select(x => ParseLine(x[0], x[1], x[2])).ToList()
            ).ToList();

            var lowest = long.MaxValue;

            foreach (var seed in seedsToPlant)
            {
                var start = seed;
                foreach (var mapping in mappings)
                {
                    foreach (var mappedRange in mapping)
                    {
                        if (mappedRange.range.Contains(start))
                        {
                            var offset = start - mappedRange.range.Start;

                            start = mappedRange.loc + offset;
                            break;
                        }
                    }
                }

                lowest = Math.Min(lowest, start);
            }

            return lowest;
        }

        public override object Solve2()
        {
            var groups = Data.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries );
            var seedsToPlant = groups[0].GetLongs().ToList();

            var seedRangesToPlant = new List<RangeLong>();
            for (var i = 0; i < seedsToPlant.Count; i += 2) 
            {
                seedRangesToPlant.Add(new RangeLong(seedsToPlant[i], seedsToPlant[i] + seedsToPlant[i + 1] ));
            }

            var mappings = groups.Select(gr =>
                gr.Split("\n")
                    .Skip(1)                                    // Skip the header row
                    .Select(line => line.GetLongs().ToList())   // get the numbers as a list
                    .Select(x => ParseLine(x[0], x[1], x[2]))   // parse to ranges
                    .OrderBy(x => x.range.Start).ToList()       // then order them
            ).ToList();

            List<RangeLong> completedMappings = new();

            Queue<(int, RangeLong)> rangesToEvaluate = new();
            rangesToEvaluate.Enqueue(seedRangesToPlant.Select(x => (1, x)));
            
            while (rangesToEvaluate.TryDequeue(out var mappingRange))
            {
                var mappingDepth = mappingRange.Item1;
                var m = mappingRange.Item2;

                if (mappingDepth >= mappings.Count)
                {
                    completedMappings.Add(m);
                    continue;
                }
                var mappedRanges = mappings[mappingDepth];

                var overlaps = SplitRangeIntoOverlappingRanges(m, mappedRanges.Select(x => x.range)).ToList();

                // get the first result that overlaps
                foreach (var overlap in overlaps)
                {
                    var mappedRange = mappedRanges.FirstOrDefault(x => x.range.Overlaps(overlap));

                    if (mappedRange.range == null)
                    {
                        rangesToEvaluate.Enqueue((mappingDepth + 1, overlap));
                        continue;
                    }

                    var startOffset = overlap.Start - mappedRange.range.Start;
                    var start = mappedRange.loc + startOffset;
                    var end = start + overlap.Length;

                    rangesToEvaluate.Enqueue((mappingDepth + 1, new RangeLong (start, end )));
                }                
            }

            return completedMappings.Min(x => x.Start);
        }

        (long loc, RangeLong range) ParseLine(long a, long b, long c)
        {
            var location = a;
            var range = new RangeLong (b, b + c );

            return (location, range);
        }

        public string testData = "";

        public record RangeLong
        {
            public RangeLong(long start, long end)
            {
                ArgumentOutOfRangeException.ThrowIfNegative(start);
                ArgumentOutOfRangeException.ThrowIfNegative(end);

                Start = start;
                End = end;
            }

            public long Start { get; set; }
            public long End { get; set; }

            public long Length => End - Start;

            public bool Contains(long l)
            {
                return l >= Start && l < End;
            }

            public bool Overlaps(RangeLong other)
            {
                return Start < other.End && other.Start < End;
            }
        }

        public IEnumerable<RangeLong> SplitRangeIntoOverlappingRanges(RangeLong rangeToSplit, IEnumerable<RangeLong> overlappingRanges)
        {
            var endNodes = overlappingRanges.SelectMany(x => new[] { x.Start, x.End }).Distinct().OrderBy(x => x).ToList();

            var rangeSegments = new List<RangeLong>();

            var remainingRange = rangeToSplit;
            
            foreach (var endNode in endNodes) 
            {
                if (remainingRange.Contains(endNode))
                {
                    rangeSegments.Add(new RangeLong(remainingRange.Start, endNode));
                    remainingRange = new RangeLong(endNode, remainingRange.End);
                }
            }

            rangeSegments.Add(remainingRange);

            return rangeSegments.Where(x => x.Length != 0);
        }
    }
}
