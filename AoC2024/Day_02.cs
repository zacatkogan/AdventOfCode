using AdventOfCode.Utils;
namespace AdventOfCode.AoC2024
{
    public class Day_02 : BaseDay
    {
        Func<IEnumerable<int>, bool> checkIfSafe = (sequence) => (sequence.All(x => x > 0) || sequence.All(x => x < 0)) && sequence.All(x => Math.Abs(x) <= 3);
        
        public override object Solve1()
        {
            var lines = DataLines.Select(RegexUtils.GetInts);
            var safeLines = 0;
            var madeSafe = 0;

            foreach (var line in lines)
            {
                var numNums = line.Count;

                var second = line.Skip(1);
                var zip = line.Zip(second).Select(x => x.First - x.Second).ToList();

                if (checkIfSafe(zip))
                {
                    safeLines++;
                }
            }

            return safeLines;
        }

        public override object Solve2()
        {
            var lines = DataLines.Select(RegexUtils.GetInts);
            var safeLines = 0;
            var madeSafe = 0;

            foreach (var line in lines)
            {
                var numNums = line.Count;

                var second = line.Skip(1);
                var zip = line.Zip(second).Select(x => x.First - x.Second).ToList();

                if (checkIfSafe(zip))
                {
                    safeLines++;
                }
                else
                {
                    for (int i = 0; i < numNums; i++)
                    {
                        var tryFixed = line.ToList();
                        tryFixed.RemoveAt(i);
                        var secondSkip = tryFixed.Skip(1);
                        var tryZipped = tryFixed.Zip(secondSkip).Select(x => x.First - x.Second).ToList();
                        if (checkIfSafe(tryZipped))
                        {
                            madeSafe++;
                            break;
                        }
                    }
                }
            }

            return safeLines + madeSafe;
        }

        public string testData = @"";
    }
}
