namespace AdventOfCode.AoC2023
{
    using AdventOfCode;

    public class Day_01: BaseDay
    {
        public override object Solve1()
        {
            return DataLines.Select(x => x.Where(c => int.TryParse(c.ToString(), out _))
                .Select(c => int.Parse(c.ToString())).ToList())
                .ToList()
                .Select(l => l.First() * 10 + l.Last())
                .Sum();
        }

        public override object Solve2()
        {
            return DataLines.Select(TextToDigits)
                .Select(x => 
                    x.Where(c => int.TryParse(c.ToString(), out _))
                        .Select(c => int.Parse(c.ToString()))
                        .ToList())
                .ToList()
                .Select(l => l.First() * 10 + l.Last())
                .Sum();
        }

        public string TextToDigits(string s)
        {
            return s.Replace("one", "o1e")
              .Replace("two", "t2o")
              .Replace("three", "t3e")
              .Replace("four", "f4r")
              .Replace("five", "f5e")
              .Replace("six", "s6x")
              .Replace("seven", "s7n")
              .Replace("eight", "e8t")
              .Replace("nine", "n9e");
        }

        string TestData = "two1nine\r\neightwothree\r\nabcone2threexyz\r\nxtwone3four\r\n4nineeightseven2\r\nzoneight234\r\n7pqrstsixteen";
    }
}
