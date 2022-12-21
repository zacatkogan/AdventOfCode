using System.Text;

namespace AdventOfCode
{
    public class Day_21 : BaseDay
    {
        public override object Solve1()
        {
            var data = Data.Split("\n").Select(x => x.Replace(":", "").Split(" ").ToList()).ToList();

            Dictionary<string, long> seen = new();

            while(!seen.ContainsKey("root"))
            {
                foreach (var row in data)
                {
                    if (seen.ContainsKey(row[0]))
                        continue;

                    if (row.Count == 2)
                    {
                        seen.Add(row[0], long.Parse(row[1]));
                        continue;
                    }

                    if (seen.TryGetValue(row[1], out long l)
                        && seen.TryGetValue(row[3], out long r)) 
                    {
                        long result = 0;
                        switch(row[2])
                        {
                            case "+": result = l + r; break;
                            case "-": result = l - r; break;
                            case "*": result = l * r; break;
                            case "/": result = l / r; break;
                        }

                        seen.Add(row[0], result);
                    }
                }
            }

            return seen["root"];
        }

        public class Special
        {
            public Stack<Func<long, long>> Operations = new();
        }

        public override object Solve2()
        {
            var data = Data.Split("\n").Select(x => x.Replace(":", "").Split(" ").ToList()).ToList();
            var rootData = data.First(x => x[0] == "root");
            rootData[2] = "=";

            Dictionary<string, object> seen = new();

            while(!seen.ContainsKey("root"))
            {
                foreach (var row in data)
                {
                    if (seen.ContainsKey(row[0]))
                        continue;

                    if (row[0] == "humn")
                    {
                        seen.Add(row[0], new Special());
                        continue;
                    }

                    if (row.Count == 2)
                    {
                        seen.Add(row[0], long.Parse(row[1]));
                        continue;
                    }

                    if (seen.TryGetValue(row[1], out var left)
                        && seen.TryGetValue(row[3], out var right))
                    {
                        switch ((left, right))
                        {
                            case (long l, long r):
                                long result = 0;
                                switch(row[2])
                                {
                                    case "+": result = l + r; break;
                                    case "-": result = l - r; break;
                                    case "*": result = l * r; break;
                                    case "/": result = l / r; break;
                                }
                                seen.Add(row[0], result);
                                break;
                            case ((Special l, long r)):
                                switch(row[2])
                                {
                                    case "+": l.Operations.Push(o => o - r); break;
                                    case "-": l.Operations.Push(o => o + r); break;
                                    case "*": l.Operations.Push(o => o / r); break;
                                    case "/": l.Operations.Push(o => o * r); break;
                                    case "=": l.Operations.Push(o => r); break;
                                }
                                seen.Add(row[0], l);
                                break;
                            case ((long l, Special r)):
                                switch(row[2])
                                {
                                    case "+": r.Operations.Push(o => o - l); break; // a = b + c => c = a - b
                                    case "-": r.Operations.Push(o => l - o); break; // a = b - c => c = b - a
                                    case "*": r.Operations.Push(o => o / l); break; // a = b * c => c = a / b
                                    case "/": r.Operations.Push(o => l / o); break; // a = b / c => c = b / a
                                    case "=": r.Operations.Push(o => l); break;
                                }
                                seen.Add(row[0], r);
                                break;
                        }
                    }
                }
            }

            var root = (Special)seen["root"];
            {
                var l = seen[rootData[1]];
                var r = seen[rootData[3]];

                var sp = l as Special ?? r as Special;
                // var e = (l as long? ?? r as long?).Value;
                long e = 0;

                while (sp.Operations.TryPop(out var op))
                {
                    e = op(e);
                }

                return e;
            }

        }

        public string testData = @"root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32";
    }
}
