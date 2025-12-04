using MathNet.Numerics.Distributions;
using System.Runtime.CompilerServices;

namespace AdventOfCode.AoC2025;

public class Day_02 : BaseDay
{
    public struct RangeLong(long start, long finish)
    {
        public readonly long Start => start;
        public readonly long Finish => finish;

        public bool Contains(long value)
        {
            return value >= start && value <= finish;
        }
    }

    private IEnumerable<RangeLong> SplitRangeOnSize(RangeLong rangeLong)
    {
        var startDigitCount = rangeLong.Start.ToString().Length;
        var endDigitCount = rangeLong.Finish.ToString().Length;

        if (startDigitCount == endDigitCount)
        {
            yield return rangeLong;
            yield break;
        }

        var currentLength = startDigitCount;
        var rangeStartValue = rangeLong.Start;

        while (currentLength < endDigitCount)
        {
            var maxDigitThisLength = long.Parse(new string(Enumerable.Repeat('9', currentLength).ToArray()));
            yield return new RangeLong(rangeStartValue, maxDigitThisLength);
            rangeStartValue = maxDigitThisLength + 1;
            currentLength++;
        }

        yield return new RangeLong(rangeStartValue, rangeLong.Finish);
    }

    


    public override object Solve1()
    {
        var splitData = Data.Split(",");
        var ranges = splitData.Select(x => x.Split("-")).Select(y => new RangeLong(long.Parse(y[0]), long.Parse(y[1]))).ToList();

        long sum = 0;

        var splitRanges = ranges.SelectMany(x => SplitRangeOnSize(x)).ToList();
        
        foreach (var range in splitRanges)
        {
            sum += GetDuplicatesInRange(range);
        }

        return sum;
    }

    public long GetDuplicatesInRange(RangeLong range)
    {
        var startAsString = range.Start.ToString();
        var finishAsString = range.Finish.ToString();
        var numDigits = range.Start.ToString().Length;

        var sum = 0L;

        // this breaks down when a range goes from one magnitude to another - e.g. 2 digits to 3 digits

        var (div, rem) = Math.DivRem(numDigits, 2);
        if (rem != 0)
            return 0; // not divisible number of digits. e.g.: checking for 2 digit repeaters in a 3 digit number

        var startDigits = long.Parse(startAsString[..div]);
        var endDigits = long.Parse(finishAsString[..div]);

        var currentDigits = startDigits;
        while (currentDigits <= endDigits)
        {
            var repeatedNumber = long.Parse(string.Join("", Enumerable.Repeat(currentDigits.ToString(), 2)));
            if (range.Contains(repeatedNumber))
            {
                sum += repeatedNumber;
            }

            currentDigits++;
        }

        return sum;
    }

    public long GetDuplicatesInRangePart2(RangeLong range)
    {
        var startAsString = range.Start.ToString();
        var finishAsString = range.Finish.ToString();
        var totalNumDigits = range.Start.ToString().Length;

        HashSet<long> longs = new HashSet<long>();

        // this breaks down when a range goes from one magnitude to another - e.g. 2 digits to 3 digits

        for (var numDigits = 1; numDigits <= totalNumDigits / 2; numDigits++)
        {
            var (div, rem) = Math.DivRem(totalNumDigits, numDigits);
            if (rem != 0)
                continue; // not divisible number of digits. e.g.: checking for 2 digit repeaters in a 3 digit number

            var startDigits = long.Parse(startAsString[..numDigits]);
            var endDigits = long.Parse(finishAsString[..numDigits]);

            var currentDigits = startDigits;
            while (currentDigits <= endDigits)
            {
                var repeatedNumber = long.Parse(string.Join("", Enumerable.Repeat(currentDigits.ToString(), div)));
                if (range.Contains(repeatedNumber))
                {
                    longs.Add(repeatedNumber);
                }

                currentDigits++;
            }
        }

        return longs.Sum();
    }

    public override object Solve2()
    {
        var splitData = Data.Split(",");
        var ranges = splitData.Select(x => x.Split("-")).Select(y => new RangeLong(long.Parse(y[0]), long.Parse(y[1]))).ToList();

        long sum = 0;

        var splitRanges = ranges.SelectMany(x => SplitRangeOnSize(x)).ToList();

        foreach (var range in splitRanges)
        {
            sum += GetDuplicatesInRangePart2(range);
        }

        return sum;
    }

    public string testData = @"";
}