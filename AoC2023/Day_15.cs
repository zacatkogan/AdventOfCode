namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;

    public class Day_15: BaseDay
    {
        public override object Solve1()
        {
            return Data.Split(',').Select(HashString).Sum();
        }

        public int HashString(string value)
        {
            int currentValue = 0;

            foreach (char c in value)
            {
                if (c == '\n' || c == '\r') continue;

                currentValue += (int)c;
                currentValue *= 17;
                currentValue %= 256;
            }

            return currentValue;
        }

        public class Lens
        {
            public string Id;
            public int Strength;
        }


        public override object Solve2()
        {
            //Data = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";
            var instructions = Data.Split(',');

            var boxes = Enumerable.Range(0, 256).Select(x => new List<Lens>()).ToList();
            
            
            foreach (var instruction in instructions)
            {
                var opIndex = instruction.IndexOfAny(new[] { '=', '-' });
                var operation = instruction[opIndex];
                var lensId = instruction[..opIndex];
                var boxId = HashString(lensId);
                
                
                if (operation == '=')
                {
                    // insert
                    var lensStrength = instruction[(opIndex + 1)..];
                    var box = boxes[boxId];
                    var existingLens = box.FirstOrDefault(x => x.Id == lensId);
                    if (existingLens != null)
                    {
                        existingLens.Strength = int.Parse(lensStrength);
                        continue;
                    }

                    boxes[boxId].Push(new Lens { Id = lensId, Strength = int.Parse(lensStrength) });
                }
                if (operation == '-')
                {
                    // remove
                    var box = boxes[boxId];
                    var boxFilter = box.Where(x => x.Id == lensId);
                    if (boxFilter.Any())
                    {
                        var lens = boxFilter.First();
                        box.Remove(lens);
                    }
                    
                }
            }
            return CalculateFocusPower(boxes);

            return "";
        }

        long CalculateFocusPower(List<List<Lens>> boxes)
        {
            long boxTotal = 0;


            for (int i = 0; i < boxes.Count; i++)
            {
                var box = boxes[i];
                for (int j = 0; j < box.Count; j++)
                {
                    boxTotal += (i + 1) * (j + 1) * box[j].Strength;
                }
            }

            return boxTotal;
        }
    }
}
