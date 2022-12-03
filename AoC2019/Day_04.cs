namespace AdventOfCode.Year_2019
{
    public class Day_04 : BaseDay
    {

        private bool ContainsDuplicates(string[] s)
        {
            return Enumerable.Zip(s.Take(5), s.Skip(1))
                .Any(x => x.First == x.Second);
        }

        private bool AllEqualOrIncreasing(string[] s)
        {
            var i = s.Select(x => int.Parse(x));

            return Enumerable.Zip(i.Take(5), i.Skip(1))
                .All(x => x.First <= x.Second);
        }

        private bool AtLeastOneIsolatedDouble(string[] s)
        {
            return Enumerable.Zip(s.Take(5), s.Skip(1))
                .Where(x => x.First == x.Second)
                .GroupBy(x => x.First)
                .Any(x => x.Count() == 1);
                
        }

        public override ValueTask<string> Solve_1()
        {
            var inputRange = Data.Split("-");
            int lowerBound = int.Parse(inputRange[0]);
            int upperBound = int.Parse(inputRange[1]);

            var result = Enumerable.Range(lowerBound, (upperBound-lowerBound))
                .Select(x => x.ToString().ToCharArray().Select(x => x.ToString()).ToArray())
                .Where(x => ContainsDuplicates(x) && AllEqualOrIncreasing(x))
                .Count();
            return new(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var inputRange = Data.Split("-");
            int lowerBound = int.Parse(inputRange[0]);
            int upperBound = int.Parse(inputRange[1]);

            var result = Enumerable.Range(lowerBound, (upperBound-lowerBound))
                .Select(x => x.ToString().ToCharArray().Select(x => x.ToString()).ToArray())
                .Where(x => 
                    ContainsDuplicates(x) 
                    && AllEqualOrIncreasing(x)
                    && AtLeastOneIsolatedDouble(x))
                .Count();
            return new(result.ToString());        }
    }
}
