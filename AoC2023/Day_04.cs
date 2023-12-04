namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using MathNet.Numerics;
    using System.Runtime.InteropServices;
    using System.Security.Principal;

    public class Day_04 : BaseDay
    {
        public override object Solve1()
        {
            var score = 0;
            foreach (var row in DataLines)
            {
                var id = row.Split(":").First();
                var rest = row.Split(":").Last();

                var nos = rest.Split("|");
                var winningNos = RegexUtils.GetInts(nos[0]);
                var cardNos = RegexUtils.GetInts(nos[1]);

                var matches = cardNos.Where(x => winningNos.Contains(x)).Count();
                if (matches > 0)
                    score += (int)Math.Pow(2, matches-1);
            }

            return score;

        }

        public override object Solve2()
        {
            SortedDictionary<int, int> CardMatches = new();

            foreach (var row in DataLines)
            {
                var idRaw = row.Split(":").First();
                var id = RegexUtils.GetInts(idRaw).First();

                var rest = row.Split(":").Last();

                var nos = rest.Split("|");
                var winningNos = RegexUtils.GetInts(nos[0]);
                var cardNos = RegexUtils.GetInts(nos[1]);

                var matches = cardNos.Where(x => winningNos.Contains(x)).Count();
                CardMatches.Add(id, matches);
            }

            var toEvaluate = new Queue<int>();
            Dictionary<int, int> Counts = CardMatches.Keys.ToDictionary(x => x, x => 0);

            foreach (var match in CardMatches)
            {
                toEvaluate.Enqueue(match.Key);
            }
            
            while (toEvaluate.TryDequeue(out var id))
            {
                Counts[id] += 1;

                var noMatches = CardMatches[id];
                for (int i = 1; i <= noMatches; i++)
                {
                    toEvaluate.Enqueue(id + i);
                }
            }

            return Counts.Sum(x => x.Value);
        }

        public string testData = @"";
    }
}
