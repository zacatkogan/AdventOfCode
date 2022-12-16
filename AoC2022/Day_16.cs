using System.Text.RegularExpressions;
namespace AdventOfCode
{
    public class Day_16 : BaseDay
    {
        public Regex regex = new Regex(@"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ([\w, ]*)");

        public class Valve
        {
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
            public Valve CurrentValve;
            public int FlowRate;
            public int PressureRelieved;

            public IEnumerable<string> Actions()
            {
                if (!OpenValves.Contains(CurrentValve.Name) && CurrentValve.FlowRate != 0)
                    yield return "open";

                foreach (var connection in CurrentValve.Connections)
                    yield return connection;
            }

            public Map ExecuteAction(string action)
            {
                var flowRate = FlowRate;
                var openValves = this.OpenValves.ToHashSet();
                var currentValve = this.CurrentValve;

                switch (action)
                {
                    case "open":
                        flowRate += CurrentValve.FlowRate;
                        openValves.Add(CurrentValve.Name);
                        break;
                    default:
                        currentValve = Valves.FirstOrDefault(x => x.Name == action);
                        break;
                }

                var map = new Map()
                {
                    Valves = Valves,
                    FlowRate = flowRate,
                    PressureRelieved = PressureRelieved + FlowRate,
                    CurrentValve = currentValve,
                    OpenValves = openValves,
                };

                return map;
            }
        }

        public class MapWithElephant
        {
            public HashSet<string> OpenValves = new();
            public List<Valve> Valves;
            public Valve CurrentValve;
            public Valve ElephantValve;
            public int FlowRate;
            public int PressureRelieved;

            public MapWithElephant ExecuteActions(string action, string elephantAction)
            {
                var newFlowRate = this.FlowRate;
                var openValves = OpenValves;
                var currentValve = CurrentValve;
                var elephantValve = ElephantValve;

                if (action == "open")
                {
                    if (!openValves.Contains(CurrentValve.Name) && CurrentValve.FlowRate != 0)
                    {
                        openValves = openValves.ToHashSet();
                        openValves.Add(CurrentValve.Name);
                        newFlowRate += CurrentValve.FlowRate;
                    }
                }
                else
                {
                    currentValve = Valves.First(x => x.Name == action);
                }

                if (elephantAction == "open")
                {
                    if (!openValves.Contains(ElephantValve.Name) && ElephantValve.FlowRate != 0)
                    {
                        openValves = openValves.ToHashSet();
                        openValves.Add(ElephantValve.Name);
                        newFlowRate += ElephantValve.FlowRate;
                    }
                }
                else
                {
                    elephantValve = Valves.First(x => x.Name == elephantAction);
                }

                var map = new MapWithElephant()
                {
                    Valves = Valves,
                    FlowRate = newFlowRate,
                    PressureRelieved = PressureRelieved + FlowRate,
                    CurrentValve = currentValve,
                    ElephantValve = elephantValve,
                    OpenValves = openValves,
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
                turns = turns.SelectMany(x => x.Actions().Select(y => x.ExecuteAction(y)));
                
                if (i > 5)                
                {
                    turns = turns.OrderByDescending(x => x.PressureRelieved).ToList();
                    turns = turns.Take(1000);
                }
            }

            var outcome = turns.MaxBy(x => x.PressureRelieved);

            return outcome.PressureRelieved;
        }

        public override object Solve2()
        {
            var valves = ParseData().ToList();
            var startValve = valves.First(x => x.Name == "AA");

            var start = new MapWithElephant() {
                Valves = valves,
                CurrentValve = startValve,
                ElephantValve = startValve
            };

            IEnumerable<MapWithElephant> turns = new[] {start};
            
            for (int i = 0; i < 26; i++)
            {
                turns = turns.SelectMany(x => x.Actions().Select(y => x.ExecuteActions(y.Item1, y.Item2))).ToList();
                
                if (i > 4)                
                {
                    turns = turns
                        //.Where(x => x.OpenValves.Count > 0)
                        .OrderByDescending(x => x.PressureRelieved)
                        .Take(10000).ToList();
                }
            }

            var outcome = turns.MaxBy(x => x.PressureRelieved);

            return outcome.PressureRelieved;
        }

        public string testData = @"";
    }
}
