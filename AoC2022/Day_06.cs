namespace AdventOfCode
{
    public class Day_06 : BaseDay
    {
        public int DoThing(int length) => Data.Select((c, i) => (Data.Skip(i).Take(length).Distinct().Count() == length, i)).First(x => x.Item1).i + length;

        public override ValueTask<string> Solve_1()
        {
            return new(DoThing(4).ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new(DoThing(14).ToString());
        }
    }
}
