namespace AdventOfCode.Year_2019
{
    public class Day_03 : BaseDay
    {
        public Dictionary<string, (int x,int y)> dirMap = 
            new Dictionary<string, (int x, int y)>(StringComparer.OrdinalIgnoreCase)
            {
                {"U", (0, 1)},
                {"D", (0, -1)},
                {"L", (-1, 0)},
                {"R", (1, 0)}
            };

        string[] GetInputs => Data.Split("\n");

        List<(int x, int y)> VisitLocations(string[] inputs)
        {
            var visitedLocations = new List<(int x, int y)>();

            (int x, int y) location = (0,0);
            foreach (var i in inputs)
            {
                var dir = i[0].ToString();
                var dirInputs = dirMap[dir];
                
                var distance = int.Parse(i.Substring(1));

                for (int j = 0; j < distance; j++)
                {
                    location = (location.x + dirInputs.x, location.y + dirInputs.y);
                    visitedLocations.Add(location);
                }
            }

            return visitedLocations;
        }


        public override ValueTask<string> Solve_1()
        {
            var inputs = GetInputs;

            var inputs1 = inputs[0].Split(",");
            var inputs2 = inputs[1].Split(",");

            var visited1 = VisitLocations(inputs1);
            var visited2 = VisitLocations(inputs2);

            var closest = Enumerable.Intersect(visited1, visited2)
                .Min(l => Math.Abs(l.x) + Math.Abs(l.y));

            return new(closest.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var inputs = GetInputs;

            var inputs1 = inputs[0].Split(",");
            var inputs2 = inputs[1].Split(",");

            var visited1 = VisitLocations(inputs1);
            var visited2 = VisitLocations(inputs2);

            var visitedDistances1 = visited1.Select((p, steps) => (p, steps: steps+1))
                .GroupBy(k => k.p, v => v.steps)
                .ToDictionary(k => k.Key, v => v.Min());

            var visitedDistances2 = visited2.Select((p, steps) => (p, steps: steps+1))
                .GroupBy(k => k.p, v => v.steps)
                .ToDictionary(k => k.Key, v => v.Min());

            var intersections = Enumerable.Intersect(
                visited1, 
                visited2);

            var intersectionSteps = intersections.Select(p => visitedDistances1[p] + visitedDistances2[p]);

            return new(intersectionSteps.Min().ToString());
        }
    }
}
