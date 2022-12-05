namespace AdventOfCode
{
    public class Day_04 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            var assignments = Data.Split("\n")
                .Select(x => x.Split(","))
                .Select(x => (x[0], x[1]));
            
            int count = 0;
            foreach (var assignment in assignments)
            {   
                var a1 = assignment.Item1.Split("-");
                var a2 = assignment.Item2.Split("-");
                var a1List = Enumerable.Range(int.Parse(a1[0]), int.Parse(a1[1]) - int.Parse(a1[0])+1).ToList();
                var a2List = Enumerable.Range(int.Parse(a2[0]), int.Parse(a2[1]) - int.Parse(a2[0])+1).ToList();

                if (a1List.All(x => a2List.Contains(x)) || a2List.All(x => a1List.Contains(x)))
                    count++;

            }

            return new(count.ToString());

        }

        public override ValueTask<string> Solve_2()
        {
            var assignments = Data.Split("\n")
                .Select(x => x.Split(","))
                .Select(x => (x[0], x[1]));
            
            int count = 0;
            foreach (var assignment in assignments)
            {   
                var a1 = assignment.Item1.Split("-").ToList(int.Parse);
                var a2 = assignment.Item2.Split("-").ToList(int.Parse);
                var a1List = Enumerable.Range(a1[0], a1[1] - a1[0]+1).ToList();
                var a2List = Enumerable.Range(a2[0], a2[1] - a2[0]+1).ToList();

                if (a1List.Intersect(a2List).Any())
                    count++;

            }        

            return new(count.ToString());

        }
    }
}
