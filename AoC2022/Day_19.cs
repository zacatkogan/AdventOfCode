using System.Text.RegularExpressions;
namespace AdventOfCode
{
    public class Day_19 : BaseDay
    {
        public enum Resource : int
        {
            Nothing = -1,
            Ore = 0,
            Clay = 1,
            Obsidian = 2,
            Geode = 3,
        }
        
        public class Blueprint
        {
            public int Id;
            public int[] OreRobot;
            public int[] ClayRobot;
            public int[] ObsidianRobot;
            public int[] GeodeRobot;

            int[][]? _robot;

            public int[][] Robots => _robot ??= new int[][]
            {
                OreRobot,
                ClayRobot,
                ObsidianRobot,
                GeodeRobot
            };

            int? _maxOre;
            public int MaxOre => _maxOre ??= Robots.Max(x => x[0]);
            int? _maxClay;
            public int MaxClay => _maxClay ??= Robots.Max(x => x[1]);
        }

        public record struct State
        {
            public Blueprint blueprint;

            public int RobOre;
            public int RobClay;
            public int RobObs;
            public int RobGeode;

            public int Ore;
            public int Clay;
            public int Obsidian;
            public int Geode;

            public int TimeRemaining;

            public static State TakeAction(State state, Resource buildBot)
            {
                state.Geode += state.RobGeode;
                state.Obsidian += state.RobObs;
                state.Clay = Math.Min(state.RobClay + state.Clay, 2 * state.blueprint.MaxClay);
                state.Ore = Math.Min(state.RobOre + state.Ore, 2 * state.blueprint.MaxOre);

                switch (buildBot)
                {
                    case Resource.Clay:
                        state.RobClay++;
                        break;
                    case Resource.Geode:
                        state.RobGeode++;
                        break;
                    case Resource.Obsidian:
                        state.RobObs++;
                        break;
                    case Resource.Ore:
                        state.RobOre++;
                        break;
                }

                if (buildBot != Resource.Nothing)
                {
                    var blueprint = state.blueprint.Robots[(int)buildBot];

                    state.Obsidian -= blueprint[2];
                    state.Clay -= blueprint[1];
                    state.Ore -= blueprint[0];
                }

                state.TimeRemaining--;

                return state;
            }

            public bool CanBuildRobot(Resource robot)
            {
                var blueprint = this.blueprint.Robots[(int)robot];

                return Obsidian >= blueprint[2] &&
                Clay >= blueprint[1] &&
                Ore >= blueprint[0];
            }

            public IEnumerable<Resource> PossibleActions()
            {
                if (CanBuildRobot(Resource.Geode))
                {
                    yield return Resource.Geode;
                    yield break;
                }

                if (CanBuildRobot(Resource.Obsidian))
                {
                    yield return Resource.Obsidian;
                    yield break;
                }
                
                if (CanBuildRobot(Resource.Clay) && RobClay < blueprint.MaxClay)
                    yield return Resource.Clay;

                if (CanBuildRobot(Resource.Ore) && RobOre < blueprint.MaxOre)
                    yield return Resource.Ore;

                yield return Resource.Nothing;
            }

        }

        public override object Solve1()
        {
            var blueprints = ParseData(Data).ToList();

            var ratings = blueprints
                //.AsParallel()
                .Select(x => FindRatingDFS(x, 24))
                .ToList();

            return ratings.Sum(x => x.Geode * x.blueprint.Id);
        }

        public override object Solve2()
        {
            var blueprints = ParseData(Data).Take(3).ToList();

            var results = blueprints.Select(x => FindRatingDFS(x, 32))
                .ToList();

            return results.Select(x => x.Geode).Multiply();
        }

        public State FindRatingDFS(Blueprint blueprint, int time)
        {
            int visited = 0;

            var startingState = new State()
            {
                blueprint = blueprint,
                RobOre = 1,
                //actions = Enumerable.Empty<Resource>(),
                TimeRemaining = time,
            };

            HashSet<State> seen = new();
            Stack<State> queue = new();
            queue.Push(startingState);

            State best = startingState;

            while(queue.TryPop(out State state))
            {
                visited++;
                if(!seen.Add(state))
                    continue;

                if (state.TimeRemaining == 0)
                {
                    if(state.Geode > best.Geode)
                        best = state;
                    
                    continue;
                }

                // if we couldn't possibly make more than the best geodes in the remaining time, skip
                // max possible would be if we created a geode bot every turn between now and end
                // N * (N+1) / 2
                
                // N = remaining turns
                
                if ( state.TimeRemaining < 5)
                {
                    var remainingTime = state.TimeRemaining;
                    var maxPossible = state.Geode + state.RobGeode * remainingTime
                        + remainingTime * (remainingTime + 1) / 2;
                    if (maxPossible <= best.Geode)
                        continue;
                }

                // otherwise, iterate over these actions
                foreach (var action in state.PossibleActions())
                    queue.Push(State.TakeAction(state, action));
            }

            return best;
        }

        public IEnumerable<Blueprint> ParseData(string data)
        {
            
            var regex = new Regex(@"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.");

            var rows = data.Split("\n");

            foreach (var row in rows)
            {
                var match = regex.Match(row);
                var groups = match.Groups;

                yield return new Blueprint()
                {
                    Id = int.Parse(groups[1].Value),
                    OreRobot = new[] {int.Parse(groups[2].Value), 0, 0, 0},
                    ClayRobot = new[] {int.Parse(groups[3].Value), 0, 0, 0},
                    ObsidianRobot = new[] {int.Parse(groups[4].Value), int.Parse(groups[5].Value), 0, 0},
                    GeodeRobot = new[] {int.Parse(groups[6].Value), 0, int.Parse(groups[7].Value), 0},
                };
            }
        }
    }
}
