namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    
    public class Day_04 : BaseDay
    {
        public override object Solve1()
        {
            var score = 0;
            foreach (var row in DataLines)
            {
                var splits = row.Split(':', '|');
                
                var winningNos = RegexUtils.GetInts(splits[1]);
                var cardNos = RegexUtils.GetInts(splits[2]);

                var matches = cardNos.Where(x => winningNos.Contains(x)).Count();
                if (matches > 0)
                    score += (int)Math.Pow(2, matches-1);
            }

            return score;
        }

        public override object Solve2()
        {
            return Solve2_LinearTime();
        }

        public object Solve2_LinearTime()
        {
            SortedDictionary<int, int> CardMatches = new();

            foreach (var row in DataLines)
            {
                var splits = row.Split(':', '|');
                var id = splits[0].GetInts().First();

                var winningNos = splits[1].GetInts();
                var cardNos = splits[2].GetInts();

                var matches = cardNos.Count(x => winningNos.Contains(x));
                CardMatches.Add(id, matches);
            }

            var counts = new int[CardMatches.Count + 1];
            
            foreach (var match in CardMatches.Reverse())
            {
                var id = match.Key;
                var matches = match.Value;
                if (match.Value == 0)
                    counts[id] = 1;
                else
                {
                    var start = id + 1;
                    var end = start + matches;
                    counts[id] = counts[start..end].Sum() + 1;
                }
            }

            return counts.Sum();
        }

        public object Solve2_NonLinearTime()
        {
            SortedDictionary<int, int> CardMatches = new();

            foreach (var row in DataLines)
            {
                var splits = row.Split(':', '|');
                var id = splits[0].GetInts().First();

                var winningNos = splits[1].GetInts();
                var cardNos = splits[2].GetInts();

                var matches = cardNos.Count(x => winningNos.Contains(x));
                CardMatches.Add(id, matches);
            }

            var toEvaluate = CardMatches.Keys.ToQueue();
            var count = 0;

            while (toEvaluate.TryDequeue(out var id))
            {
                var noMatches = CardMatches[id];
                toEvaluate.Enqueue(Enumerable.Range(id + 1, noMatches));
            }

            return count;
        }

        public string testData = @"";
    }
}
