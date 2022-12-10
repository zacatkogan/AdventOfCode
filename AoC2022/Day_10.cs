namespace AdventOfCode
{
    public class Day_10 : BaseDay
    {
        public override ValueTask<string> Solve_1()
        {
            var instructions = Data.Split("\n");
            var cpu = new CPU(instructions);

            long strength = 0;
            cpu.Tick(19);
            strength += cpu.X * 20;
            cpu.Tick(40); // 60
            strength += cpu.X * 60;
            cpu.Tick(40); // 100
            strength += cpu.X * 100;
            cpu.Tick(40); // 140
            strength += cpu.X * 140;
            cpu.Tick(40);
            strength += cpu.X * 180;
            cpu.Tick(40); // 220
            strength += cpu.X * 220;

            return new(strength.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            CPU cpu = new CPU(Data.Split("\n"));

            for(int j = 0; j < 6; j++)
            {
                var gpu = Enumerable.Repeat(".", 40).ToArray();

                for (int i = 0; i < 40; i++)
                {
                    if (i+1 >= (cpu.X) && i+1 <= (cpu.X+2))
                        gpu[i] = "#";
                        
                    cpu.Tick(1);
                }

                Console.WriteLine(string.Join("", gpu));
            }

            return new("");
        }

        class CPU
        {
            public int X = 1;
            public int PC = 0;

            string[] instructions;

            public CPU(string[] instructions)
            {
                this.instructions = instructions;
                ticksEnumerator = this.Tick().GetEnumerator();
            }

            IEnumerator<bool> ticksEnumerator;

            public void Tick(int count)
            {
                for (int i = 0; i < count; i++)
                    ticksEnumerator.MoveNext();
            }

            public IEnumerable<bool> Tick()
            {
                while (true)
                {
                    var instruction = instructions[PC];

                    if (instructions[PC] == "noop")
                    {
                        PC++;
                        yield return true;
                    }
                    else
                    {
                        var inc = int.Parse(instructions[PC].Split(" ")[1]);
                        foreach (var r in AddX(inc))
                            yield return r;     
                    }
                }
            }

            IEnumerable<bool> AddX(int x)
            {
                yield return false;
                X += x;
                PC++;
                yield return true;

            }
        }
    }
}
