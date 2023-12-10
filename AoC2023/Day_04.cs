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
            SortedDictionary<int, int> CardMatches = new();

            foreach (var row in DataLines)
            {
                var splits = row.Split(':', '|');
                var id = splits[0].GetInts().First();

                var winningNos = splits[1].GetInts();
                var cardNos = splits[2].GetInts();

                var matches = cardNos.Where(x => winningNos.Contains(x)).Count();
                CardMatches.Add(id, matches);
            }

            var toEvaluate = CardMatches.Keys.ToQueue();
            var counts = new int[CardMatches.Count + 1];
            
            while (toEvaluate.TryDequeue(out var id))
            {
                counts[id] += 1;

                var noMatches = CardMatches[id];
                toEvaluate.Enqueue(Enumerable.Range(id + 1, noMatches));
            }

            return counts.Sum();
        }

        public string testData = @"";
    }
}
