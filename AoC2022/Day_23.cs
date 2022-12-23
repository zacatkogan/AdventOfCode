using System.Text;

namespace AdventOfCode
{

    // Coordinate System:
    //  +X is in the right direction
    //  +Y is in the down direction

    public class Day_23 : BaseDay
    {
        public Dictionary<string, Position> Directions = new()
        {
            {"N", (0, -1)},
            {"S", (0, 1)},
            {"E", (1, 0)},
            {"W", (-1, 0)},
            {"NW", (-1, -1)},
            {"NE", (1, -1)},
            {"SW", (-1, 1)},
            {"SE", (1, 1)}
        };

        public List<string> NorthDirections = new() { "N", "NE", "NW"};
        public List<string> SouthDirections = new() { "S", "SE", "SW"};
        public List<string> EastDirections = new() { "E", "NE", "SE"};
        public List<string> WestDirections = new() { "W", "NW", "SW"};
        
        public IEnumerable<List<string>> GetDirectionsPriorityForRound(int round)
        {
            var priorityList = new List<string>[] { NorthDirections, SouthDirections, WestDirections, EastDirections};

            var offset = round % 4;
            return priorityList.Concat(priorityList).Skip(offset).Take(4);
        }

        public class Elf
        {
            public int Id;
            public Position Location;
        }

        /// returns true if there are any elves in the specified directions
        public bool AreNeighboringElvesPresent(Position p, IEnumerable<Position> directionsToCheck, IEnumerable<Elf> elfLocations)
        {
            return directionsToCheck.Select(d => d + p).Any(p => elfLocations.Any(e => e.Location == p));
        }

        public bool AreNeighboringElvesPresent(Position p, IEnumerable<Position> directionsToCheck, HashSet<Position> elfLocations)
        {
            return directionsToCheck.Select(d => d + p).Any(p => elfLocations.Contains(p));
        }
        public List<Elf> ParseData(string data)
        {
            List<Elf> elves = new();
            var numElves = 1;
            var splitData = data.Split("\n");
            var lenX = splitData[0].Length;
            
            for(int y = 0; y < splitData.Length; y++)
            for(int x = 0; x < lenX; x++)
            {
                if (splitData[y][x] == '#')
                {
                    elves.Add(new Elf{Location = (x, y), Id = numElves});
                    numElves++;
                }
            }

            return elves;
        }

        public override object Solve1()
        {
            List<Elf> elves = ParseData(Data);

            int round = 0;

            PrintDictionaryToGrid(elves);

            //RoundTick
            while (round < 10)
            {
                // first half of round:

                List<(Elf e, Position newPosition)> proposedElfMovements = new();
                var dirPriorities = GetDirectionsPriorityForRound(round).ToList();
                var elfPositions = elves.Select(x => x.Location).ToHashSet();


                foreach (var elf in elves)
                {
                    // for each elf, consider adjacent nodes (orthogonal and diagonal)

                    if (!AreNeighboringElvesPresent(elf.Location, Directions.Values, elfPositions))
                    {
                        // no neighboring elves; do nothing
                        continue;
                    }
                    
                    // check if there are elves in the N, S, W or E directions in that order, rotating each round
                    if (!AreNeighboringElvesPresent(elf.Location, dirPriorities[0].Select(d => Directions[d]), elfPositions))
                        proposedElfMovements.Add((elf, elf.Location + Directions[dirPriorities[0][0]]));

                    else if (!AreNeighboringElvesPresent(elf.Location, dirPriorities[1].Select(d => Directions[d]), elfPositions))
                        proposedElfMovements.Add((elf, elf.Location + Directions[dirPriorities[1][0]]));

                    else if (!AreNeighboringElvesPresent(elf.Location, dirPriorities[2].Select(d => Directions[d]), elfPositions))
                        proposedElfMovements.Add((elf, elf.Location + Directions[dirPriorities[2][0]]));
                    
                    else if (!AreNeighboringElvesPresent(elf.Location, dirPriorities[3].Select(d => Directions[d]), elfPositions))
                        proposedElfMovements.Add((elf, elf.Location + Directions[dirPriorities[3][0]]));
                }

                // second half: elves move to the new position only if the position is unique amongst elves
                var elvesThatWillMove = proposedElfMovements.GroupBy(x => x.newPosition)
                    .Where(x => x.Count() == 1)
                    .SelectMany(x => x) // flatten the list
                    .ToList();

                // update the elves locations to the new locations
                elvesThatWillMove.ForEach(x => x.e.Location = x.newPosition);

                PrintDictionaryToGrid(elves);

                round++;
                Console.WriteLine("---- end of round {0} ----", round);
            }
            
            var elfLocations = elves.ToDictionary(x => x.Location);
            Position minPos = (elves.Min(e => e.Location.X), elves.Min(e => e.Location.Y));
            Position maxPos = (elves.Max(e => e.Location.X), elves.Max(e => e.Location.Y));

            var emptyGround = 0;

            for (int i = minPos.X; i <= maxPos.X; i++)
            for (int j = minPos.Y; j <= maxPos.Y; j++)
            {
                if (!elfLocations.ContainsKey((i,j)))
                    emptyGround++;
            }

            return emptyGround;
        }


        public override object Solve2()
        {
            List<Elf> elves = ParseData(Data);

            int round = 0;

            PrintDictionaryToGrid(elves);

            //RoundTick
            while (true)
            {
                // first half of round:

                List<(Elf e, Position newPosition)> proposedElfMovements = new();
                var dirPriorities = GetDirectionsPriorityForRound(round).ToList();

                var elfPositions = elves.Select(x => x.Location).ToHashSet();

                foreach (var elf in elves)
                {
                    // for each elf, consider adjacent nodes (orthogonal and diagonal)

                    if (!AreNeighboringElvesPresent(elf.Location, Directions.Values, elfPositions))
                    {
                        // no neighboring elves; do nothing
                        continue;
                    }
                    
                    // check if there are elves in the N, S, W or E directions in that order, rotating each round
                    if (!AreNeighboringElvesPresent(elf.Location, dirPriorities[0].Select(d => Directions[d]), elfPositions))
                        proposedElfMovements.Add((elf, elf.Location + Directions[dirPriorities[0][0]]));

                    else if (!AreNeighboringElvesPresent(elf.Location, dirPriorities[1].Select(d => Directions[d]), elfPositions))
                        proposedElfMovements.Add((elf, elf.Location + Directions[dirPriorities[1][0]]));

                    else if (!AreNeighboringElvesPresent(elf.Location, dirPriorities[2].Select(d => Directions[d]), elfPositions))
                        proposedElfMovements.Add((elf, elf.Location + Directions[dirPriorities[2][0]]));
                    
                    else if (!AreNeighboringElvesPresent(elf.Location, dirPriorities[3].Select(d => Directions[d]), elfPositions))
                        proposedElfMovements.Add((elf, elf.Location + Directions[dirPriorities[3][0]]));
                }

                // second half: elves move to the new position only if the position is unique amongst elves
                var elvesThatWillMove = proposedElfMovements.GroupBy(x => x.newPosition)
                    .Where(x => x.Count() == 1)
                    .SelectMany(x => x) // flatten the list
                    .ToList();

                if (elvesThatWillMove.Count == 0)
                {
                    return round + 1;
                }

                // update the elves locations to the new locations
                elvesThatWillMove.ForEach(x => x.e.Location = x.newPosition);

                round++;
                Console.WriteLine("---- end of round {0} ----", round);
            }
            
            var elfLocations = elves.ToDictionary(x => x.Location);
            Position minPos = (elves.Min(e => e.Location.X), elves.Min(e => e.Location.Y));
            Position maxPos = (elves.Max(e => e.Location.X), elves.Max(e => e.Location.Y));

            var emptyGround = 0;

            for (int i = minPos.X; i <= maxPos.X; i++)
            for (int j = minPos.Y; j <= maxPos.Y; j++)
            {
                if (!elfLocations.ContainsKey((i,j)))
                    emptyGround++;
            }

            return emptyGround;        
        }

        public void PrintDictionaryToGrid(IEnumerable<Elf> elves)
        {
            return;
            Position minPos = (elves.Min(e => e.Location.X), elves.Min(e => e.Location.Y));
            Position maxPos = (elves.Max(e => e.Location.X), elves.Max(e => e.Location.Y));

            var positionsDict = elves.ToDictionary(x => x.Location);

            for (int j = minPos.Y; j <= maxPos.Y; j++)
            {
                var sb = new StringBuilder();

                for (int i = minPos.X; i <= maxPos.X; i++)
                {
                    if (positionsDict.TryGetValue((i,j), out var elf))
                        sb.Append(elf.Id % 10);
                    else
                        sb.Append(".");
                }

                Console.WriteLine(sb.ToString());
            }
            Console.WriteLine();
        }

        public string testData = @"..............
..............
.......#......
.....###.#....
...#...#.#....
....#...##....
...#.###......
...##.#.##....
....#..#......
..............
..............
..............";
    }
}
