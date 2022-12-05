namespace AdventOfCode
{
    public class Day_05 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            var stacks = Enumerable.Range(0,9).ToList(x => new Stack<string>());

            var data = Data.Split("\n\n");
            
            var rawCrates = data[0].Split("\n");

            foreach(var crateStacks in rawCrates.Reverse().Skip(1))
                for (int i = 0; i < 9; i++)
                {
                    var crateId = crateStacks[i* 4+1].ToString();
                    if (!string.IsNullOrWhiteSpace(crateId))
                        stacks[i].Push(crateId);
                }

            var instructions = data[1].Split("\n");
            foreach (var instruction in instructions)
            {
                var instSplit = instruction.Split(" ");

                var count = int.Parse(instSplit[1].ToString());

                var source = int.Parse(instSplit[3].ToString())-1;
                var dest = int.Parse(instSplit[5].ToString())-1;

                for (int j = 0; j < count; j++)
                {
                    stacks[dest].Push(stacks[source].Pop());
                }
            }

            var result = string.Join("", stacks.SelectMany(x => x.Pop()));

            return new(result);


            //return new(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var stacks = Enumerable.Range(0,9).ToList(x => new Stack<string>());

            var data = Data.Split("\n\n");
            
            var rawCrates = data[0].Split("\n");

            foreach(var crateStacks in rawCrates.Reverse().Skip(1))
                for (int i = 0; i < 9; i++)
                {
                    var crateId = crateStacks[i* 4+1].ToString();
                    if (!string.IsNullOrWhiteSpace(crateId))
                        stacks[i].Push(crateId);
                }

            var instructions = data[1].Split("\n");
            foreach (var instruction in instructions)
            {
                var instSplit = instruction.Split(" ");

                var count = int.Parse(instSplit[1].ToString());

                var source = int.Parse(instSplit[3].ToString())-1;
                var dest = int.Parse(instSplit[5].ToString())-1;

                var temp = new Stack<string>();
                for (int j = 0; j < count; j++)
                {
                    temp.Push(stacks[source].Pop());
                }

                for (int j = 0; j < count; j++)
                {
                    stacks[dest].Push(temp.Pop());
                }
            }

            var result = string.Join("", stacks.SelectMany(x => x.Pop()));

            return new(result);
        }
    }
}
