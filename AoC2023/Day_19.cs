namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;

    public class Day_19 : BaseDay
    {


        #region Part1
        public override object Solve1()
        {
            var splits = DataLines.SplitOnElement(string.Empty).ToList();
            var workflows = splits[0].Select(Workflow.Parse).ToDictionary(x => x.Name);
            var parts = splits[1].Select(Part.Parse).ToList();

            var accepted = new List<Part>();

            foreach (var part in parts)
            {
                var workflow = workflows["in"];

                while (true)
                {
                    var newWorkflow = ProcessRules(workflow.Rules, part);

                    if (newWorkflow == "A")
                    {
                        accepted.Add(part);
                        break;
                    }
                    if (newWorkflow == "R")
                        break;
                    
                    workflow = workflows[newWorkflow];
                }

            }

            return accepted.Sum(x => x.Sum);
        }

        public struct Part
        {
            public int x;
            public int m;
            public int a;
            public int s;

            public static Part Parse(string partRaw)
            {
                var ints = partRaw.GetInts();
                return new Part
                {
                    x = ints[0],
                    m = ints[1],
                    a = ints[2],
                    s = ints[3],
                };
            }

            public int Sum => x + m + a + s;
        }

        public static string ProcessRules(IEnumerable<Rule> rules, Part part)
        {
            foreach (var rule in rules)
            {
                if (string.IsNullOrEmpty(rule.Comparison))
                    return rule.Outcome;

                var element = GetValue(rule.Comparison[0], part);
                var comparer = GetComparer(rule.Comparison[1]);
                var value = int.Parse(rule.Comparison[2..]);

                if (comparer(element, value))
                    return rule.Outcome;
            }

            return null;
        }

        public static int GetValue(char c, Part part)
        {
            switch (c)
            {
                case 'x': return part.x;
                case 'm': return part.m;
                case 'a': return part.a;
                case 's': return part.s;
                default: throw new Exception();
            };
        }

        public static Func<int, int, bool>GetComparer(char c)
        {
            if (c == '<')
                return LessThan;
            if (c == '>')
                return GreaterThan;

            throw new Exception();
        }

        public static Func<int, int, bool> LessThan = (a, b) => a < b;
        public static Func<int, int, bool> GreaterThan = (a, b) => a < b;
        #endregion

        #region Part2
        public override object Solve2()
        {
            var splits = DataLines.SplitOnElement(string.Empty).ToList();
            var workflows = splits[0].Select(Workflow.Parse).ToDictionary(x => x.Name);

            var accepted = new List<PartRange>();

            Queue<(PartRange, string)> queue = new();
            List<(string workflow,(PartRange, string))> processed = new();
            queue.Enqueue((PartRange.Default, "in"));

            while (queue.TryDequeue(out var partWorkflow))
            {
                var part = partWorkflow.Item1;
                var workflow = workflows[partWorkflow.Item2];

                var results = ProcessRules(workflow.Rules, part).ToList();

                foreach (var result in results)
                {
                    if (result.Item2 == "A")
                    {
                        accepted.Add(result.Item1);
                        continue;
                    }
                    if (result.Item2 == "R")
                        continue;
                    queue.Enqueue(result);
                }

                processed.AddRange(results.Select(x => (workflow.Name, x)));
            }

            accepted = accepted.OrderBy(p => p.x.Start).ThenBy(p => p.m.Start).ThenBy(p => p.a.Start).ThenBy(p => p.s.Start).ToList();
            return accepted.Sum(x => x.Size);

            // 167409079868000 is the Sample

            // 404904760892901 is the Sample output so far...
            // 404916397387050 is the Sample output so far...
            // 405650725000000
            // 390382394793600 is too high
        }

        public struct PartRange
        {
            public override string ToString()
            {
                return $"x:{x}, m:{m}, a:{a}, s:{s}";
            }

            public static PartRange Default => new PartRange
            {
                x = new Interval(1, 4001),
                m = new Interval(1, 4001),
                a = new Interval(1, 4001),
                s = new Interval(1, 4001),
            };

            public Interval x;
            public Interval m;
            public Interval a;
            public Interval s;

            public (PartRange, PartRange) Split(char element, int value)
            {
                var lower = this;

                var upper = this;

                if (element == 'x')
                {
                    lower.x = new Interval(x.Start, value);
                    upper.x = new Interval(value, x.End);
                }
                if (element == 'm')
                {
                    lower.m = new Interval(m.Start, value);
                    upper.m = new Interval(value, m.End);
                }
                if (element == 'a')
                {
                    lower.a = new Interval(a.Start, value);
                    upper.a = new Interval(value, a.End);
                }
                if (element == 's')
                {
                    lower.s = new Interval(s.Start, value);
                    upper.s = new Interval(value, s.End);
                }

                return (lower, upper);
            }

            public long Size => x.Length * m.Length * a.Length * s.Length;
        }

        public static IEnumerable<(PartRange, string)> ProcessRules(IEnumerable<Rule> rules, PartRange part)
        {
            foreach (var rule in rules)
            {
                if (string.IsNullOrEmpty(rule.Comparison))
                {
                    yield return (part, rule.Outcome);
                    continue;
                }

                var element = rule.Comparison[0];
                var comparer = rule.Comparison[1];
                var value = int.Parse(rule.Comparison[2..]);

                if (comparer == '<')
                {
                    var interval = GetValue(element, part);
                    if (interval.Contains(value))
                    {
                        var (lower, higher) = part.Split(element, value);
                        yield return (lower, rule.Outcome);
                        part = higher;
                        continue;
                    }

                    else if (interval.End < value)
                    {
                        yield return (part, rule.Outcome);
                        yield break;
                    }
                    else
                    {
                        // go on with next rule
                        continue;
                    }
                }

                else if (comparer == '>')
                {
                    var interval = GetValue(element, part);

                    // fully above - everything is true
                    if (interval.Start > value)
                    {
                        yield return (part, rule.Outcome);
                        yield break;
                    }

                    // fully below - everything is false
                    else if (interval.End <= value)
                    {
                        continue;
                    }

                    if (interval.Contains(value))
                    {
                        var (lower, higher) = part.Split(element, value + 1);
                        yield return (higher, rule.Outcome);
                        part = lower;
                        continue;
                    }
                }
            }
        }

        public static Interval GetValue(char c, PartRange part)
        {
            switch (c)
            {
                case 'x': return part.x;
                case 'm': return part.m;
                case 'a': return part.a;
                case 's': return part.s;
                default: throw new Exception();
            };
        }
        #endregion

        #region Common
        public struct Workflow
        {
            public string Raw;
            public string Name;
            public List<Rule> Rules;
            public List<string> RulesRaw;

            public static Workflow Parse(string wf)
            {
                var w = new Workflow();
                w.Raw = wf;
                var startOfRules = wf.IndexOf("{");

                w.Name = wf[..startOfRules];

                var rulesRaw = wf[(startOfRules + 1)..^1];
                var rulesSplit = rulesRaw.Split(',');
                w.RulesRaw = rulesSplit.ToList();
                w.Rules = rulesSplit.Select(Rule.Parse).ToList();

                return w;
            }
        }

        public struct Rule
        {
            public string Comparison;
            public string Outcome;

            public static Rule Parse(string s)
            {
                var parts = s.Split(":");
                if (parts.Length == 1)
                {
                    return new Rule()
                    {
                        Outcome = s
                    };
                }
                else
                {
                    return new Rule
                    {
                        Comparison = parts[0],
                        Outcome = parts[1],
                    };
                }
            }

            public override string ToString()
            {
                return $"{this.Comparison}:{this.Outcome}";
            }
        }
        #endregion
    }
}
