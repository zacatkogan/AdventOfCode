namespace AdventOfCode.AoC2025;

public class Day_05 : BaseDay
{
    public override object Solve1()
    {
        var parts = DataLines.SplitOnElement("").ToList();
        var ranges = parts[0].Select(Range<long>.Parse);
        var items = parts[1].Select(long.Parse);

        return items.Where(item => ranges.Any(x => x.Contains(item))).Count();
    }

    public override object Solve2()
    {
        var parts = DataLines.SplitOnElement("").ToList();
        var ranges = parts[0].Select(Range<long>.Parse).ToList();

        var updated = true;

        while (updated)
        {
            updated = false;
            for (int i = 0; i < ranges.Count; i++)
            {
                var range = ranges[i];

                for (int j = i + 1; j < ranges.Count; j++)
                {
                    var testRange = ranges[j];
                    if (range.Overlaps(testRange))
                    {
                        range.Combine(testRange);
                        ranges.RemoveAt(j);
                        updated = true;
                    }
                }
            }
        }

        return ranges.Sum(x => x.Finish - x.Start + 1);
    }

    public string testData = "3-5\n10-14\n16-20\n12-18";
}
