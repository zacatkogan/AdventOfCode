namespace AdventOfCode.AoC2025;

public class Day_03 : BaseDay
{
    public override object Solve1()
    {
        var parsedDataLines = DataLines.Select(line => line.Select(c => int.Parse(c.ToString())).ToList()).ToList();

        var total = 0;

        foreach (var line in parsedDataLines)
        {
            var highest = 0;
            for (int i = 0; i < line.Count - 1; i++)
            for (int j = i + 1; j < line.Count; j++)
                {
                    var jolt = line[i] * 10 + line[j];
                    if (jolt > highest) 
                        highest = jolt;
                }
            
            total += highest;
        }

        return total;
    }

    public override object Solve2()
    {
        var parsedDataLines = DataLines.Select(line => line.Select(c => int.Parse(c.ToString())).ToList()).ToList();
        return parsedDataLines.Select(GetHighestNumber).Sum();        
    }

    public long GetHighestNumber(List<int> jolt)
    {
        var digitIndexes = Enumerable.Range(jolt.Count - 12, 12).ToList();

        for (int i = 0; i < 12; i++)
        {
            // move backwards from current index towards 0, not surpassing the index of the prev number.
            // if the value of the prev index is equal or higher, replace the current index with that index.
            int minIndex;
            
            if (i == 0)
            {
                minIndex = 0;
            }
            else
            {
                minIndex = digitIndexes[i-1] + 1;
            }

            var currentIndex = digitIndexes[i];
            var currentValue = jolt[currentIndex];

            int j = currentIndex - 1;
            while (j >= minIndex)
            {
                var newValue = jolt[j];
                if (newValue >= currentValue)
                {
                    currentValue = newValue;
                    currentIndex = j;
                }

                j--;
            }

            digitIndexes[i] = currentIndex;
        }

        var numberString = string.Join("", digitIndexes.Select(x => jolt[x].ToString()));
        return long.Parse(numberString);

    }

    public string testData = @"";
}
