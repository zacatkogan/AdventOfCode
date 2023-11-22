namespace AdventOfCode.AoC2021
{
	public class Day_06 : BaseDay
	{
        public override object Solve1()
        {
            // part 1
            List<byte> LanternFish = source.Select(x => (byte)x).ToList();

            try
            {
                for (int d = 0; d < 80; d++)
                {
                    int count = LanternFish.Count;

                    for (int i = 0; i < count; i++)
                    {
                        var fish = LanternFish[i];
                        if (fish == 0)
                        {
                            LanternFish[i] = 6;
                            LanternFish.Add(8);
                        }
                        else
                        {
                            LanternFish[i]--;
                        }
                    }
                }
                return LanternFish.Count;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        // part 2: bucketize and iterate
        public override object Solve2()
        {
            var lanternFishGrouped = source.GroupBy(x => x).ToDictionary(x => x.Key, x => (long)x.Count());

            for (int d = 0; d < 256; d++)
            {
                // decrement...
                var tmp = lanternFishGrouped.ToDictionary(x => x.Key - 1, x => x.Value);

                // check for "new fish"
                if (tmp.TryGetValue(-1, out long newFish))
                {
                    tmp.Remove(-1);
                    if (tmp.ContainsKey(6))
                        tmp[6] = tmp[6] + newFish;
                    else
                        tmp.Add(6, newFish);
                    tmp.Add(8, newFish);
                }

                lanternFishGrouped = tmp;
            }

            return lanternFishGrouped.Sum(x => x.Value);
        }

		IEnumerable<int> source => Data.Split(",").Select(int.Parse);
    }
}