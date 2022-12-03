namespace AdventOfCode
{
    public class Day_03 : BaseDay
    {
        int GetPriority(char c)
        {
            if (Char.IsUpper(c))
                return 27 + (c-'A');
            else
                return 1 + (c-'a');
        }

        public override ValueTask<string> Solve_1()
        {
            var elves = Data.Split("\n");
            var splits = elves.Select(x => x.ToArray())
                .Select(x => (x[0..(x.Length/2)], x[(x.Length/2)..])).ToList();

            var sharedType = splits.Select(x => Enumerable.Intersect(x.Item1, x.Item2)).ToList();

            var priorities = splits
                .Select(x => GetPriority(Enumerable.Intersect(x.Item1, x.Item2).Single())).ToList();
            
            return new(priorities.Sum().ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var elves = Data.Split("\n");
            var result = elves.Chunk(3)
                .Select(x => x.IntersectMany().Single())
                .Sum(GetPriority);

            return new(result.ToString());
        }

        public object Solve_1_Alec()
        {
            return Data.Split("\n").Select(x => (x[..(x.Length/2)].Intersect(x[(x.Length/2)..]).Single()-98) % 56).Sum();
        }

        public object Solve_2_Alec()
        {
            return Data.Split("\n").Chunk(3).Select(x => x.IntersectMany().Single()).Sum(x => (x-96)%58);
        }

    }
}
