namespace AdventOfCode.Year_2019
{
    public class Day_02 : BaseDay
    {
        public List<int> GetSanitisedData() => Data
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        string sampleData = "1,9,10,3,2,3,11,0,99,30,40,50";

        string sampleData2 = "1,0,0,0,99";
        string sampleData3 = "2,3,0,3,99";
        string sampleData4 = "2,4,4,5,99,0";
        string sampleData5 = "1,1,1,4,99,5,6,0,99";
        
        public override ValueTask<string> Solve_1()
        {
            var data = GetSanitisedData().ToList();

            data[1] = 12;
            data[2] = 2;

            var result = new IntCodeComputer_02(data).Run();
            return new(result.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            int targetValue = 19690720;

            for (int noun = 0; noun <= 99; noun++)
            for (int verb = 0; verb <= 99; verb++)
            {
                var data = GetSanitisedData();
                data[1] = noun;
                data[2] = verb;

                var result = new IntCodeComputer_02(data).Run();
                if (result == targetValue)
                {
                    return new((100 * noun + verb).ToString());
                }
            }

            throw new Exception();
        }

        
    }

    public class IntCodeComputer_02
    {
        public IntCodeComputer_02(List<int> data)
        {
            this.Data = data;
        }

        List<int> Data;

        public int Run()
        {
            var pc = 0;

            while (true)
            {
                var instruction = (int)Data[pc];
                int first_addr;
                int second_addr;
                int dest_addr;
                switch (instruction)
                {
                    case 1:
                        first_addr = (int)Data[pc+1];
                        second_addr = (int)Data[pc+2];
                        dest_addr = (int)Data[pc+3];

                        Data[dest_addr] = Data[first_addr] + Data[second_addr];
                        break;
                    case 2:
                        first_addr = (int)Data[pc+1];
                        second_addr = (int)Data[pc+2];
                        dest_addr = (int)Data[pc+3];

                        Data[dest_addr] = Data[first_addr] * Data[second_addr];
                        break;
                    case 99:
                        goto end;
                    default:
                        throw new Exception();
                }

                pc += 4;
            }
            end:
                return Data[0];
        }
    }
}
