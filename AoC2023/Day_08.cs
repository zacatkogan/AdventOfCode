namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using System.Text.RegularExpressions;

    public class Day_08 : BaseDay
    {
        public override object Solve1()
        {
            //Data = "RL\n\nAAA = (BBB, CCC)\nBBB = (DDD, EEE)\nCCC = (ZZZ, GGG)\nDDD = (DDD, DDD)\nEEE = (EEE, EEE)\nGGG = (GGG, GGG)\nZZZ = (ZZZ, ZZZ)";

            var steps = DataLines[0];

            var map = DataLines.Skip(2)
                .Select(x => Regex.Matches(x, @"\w{3}"))
                .Select(x => new { Start = x[0].Value, Left = x[1].Value, Right = x[2].Value })
                .ToDictionary(x => x.Start);

            var start = map["AAA"];
            var end = map["ZZZ"];
            var numSteps = 0;

            var current = start;

            while (true)
            {
                foreach (var s in steps)
                {
                    if (s == 'R')
                    {
                        current = map[current.Right];
                    }
                    else
                    {
                        current = map[current.Left];
                    }

                    numSteps++;

                    if (current == end)
                        return numSteps;
                }
            }

        }

        public override object Solve2()
        {
            var repeats = new List<long>
            {
                21883,
                19667,
                14681,
                16897,
                13019,
                11911,
            };

            var a = repeats.Select(x => x - 1).ToList();

            Dictionary<long, int> SeenValues = new Dictionary<long, int>();

            for (int j = 0; j < 1_000_000_000; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    a[i] += repeats[i];
                    if (SeenValues.TryGetValue(a[i], out int seenCount))
                    {
                        if (seenCount == 5)
                            return a[i];
                        
                        SeenValues[a[i]]++;
                    }
                    else
                    {
                        SeenValues.Add(a[i], 1);
                    }

                }
            }







            //Data = "LR\n\n11A = (11B, XXX)\n11B = (XXX, 11Z)\n11Z = (11B, XXX)\n22A = (22B, XXX)\n22B = (22C, 22C)\n22C = (22Z, 22Z)\n22Z = (22B, 22B)\nXXX = (XXX, XXX)";
            //DataLines = null;

            var steps = DataLines[0];

            var map = DataLines.Skip(2)
                .Select(x => Regex.Matches(x, @"\w{3}"))
                .Select(x => new { Start = x[0].Value, Left = x[1].Value, Right = x[2].Value })
                .ToDictionary(x => x.Start);

            var startingNodes = map.Values.Where(x => x.Start.EndsWith('A')).ToList();

            var numSteps = 0;

            var currentNodes = startingNodes.ToList();


            // 277 steps *  714 map nodes - should repeat every 197,778 steps

            // foreach node, find the first an second time that it hits each endpoint - 

            var nodeTimes = Enumerable.Repeat(0, startingNodes.Count)
                .Select(x => new List<(int step, string node)>())
                .ToList();

            while (true)
            {
                foreach (var s in steps)
                {
                    for (int i = 0; i < currentNodes.Count; i++)
                    {
                        var node = currentNodes[i];

                        if (s == 'R')
                        {
                            node = map[node.Right];
                        }
                        else
                        {
                            node = map[node.Left];
                        }

                        currentNodes[i] = node;

                        if (node.Start.EndsWith('Z'))
                            nodeTimes[i].Add((numSteps, node.Start));

                    }

                    numSteps++;

                    if (currentNodes.All(x => x.Start.EndsWith('Z')))
                    {
                        return numSteps;
                    }

                    if (numSteps > 1_00_000)
                    {
                        DumpData(nodeTimes);
                        break;
                    }
                }
            }

        }
    
        public void DumpData(List<List<(int, string)>> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                Console.WriteLine($"Checking Node {i} for repeats");
                GetPatternRepeatCountAndLength(data[i]);
            }
        }

        public void GetPatternRepeatCountAndLength(List<(int, string)> data)
        {
            var N = 3;
            // starting at index 1, take the first N elements
            for (int i = 0; i <  data.Count - N; i++)
            {
                var elementsToMatch = data.Select(x => x.Item2).Skip(i).Take(N).ToList();

                // move along the array, comparing the N elements against the elementsToMatch
                for (int j = i + 1; j < data.Count -N; j++)
                {
                    var seqB = data[j..(j + N)];

                    if (Matches(elementsToMatch, seqB.Select(x => x.Item2)))
                    { 
                        var startStep = data[i].Item1;
                        var endStep = data[j].Item1;

                        Console.WriteLine("Repeats After " + (endStep - startStep) + " steps, starting at " + startStep);
                        break;
                    }

                }


            }
        }

        public bool Matches(IEnumerable<string> a, IEnumerable<string> b)
        {
            return a.Zip(b).All(x => x.First == x.Second);
        }
    }
}
