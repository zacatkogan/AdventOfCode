namespace AdventOfCode.Year_2019
{
    public class Day_01 : BaseDay
    {
        public string[] SanitisedData => Data.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        public override ValueTask<string> Solve_1()
        {
            var result = SanitisedData.Select(int.Parse)
                .Sum(x => x/3 - 2);

            return new(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var split = SanitisedData;

            int totalFuel = 0;

            int getFuelMass(int mass)
            {
                return Math.Max(mass/3-2, 0);
            }

            foreach (var s in split)
            {

                var initalMass = int.Parse(s);
                var currentMass = initalMass;
                var fuel = 0;
                while (true)
                {
                    currentMass = getFuelMass(currentMass);
                    if (currentMass == 0)
                        break;
                    
                    fuel += currentMass;
                }
                totalFuel += fuel;
            }
            Console.Write(totalFuel);
            return new(totalFuel.ToString());
        }
    
    }
}
