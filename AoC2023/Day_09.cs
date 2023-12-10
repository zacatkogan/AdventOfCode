namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;

    public class Day_09: BaseDay
    {
        public override object Solve1()
        {
            //Data = "0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45";


            var extrapolatedValues = 0;

            foreach (var line in DataLines)
            {
                var nums = line.GetSignedInts();

                var histories = new List<List<int>>();
                histories.Add(nums.ToList());
                
                while (true)
                {
                    var thisLine = histories.Count;

                    // calculate the difference between elements in the previous line
                    var lastLine = histories[thisLine - 1];
                    var zipped = lastLine.Take(lastLine.Count - 1).Zip(lastLine.Skip(1));
                    var diffs = zipped.Select(x => x.Second - x.First).ToList();
                    histories.Add(diffs);

                    if (diffs.All(x => x == 0))
                        break;
                }

                var val = Extrapolate(histories);

                extrapolatedValues += val;
            }

            return extrapolatedValues;
        }

        public override object Solve2()
        {
            var extrapolatedValues = 0;

            foreach (var line in DataLines)
            {
                var nums = line.GetSignedInts();

                var histories = new List<List<int>>
                {
                    nums.ToList()
                };

                while (true)
                {
                    var thisLine = histories.Count;

                    // calculate the difference between elements in the previous line
                    var lastLine = histories[thisLine - 1];
                    var zipped = lastLine.Take(lastLine.Count - 1).Zip(lastLine.Skip(1));
                    var diffs = zipped.Select(x => x.Second - x.First).ToList();
                    histories.Add(diffs);

                    if (diffs.All(x => x == 0))
                        break;
                }

                var val = ExtrapolateBack(histories);

                extrapolatedValues += val;
            }

            return extrapolatedValues;
        }

        public int Extrapolate(List<List<int>> data)
        {
            var currentRate = 0;

            for (int i = data.Count; i > 0; i--)
            {
                var currentRow = i - 1;
                try
                {
                    var lastValue = data[currentRow].Last();
                    currentRate = lastValue + currentRate;
                }
                catch { }
            }

            return currentRate;
        }

        public int ExtrapolateBack(List<List<int>> data)
        {
            var currentRate = 0;

            for (int i = data.Count; i > 0; i--)
            {
                var currentRow = i - 1;
                try
                {
                    var lastValue = data[currentRow].First();
                    currentRate = lastValue - currentRate;
                }
                catch { }
            }

            return currentRate;
        }
    }
}
