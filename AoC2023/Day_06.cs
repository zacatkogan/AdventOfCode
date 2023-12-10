namespace AdventOfCode.AoC2023
{
    using AdventOfCode.Utils;

    public class Day_06 : BaseDay
    {
        string data = "Time:        60     80     86     76\nDistance:   601   1163   1559   1300";
        string data2 = "Time:        60808676\nDistance:   601116315591300";
        //string data2 = "Time:      71530\nDistance:  940200";


        //string data = "Time:      7  15   30\nDistance:  9  40  200";

        public override object Solve1()
        {
            var dataLines = data.Split("\n");

            var times = dataLines[0].GetInts();
            var dists = dataLines[1].GetInts();

            long margin = 1;

            foreach (var race in Enumerable.Zip(times, dists))
            {
                var dist = race.Second;
                var time = race.First;

                var wins = GetDistances(race.First).Count(x => x > dist);
                margin *= wins;
            }
               

            return margin;
        }

        public override object Solve2()
        {
            var dataLines = data2.Split("\n");

            var times = dataLines[0].GetLongs().First();
            var dists = dataLines[1].GetLongs().First();

            var d = GetDistancesLong(times).First(x => x.dist > dists);
            var wins = times - (2 * d.time) + 1;

            return wins;
        }


        IEnumerable<int> GetDistances(int time)
        {
            // wait for N seconds
            for (int i = 0; i <= time; i++)
            {
                yield return (time - i) * i;
            }
        }

        IEnumerable<(long time, long dist)> GetDistancesLong(long time)
        {
            // wait for N seconds
            for (long i = 0; i <= time; i++)
            {
                yield return (i, (time - i) * i);
            }
        }

    }
}
