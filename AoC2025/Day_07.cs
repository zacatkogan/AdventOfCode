using System.Collections.Concurrent;

namespace AdventOfCode.AoC2025;

public class Day_07 : BaseDay
{
    public override object Solve1()
    {
        var startIndex = DataLines[0].IndexOf('S');

        var beams = new HashSet<Position>();
        beams.Add((startIndex, 0));

        int splits = 0;

        for (int row = 1; row < DataLines.Length; row++)
        {
            var beamsPrevRow = beams.Where(beam => beam.Y == row - 1).ToList();
            var currentRow = DataLines[row];

            foreach (var beam in beamsPrevRow)
            {
                var x = beam.X;
                if (currentRow[x] == '^')
                {
                    splits++;
                    beams.Add((x - 1, row));
                    beams.Add((x + 1, row));
                }
                else
                {
                    beams.Add((x, row));
                }
            }
        }

        return splits;
    }

    public override object Solve2()
    {
        // this is basically pascal's triangle with splitters
        // count the number of ways to reach each position, this feeds down into later positions

        var data = DataLines.Select(line => line.ToCharArray()).ToArray();
        var startIndex = DataLines[0].IndexOf('S');

        // use ConcurrentDictionary for AddOrUpdate method, easier than checking for existence first
        var beams = new ConcurrentDictionary<Position, long>();

        beams.TryAdd((startIndex, 0), 1);

        for (int row = 1; row < data.Length; row++)
        {
            var currentRow = data[row];
            
            // project beams down from previous row
            var projectedBeams = beams.Where(b => b.Key.Y == row - 1).ToList();

            foreach (var (pos, ways) in projectedBeams)
            {
                var x = pos.X;
                if (currentRow[x] == '^')
                {
                    // splitter
                    beams.AddOrUpdate((x - 1, row), ways, (_, oldWays) => oldWays + ways);
                    beams.AddOrUpdate((x + 1, row), ways, (_, oldWays) => oldWays + ways);
                }
                else
                {
                    // straight beam
                    beams.AddOrUpdate((x, row), ways, (_, oldWays) => oldWays + ways);
                }
            }
        }

        var total = beams.Where(b => b.Key.Y == data.Length - 1).Sum(y => y.Value);
        return total;
    }
}
