using System.Numerics;
namespace AdventOfCode
{
    public class Day_11 : BaseDay
    {
        public class Monkey{
            
            public List<int> items = new();
            public Func<int, int> operation;
            public int divisor;
            public int MonkeyIfTrue;
            public int MonkeyIfFalse;

            public void UpdateMonkey()
            {
                for (int i = 0; i < items.Count; i++)
                {
                    items[i] = operation(items[i]) / 3;
                }
            }

            public (int, int)? ThrowToWhichMonkey(int divisor)
            {
                if (items.Count == 0)
                    return null;

                var item = items.Dequeue();

                var worryLevel = operation(item)/divisor;
                if (worryLevel % this.divisor == 0)
                    return (MonkeyIfTrue, worryLevel);
                
                return (MonkeyIfFalse, worryLevel);
            }
        }

        public string testData = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

        public List<Monkey> ParseData()
        {
            var monkeysRaw = Data.Split("\n\n");
            
            List<Monkey> monkeys = new();

            foreach (var m in monkeysRaw)
            {
                var monkeySplit = m.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var currentMonkey = new Monkey();
                //items
                currentMonkey.items = monkeySplit[1].Split(":")[1].Split(",").ToList(x => int.Parse(x));

                currentMonkey.divisor = int.Parse(monkeySplit[3].Split(" ").Last());
                currentMonkey.MonkeyIfTrue = int.Parse(monkeySplit[4].Split(" ").Last());
                currentMonkey.MonkeyIfFalse = int.Parse(monkeySplit[5].Split(" ").Last());


                Func<int, int> a = (old) => old;
                Func<int, int> b = (old) => old;

                
                var operation = monkeySplit[2];

                var opsSplit = operation.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (opsSplit[3] == "old")
                    ;
                else
                {
                    var literalA = int.Parse(opsSplit[3]);
                    a = (old) => literalA;
                }

                if (opsSplit[5] == "old")
                    ;
                else
                {
                    var literalB = int.Parse(opsSplit[5]);
                    b = (old) => literalB;
                }
                

                Func<int, int> operand = (old) => a(old) + b(old);

                if (operation.Contains("*"))
                    operand = (old) => a(old) * b(old);
                
                currentMonkey.operation = operand; 

                monkeys.Add(currentMonkey);               
            }
            return monkeys;
        }

        public override object Solve1()
        {
            var monkeys = ParseData();
            var inspections = monkeys.ToList(x => 0);

            for (int i = 0; i < 20; i++)
            {
                
                foreach ((var currentMonkey, var currentMonkeyInt) in monkeys.Select((i,m) => (i,m)))
                {
                    while (currentMonkey.items.Count > 0)
                    {
                        var thrownItem = currentMonkey.ThrowToWhichMonkey(3);
                        (var toMonkey, var item) = thrownItem.Value;
                        monkeys[toMonkey].items.Add(item);
                        inspections[currentMonkeyInt]++;
                    }
                }
            }

            var monkeyBusiness = inspections.OrderByDescending().Take(2).ToList();

            return monkeyBusiness[0] * monkeyBusiness[1];

        }

        public override object Solve2()
        {
            var monkeys = ParseData();
            var inspections = monkeys.ToList(x => 0);
            var mod = monkeys.Select(x => x.divisor).Aggregate(1, (total, next) => total * next);

            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine($"{DateTime.Now}: {i})");
                foreach ((var currentMonkey, var currentMonkeyInt) in monkeys.Select((i,m) => (i,m)))
                {
                while (currentMonkey.items.Count > 0)
                    {
                        var thrownItem = currentMonkey.ThrowToWhichMonkey(1);
                        (var toMonkey, var item) = thrownItem.Value;
                        monkeys[toMonkey].items.Add(item % mod);
                        inspections[currentMonkeyInt]++;
                    }
                }
            }

            var monkeyBusiness = inspections.OrderByDescending().Take(2).ToList();

            return (int)monkeyBusiness[0] * (int)monkeyBusiness[1];
        }


    }
}
