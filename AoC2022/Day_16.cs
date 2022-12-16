using System.Text.RegularExpressions;
namespace AdventOfCode
{
    public class Day_16 : BaseDay
    {
        public Regex regex = new Regex(@"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ([\w, ]*)");

        public struct Valve
        {
            public Valve() { }
            public string Name;
            public int FlowRate;
            public List<string> Connections;

            public override string ToString()
            {
                return $"{Name}: {FlowRate} - {string.Join(",", Connections)}";
            }
        }

        public class Map
        {
            public HashSet<string> OpenValves = new();
            public List<Valve> Valves;
            public IEnumerable<string> Steps = Enumerable.Empty<string>();
            public Valve CurrentValve;
            public int FlowRate;
            public int PressureRelieved;
            public int Time = 0;

            public IEnumerable<Func<Map>> Actions()
            {
                if (!OpenValves.Contains(CurrentValve.Name) && CurrentValve.FlowRate != 0)
                    yield return () => TurnValve();

                foreach (var connection in CurrentValve.Connections)
                    yield return () => WalkTo(connection);
                
            }

            public Map TurnValve()
            {
                var flowRate = FlowRate +  CurrentValve.FlowRate;
                OpenValves.Add(CurrentValve.Name);

                var map = new Map()
                {
                    Valves = Valves,
                    Steps = Steps.Append($"Turn on {CurrentValve.Name}"),
                    FlowRate = flowRate,
                    PressureRelieved = PressureRelieved + FlowRate,
                    CurrentValve = CurrentValve,
                    OpenValves = OpenValves.ToHashSet(),
                    Time = Time + 1,
                };

                return map;
            }

            public Map WalkTo(string room)
            {
                return new Map()
                {
                    Valves = Valves,
                    Steps = Steps.Append($"Walk to {room}"),
                    FlowRate = FlowRate,
                    PressureRelieved = PressureRelieved + FlowRate,
                    CurrentValve = Valves.First(x => x.Name == room),
                    OpenValves = OpenValves.ToHashSet(),
                    Time = Time + 1,
                };
            }
        }

public class MapWithElephant
        {
            public HashSet<string> OpenValves = new();
            public List<Valve> Valves;
            public IEnumerable<string> Steps = Enumerable.Empty<string>();
            public Valve CurrentValve;
            public Valve ElephantValve;
            public int FlowRate;
            public int PressureRelieved;
            public int Time = 0;

            public MapWithElephant ExecuteActions(string action, string elephantAction)
            {
                var newFlowRate = this.FlowRate;
                var openValves = OpenValves.ToHashSet();
                var currentValve = CurrentValve;
                var elephantValve = ElephantValve;

                if (action == "open")
                {
                    if (!openValves.Contains(CurrentValve.Name) && CurrentValve.FlowRate != 0 && openValves.Add(CurrentValve.Name))
                     newFlowRate += CurrentValve.FlowRate;
                }
                else
                {
                    currentValve = Valves.First(x => x.Name == action);
                }

                if (elephantAction == "open")
                {
                    if (!openValves.Contains(ElephantValve.Name) && ElephantValve.FlowRate != 0 && openValves.Add(ElephantValve.Name))
                        newFlowRate += ElephantValve.FlowRate;
                }
                else
                {
                    elephantValve = Valves.First(x => x.Name == elephantAction);
                }

                var map = new MapWithElephant()
                {
                    Valves = Valves,
                    Steps = Steps.Append($"{action}, {elephantAction}"),
                    FlowRate = newFlowRate,
                    PressureRelieved = PressureRelieved + FlowRate,
                    CurrentValve = currentValve,
                    ElephantValve = elephantValve,
                    OpenValves = openValves,
                    Time = Time + 1,
                };

                return map;
            }

            public IEnumerable<(string, string)> Actions()
            {
                return 
                    from a in Actions(CurrentValve.Name).ToList()
                    from b in Actions(ElephantValve.Name).ToList()
                    select (a,b);
            }

            public IEnumerable<string> Actions(string room)
            {
                var a = new List<string>();

                var valve = Valves.First(x=> x.Name == room);
                if (!OpenValves.Contains(room) && valve.FlowRate > 0)
                    a.Add("open");
                
                a.AddRange(valve.Connections);

                return a;
            }

        }


        public IEnumerable<Valve> ParseData()
        {
            foreach(var row in Data.Split("\n"))
            {
                var match = regex.Match(row);

                yield return new Valve() { 
                    Name=match.Groups[1].Value,
                    FlowRate = int.Parse(match.Groups[2].Value),
                    Connections = match.Groups[3].Value.Split(",", StringSplitOptions.TrimEntries).ToList()
                };
            }
        }

        public override object Solve1()
        {
            var valves = ParseData().ToList();

            var start = new Map() {
                Valves = valves,
                CurrentValve = valves.First(x => x.Name == "AA"),
            };

            IEnumerable<Map> turns = new[] {start};
            
            for (int i = 0; i < 30; i++)
            {
                turns = turns.SelectMany(x => x.Actions().Select(x => x()));
                
                if (i > 5)                
                {
                    turns = turns.OrderByDescending(x => x.PressureRelieved).ToList();
                    turns = turns.Take(10000);
                }
            }

            var outcome = turns.MaxBy(x => x.PressureRelieved);

            return outcome.PressureRelieved;
        }

        public override object Solve2()
        {
            var valves = ParseData().ToList();

            var start = new MapWithElephant() {
                Valves = valves,
                CurrentValve = valves.First(x => x.Name == "AA"),
                ElephantValve = valves.First(x => x.Name == "AA")
            };

            IEnumerable<MapWithElephant> turns = new[] {start};
            
            for (int i = 0; i < 26; i++)
            {
                turns = turns.SelectMany(x => x.Actions().Select(y => x.ExecuteActions(y.Item1, y.Item2)));
                
                if (i > 5)                
                {
                    turns = turns.OrderByDescending(x => x.PressureRelieved).ToList();
                    turns = turns.Take(10000);
                }
            }

            var outcome = turns.MaxBy(x => x.PressureRelieved);

            return outcome.PressureRelieved;
        }

        public string testData = @"";
    }
}
