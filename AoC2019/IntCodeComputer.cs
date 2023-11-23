namespace AoC2019
{
    public class IntCodeComputer
    {
        private int PC;

        int GetValue(int value, bool mode)
        {
            if (!mode)
                return Data[value];
            else
                return value;
        }

        void SetValue(int address, int value)
        {
            Data[address] = value;
        }

        // Action<int[], int, int> SetValue = (data, address, value) =>
        // {
        //     var addressLoc = data[address];
        //     data[addressLoc] = value;
        // };

        // Func<int[], int, int> Add = (data, pc) => 
        // {
            
        //     return pc += 4;
        // };


        public void Add()
        {
            int arg1 = Data[PC+1];

            int arg2 = Data[PC+2];

            int arg3 = Data[PC+3];

            PC += 4;
        }

        public void Multiply()
        {
            var modes = Data[PC];

            int arg1 = Data[PC+1];
            bool mode1 = (modes & 0b100) != 0;
            int arg2 = Data[PC+2];
            bool mode2 = (modes & 0b1000) != 0;

            int arg3 = Data[PC+3];

            var value1 = GetValue(arg1, mode1);
            var value2 = GetValue(arg2, mode2);
            //int result = value;

            PC += 4;
        }


        public IntCodeComputer(List<int> data)
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
