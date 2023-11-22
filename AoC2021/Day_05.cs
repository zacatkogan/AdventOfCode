using System.Net.WebSockets;

namespace AdventOfCode.AoC2021
{
	public class Day_05 : BaseDay
	{
		public override object Solve1()
		{
			// part 1:
            var vents = new int[1000, 1000];

            // only straight vents for part 1
            foreach (var vent in coords.Where(v => v.start.X == v.end.X || v.start.Y == v.end.Y))
            {
                var deltax = vent.end.X - vent.start.X;
                var deltay = vent.end.Y - vent.start.Y;

                for (int i = 0; Math.Abs(i) <= Math.Abs(deltax); i = (deltax >= 0 ? i + 1 : i - 1))
                    for (int j = 0; Math.Abs(j) <= Math.Abs(deltay); j = (deltay >= 0 ? j + 1 : j - 1))
                    {
                        vents[vent.start.X + i, vent.start.Y + j]++;
                    }
            }

            var sum = 0;

            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                {
                    if (vents[i, j] > 1)
                        sum++;
                }

            return sum;
        }

		public override object Solve2()
		{
            var vents2 = new int[1000, 1000];


            foreach (var vent in coords)
            {
                var deltax = vent.end.X - vent.start.X;
                var deltay = vent.end.Y - vent.start.Y;

                if (deltax == 0 || deltay == 0)
                {
                    for (int i = 0; Math.Abs(i) <= Math.Abs(deltax); i = (deltax >= 0 ? i + 1 : i - 1))
                        for (int j = 0; Math.Abs(j) <= Math.Abs(deltay); j = (deltay >= 0 ? j + 1 : j - 1))
                        {
                            vents2[vent.start.X + i, vent.start.Y + j]++;
                        }
                }
                else if (Math.Abs(deltax) == Math.Abs(deltay))
                {
                    int xdir = deltax < 0 ? -1 : 1;
                    int ydir = deltay < 0 ? -1 : 1;

                    for (int i = 0; i <= Math.Abs(deltax); i++)
                    {
                        try
                        {
                            vents2[vent.start.X + (xdir * i), vent.start.Y + (ydir * i)]++;
                        }
                        catch
                        { }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            var sum2 = 0;

            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                {
                    if (vents2[i, j] > 1)
                        sum2++;
                }

            return sum2;
        }

        (Position start, Position end) ParseCoords(string input)
		{
			var raw = input.Split(" ");

			var rawStart = raw[0].Split(",");
			var rawEnd = raw[2].Split(",");

			return ((int.Parse(rawStart[0]), int.Parse(rawStart[1])), (int.Parse(rawEnd[0]), int.Parse(rawEnd[1])));
		}

		public IEnumerable<(Position start, Position end)> coords => DataLines.Select(ParseCoords).ToList();
	}
}











// only straight vents for part 1
