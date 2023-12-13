namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using System.Text.RegularExpressions;

    public class Day_12: BaseDay
    {
        public override object Solve1()
        {
            return DataLines.Select(x => x.Split()).Select(x => ProcessString(x[0], x[1])).Sum();
        }

        public override object Solve2()
        {
            var expandedLines = DataLines.Select(Unfold).ToList();

            return expandedLines.Select(x => ProcessString(x.Item1, x.Item2)).Sum();
        }

        public (string s, string ints) Unfold(string s)
        {
            var splits = s.Split(' ');
            return (s: Unfold(splits[0], '?'), ints: Unfold(splits[1], ','));
        }

        public string Unfold(string s, char unfoldChar)
        {
            return string.Join(unfoldChar, Enumerable.Repeat(s, 5));
        }

        public string PopInt(string str, out int value)
        {
            if (string.IsNullOrEmpty(str))
            {
                value = 0;
                return string.Empty;
            }

            var firstComma = str.IndexOf(',');
            if (firstComma == -1)
            {
                value = int.Parse(str);
                return string.Empty;
            }

            var first = str.Substring(0, firstComma);
            var second = str.Substring(firstComma + 1);

            value = int.Parse(first);
            return second;
        }

        public Dictionary<(string, string), long> Cache = new();

        public long ProcessString(string input, string nums)
        {
            if (Cache.TryGetValue((input, nums), out long value))
                return value;

            var c = input.FirstOrDefault();

            long result = 0;
            if (c == '.')
                result = ProcessDot(input, nums);

            else if (c == '#')
                result = ProcessHash(input, nums);

            else if (c == '?')
                result = ProcessQuestion(input, nums);

            else if (c == default)
            {
                if (nums.Length > 0)
                {
                    // still numbers left that haven't been processed.
                    result = 0;
                }
                else
                {
                    result = 1;
                }
            }

            Cache.Add((input, nums), result);
            return result;
        }

        public long ProcessDot(string input, string nums)
        {
            return ProcessString(input[1..], nums);
        }

        public long ProcessHash(string input, string nums)
        {
            nums = PopInt(nums, out int length);

            if (length == 0)
                return 0;

            // check that we can take at least `length` maybe-springs
            var maybeSprings = input.TakeWhile(c => c == '#' || c == '?').Count();
            if (maybeSprings < length)
            {
                return 0;
            }
            else if (input.Length == length)
            {
                return ProcessString("", nums);
            }
            else if (input[length] == '#')
            {
                return 0;
            }

            // skip over any dots or potential `?` tokens
            input = input[(length + 1)..];

            return ProcessString(input, nums);
        }

        public long ProcessQuestion(string input, string nums)
        {
            var end = input[1..];
            var result1 = ProcessString("." + end, nums);
            var result2 = ProcessString("#" + end, nums);

            return result1 + result2;
        }
    }
}
