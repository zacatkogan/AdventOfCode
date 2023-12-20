namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    
    public class Day_20 : BaseDay
    {
        // Flip Flop (%)
        // - High Pulse: nothing
        // - Low Pulse: toggle between on and off, sending a pulse corresponding to new state

        // Conjunction (&)
        // multiple input states, starting LOW
        // when pulse is received, update input state
        // if all states high, sends LOW, else sends HIGH

        // Broadcast (broadcaster)
        // sends pulse to all Destination Modules

        // Button (button module)
        // sends LOW pulse to broadcaster module

        // pulses processed in the order they are sent (Queue behavior)

        public static int currentPress = 0;

        #region Part1
        public override object Solve1()
        {
            //Data = "broadcaster -> a\n%a -> inv, con\n&inv -> b\n%b -> con\n&con -> output";

            var modules = ParseModules(DataLines);

            var highPulses = 0;
            var lowPulses = 0;

            foreach (var i in Enumerable.Range(0, 1000))
            {
                var (high, low) = PushButton(modules);
                highPulses += high;
                lowPulses += low;
            }

            return highPulses * lowPulses;
        }
        #endregion

        #region Part2
        public override object Solve2()
        {
            var modules = ParseModules(DataLines);
            var rx = new RX();
            modules.Add(rx.Name, rx);
            var zh = new ZH((Conjunction)modules["zh"]);
            modules["zh"] = zh;

            var presses = 0;

            PushButton(modules);
            currentPress++;
            presses++;

            while (zh.Toggles.Count != 4)
            {
                presses++;
                currentPress++;

                PushButton(modules);
            }

            return zh.Toggles.Aggregate(1L, (a, v) => a * v.Item2);
        }

        public class RX : Module
        {
            public override string Name { get; set; } = "rx";
            public override IEnumerable<(string, bool)> Pulse(bool high, string source)
            {
                if (!high)
                    LowPulse = true;
                return Enumerable.Empty<(string, bool)>();
            }

            public bool LowPulse = false;
        }

        public class ZH : Conjunction
        {
            public ZH(Conjunction cj)
            {
                this.Inputs = cj.Inputs;
                this.Outputs = cj.Outputs;
                this.Name = cj.Name;
            }

            public List<(string, int)> Toggles = new();

            public override IEnumerable<(string, bool)> Pulse(bool high, string source)
            {
                if (high == true)
                    Toggles.Add((source, currentPress));

                return base.Pulse(high, source);
            }
        }
        #endregion

        public (int high, int low) PushButton(Dictionary<string, Module> modules)
        {
            var pulses = new Dictionary<bool, int> { { true, 0 }, { false, 0 } };

            var queue = new Queue<((string, bool) output, string source)>();
            Button button = new Button();
            foreach (var outcome in button.Pulse(false, ""))
            {
                queue.Enqueue((outcome, button.Name));
            }

            while (queue.TryDequeue(out var result))
            {
                var source = result.source;
                var output = result.output;
                var nextModuleName = output.Item1;
                var pulse = output.Item2;

                pulses[pulse]++;

                if (!modules.ContainsKey(nextModuleName))
                    continue;
                var module = modules[nextModuleName];

                var outcomes = module.Pulse(pulse, source).ToList();

                foreach (var outcome in outcomes)
                {
                    queue.Enqueue((outcome, module.Name));
                }
            }

            return (pulses[true], pulses[false]);
        }

        public Dictionary<string, Module> ParseModules(string[] input)
        {
            Dictionary<string, Module> modules = new();

            foreach (var m in input)
            {
                var splits = m.Split(" ");
                var name = splits[0];
                var outputs = splits.Skip(2).Select(x => x.Trim(',')).ToList();

                if (name[0] == '%')
                {
                    // new flip flop
                    var module = new FlipFlop()
                    {
                        Name = name[1..],
                        Outputs = outputs,
                    };

                    modules.Add(module.Name, module);
                    continue;
                }

                if (m[0] == '&')
                {
                    var module = new Conjunction
                    {
                        Name = name[1..],
                        Outputs = outputs,
                        // need to fill Inputs later
                    };

                    modules.Add(module.Name, module);
                    continue;
                }

                if (splits[0] == "broadcaster")
                {
                    var module = new Broadcaster
                    {
                        Outputs = outputs,
                    };

                    modules.Add(module.Name, module);
                }
            }

            foreach (Conjunction mod in modules.Values.Where(x => x is Conjunction))
            {
                var inputs = modules.Values.Where(x => x.Outputs.Contains(mod.Name)).Select(x => x.Name);
                mod.Inputs = inputs.ToDictionary(i => i, v => false);
            }

            return modules;
        }

        public abstract class Module
        {
            public virtual string Name { get; set; }
            public abstract IEnumerable<(string, bool)> Pulse(bool high, string source);
            public List<string> Outputs { get; set; } = new();
        }

        public class Broadcaster : Module
        {
            public override string Name { get; set; } = "broadcaster";

            public override IEnumerable<(string, bool)> Pulse(bool high, string source)
            {
                foreach (var o in Outputs)
                {
                    yield return (o, high);
                }
            }
        }

        public class Button : Module
        {
            public override string Name { get; set; } = "button";
            public override IEnumerable<(string, bool)> Pulse(bool high, string source)
            {
                yield return ("broadcaster", false);
            }
        }

        public class FlipFlop : Module
        {
            public override IEnumerable<(string, bool)> Pulse(bool high, string source)
            {
                if (high)
                {
                    yield break;
                }
                else
                {
                    State = !State;
                    foreach (var o in Outputs)
                    {
                        yield return(o, State);
                    }
                }
            }

            public bool State { get; set; } = false;
        }

        public class Conjunction : Module
        {
            public override IEnumerable<(string, bool)> Pulse(bool high, string source)
            {
                Inputs[source] = high;

                var outputState = !Inputs.Values.All(x => x == true);

                //if (outputState == false && checks.Contains(this.Name))
                //    ;

                foreach (var o in Outputs)
                    yield return (o, outputState);
            }

            public Dictionary<string, bool> Inputs = new();
        }
    }
}
