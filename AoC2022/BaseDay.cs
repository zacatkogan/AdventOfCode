namespace AdventOfCode
{
    public abstract class BaseDay : AdventOfCode.BaseDayWithData
    {
        public override int GetProblemYear() => 2022;

        public override ValueTask<string> Solve_1() => new(Solve1().ToString());
        public override ValueTask<string> Solve_2() => new(Solve2().ToString());

        public virtual object Solve1() => throw new NotImplementedException();
        public virtual object Solve2() => throw new NotImplementedException();
    }
}
