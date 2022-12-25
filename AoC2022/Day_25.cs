namespace AdventOfCode
{
    public class Day_25 : BaseDay
    {
        public long ConvertToInt(string input)
        {
            long accumulator = 0;
            long result = 0;

            foreach (var c in input)
            {
                long val = 0;
                switch (c)
                {
                    case '1': val = 1; break;
                    case '2': val = 2; break;
                    case '-': val = -1; break;
                    case '=': val = -2; break;
                }

                accumulator += val;
                result = accumulator;
                accumulator *= 5;
            }

            return result;
        }

        public string RemainderToChar(long remainder)
        {
            switch (remainder)
            {
                case -2: return "=";
                case -1: return "-";
                case 0:
                case 1:
                case 2:
                    return remainder.ToString();
                default: throw new ArgumentOutOfRangeException();

            }
        }

        public string ConvertToSnafu(long input)
        {
            var t = input + 2;
            var result = t / 5;
            var remainder = (t % 5) - 2;

            if (result > 0)
                return ConvertToSnafu(result) + RemainderToChar(remainder);

            else
                return RemainderToChar(remainder);
        }

        public override object Solve1()
        {
            var sumInt = Data.Split("\n").Sum(x => ConvertToInt(x));

            return ConvertToSnafu(sumInt);
        }

        public override object Solve2()
        {
            return "Merry Christmas!";
        }

        public string testData = @"1=-0-2
12111
2=0=
21
2=01
111
20012
112
1=-1=
1-12
12
1=
122";
    }
}
