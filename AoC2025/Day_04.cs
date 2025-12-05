namespace AdventOfCode.AoC2025;

public class Day_04 : BaseDay
{
    public override object Solve1()
    {
        var count = 0;

        var minX = 0;
        var minY = 0;
        var maxY = DataLines.Length;
        var maxX = DataLines[0].Length;

        for (int y = 0; y < maxY; y++)
        {
            var line = DataLines[y];
            for (int x = 0; x < maxX; x++)
            {
                var currentPos = (x, y);

                if (DataLines[y][x] != '@')
                    continue;

                var sum = Position.AllDirections.Select(d => currentPos + d)
                    .Where(p => p.X >= minX && p.X < maxX && p.Y >= minY && p.Y < maxY)
                    .Select(p => DataLines[p.Y][p.X])
                    .Count(p => p == '@');

                if (sum < 4)
                    count++;
            }
        }

        return count;
    }

    public override object Solve2()
    {
        var hashSet = new HashSet<Position>();
        for (int y = 0; y < DataLines.Length;y++)
        for (int x = 0; x < DataLines[y].Length;x++)
        {
            if (DataLines[y][x] == '@')
            {
                hashSet.Add((x, y));
            }
        }

        var removedCount = 0;
        bool working = true;
        while (working)
        {
            working = false;

            foreach (var p in hashSet.ToList())
            {
                if (p.GetAllNeighbors().Count(hashSet.Contains) < 4)
                {
                    hashSet.Remove(p);
                    removedCount++;
                    working = true;
                }
            }

        }

        return removedCount;
    }

    public string testData = "..@@.@@@@.\n@@@.@.@.@@\n@@@@@.@.@@\n@.@@@@..@.\n@@.@@@@.@@\n.@@@@@@@.@\n.@.@.@.@@@\n@.@@@.@@@@\n.@@@@@@@@.\n@.@.@@@.@.";
}
