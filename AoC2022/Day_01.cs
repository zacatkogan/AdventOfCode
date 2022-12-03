using AdventOfCode;

namespace AdventOfCode
{
    public class Day_01 : BaseDay
    {
        public string[] SanitisedData => Data.Split('\n', StringSplitOptions.None);

        public string Solve_1_Reattempt()
        {
            var result = Data.Split("\n")
                .SplitOnElement("")
                .Select(x => x.Sum(int.Parse))
                .Max();

            return result.ToString();
        }

        public string Solve_2_Reattempt()
        {
            var result = Data.Split("\n")
                .SplitOnElement("")
                .Select(x => x.Sum(int.Parse))
                .OrderByDescending()
                .Take(3)
                .Sum();

            return result.ToString();
        }

        public override ValueTask<string> Solve_1()
        {
            return new(Solve_1_Reattempt());


            var current_max = 0;

            var current_elf = 0;
            foreach (var x in SanitisedData)
            {
                if (string.IsNullOrWhiteSpace(x))
                {
                    if (current_elf > current_max)
                        current_max = current_elf;
                    current_elf = 0;
                    continue;
                }

                current_elf += int.Parse(x);
            }

            return new(Math.Max(current_max, current_elf).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new(Solve_2_Reattempt());

            List<int> elves = new();
            var current_elf = 0;

            foreach (var x in SanitisedData)
            {
                if (string.IsNullOrWhiteSpace(x))
                {
                    elves.Add(current_elf);
                    current_elf = 0;
                    continue;
                }

                current_elf += int.Parse(x);
            }

            var result = elves.OrderByDescending(x => x).Take(3).Sum();
            return new(result.ToString());
        }
    
    }
}
