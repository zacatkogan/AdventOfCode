using System.Collections;
using System.Diagnostics.CodeAnalysis;
using AdventOfCode;

namespace AdventOfCode.AoC2021
{
    public class Day_23 : BaseDay
    {
        public enum UnitType
        {
            A = 0,
            B = 1,
            C = 2,
            D = 3
        }

        public record struct Unit
        {
            public UnitType UnitType;
            public Position Position;

            public override string ToString()
            {
                return $"{UnitType} {Position}";
            }
        }

        public static List<int> HomeCols = new() { 3, 5, 7, 9};
        public static List<Position> HallwayPositions = new()
        {
            (1,1), (2,1), (4,1), (6,1), (8,1), (10,1), (11,1)
        };

        public static int GetHomeCol(UnitType type) => HomeCols[(int)type];

        public class SequenceComparer : IEqualityComparer<IEnumerable<Unit>>
        {
            public bool Equals(IEnumerable<Unit>? x, IEnumerable<Unit>? y)
            {
                var hashSet = x.ToHashSet();

                foreach (var element in y)
                {
                    if (hashSet.Add(element))
                        return false;
                }

                return true;
            }

            public int GetHashCode([DisallowNull] IEnumerable<Unit> obj)
            {
                int hashCode = 0;

                foreach (var u in obj.OrderBy(x => x.UnitType)
                    .ThenBy(x => x.Position.X)
                    .ThenBy(x => x.Position.Y))
                    hashCode = HashCode.Combine(hashCode, u);

                return hashCode;
            }
        }

        public class GameMap
        {
            public override int GetHashCode()
            {
                int hashCode = 0;

                foreach (var u in Units.OrderBy(x => x.UnitType)
                    .ThenBy(x => x.Position.X)
                    .ThenBy(x => x.Position.Y))
                    hashCode = HashCode.Combine(hashCode, u);

                return hashCode;
            }

            public int GetMapRank()
            {
                return Score;
                //return -NumActions * 10000 + Score;
            }

            public List<Unit> Units;
            public HashSet<Position> Walls;

            public int Score;
            public int NumActions;

            public List<(Unit, Position)> Moves = new();

            public IEnumerable<(Unit, Position)> GetMoves()
            {
                // check for rooms that are already safe
                // home col must be empty, or all of units type
                var safeRooms = HomeCols.Select((x,i) => (col:x, type:(UnitType)i))
                    .Where(r => Units.Where(u => u.Position.X == r.col).All(u => u.UnitType == r.type))
                    .Select(x => x.col)
                    .ToList();

                // try and move non-safe units out of the rooms into the hallway
                // get the top unit of each room
                var homeUnits = HomeCols
                    .Except(safeRooms)
                    .Select(c => Units.Where(u => u.Position.X == c).MinBy(u => u.Position.Y))
                    .ToList();

                var availableHallwaySpaces = HallwayPositions.Where(x => !Units.Any(u => u.Position == x)).ToList();

                var moves = from u in homeUnits
                            from h in availableHallwaySpaces
                            where CanMove(u.Position, h)
                            select (u,h);
                foreach (var move in moves)
                    yield return move;

                // try and move units out of the hallways into the rooms
                var movingHome = Units
                    .Where(u => HallwayPositions.Contains(u.Position) && CanMoveHome(u))
                    .Select(u => (unit:u, homeCol:GetHomeCol(u.UnitType)))
                    .Select(u => (
                        u.unit,
                        dest:Units.Select(x => x.Position).Concat(Walls).Where(x => x.X == u.homeCol && x.Y > 0).MinBy(x => x.Y) + (0,-1)
                    ))
                    .Where(x => CanMove(x.unit.Position, x.dest))
                    .ToList();
                    
                foreach (var u in movingHome)
                    yield return u;
            }

            public bool CanMoveHome(Unit unit)
            {
                var homeCol = GetHomeCol(unit.UnitType);
                var unitsInCol = Units.Where(u => u.Position.X == homeCol).ToList();

                // home col must be empty, or all of units type
                return !unitsInCol.Any() || unitsInCol.All(u => u.UnitType == unit.UnitType);
            }

            // Assumes that we're always moving to or from the top position of a room
            public bool CanMove(Position from, Position to)
            {
                if (from.Y == to.Y)
                    return false;
                if (from.X == to.X)
                    return false;

                var start = Math.Min(from.X, to.X);
                var end = Math.Max(from.X, to.X);
                var hallwayY = Math.Min(from.Y, to.Y);
                
                for (int i = start; i <= end; i++)
                {
                    // check if any positions are blocked
                    if (Units.Any(x => x.Position == (i, hallwayY) && x.Position != from))
                        return false;
                }

                return true;
            }
        
            public int GetMoveCost(Unit u, Position newPosition)
            {
                var moveCost = (int)Math.Pow(10, (int)u.UnitType);
                var distance = Position.ManhattanDistance(u.Position, newPosition);
                return moveCost * distance;
            }

            public GameMap ExecuteMove(Unit unit, Position newPosition)
            {
                var map = new GameMap
                {
                    Walls = this.Walls,
                    Units = this.Units.ToList(),
                    Score = this.Score + GetMoveCost(unit, newPosition),
                    NumActions = this.NumActions + 1,
                    Moves = this.Moves.ToList()
                };

                map.Moves.Add((unit, newPosition));

                map.Units.Remove(unit);
                map.Units.Add(new Unit{ UnitType = unit.UnitType, Position = newPosition });

                return map;
            }

            public bool IsGameComplete()
            {
                // check that all units are in their home cols
                return Units.All(u => u.Position.X == GetHomeCol(u.UnitType));                    
            }
        }


        public GameMap ParseMap(string mapData)
        {
            var walls = new HashSet<Position>();
            var units = new List<Unit>();

            foreach ((string rowString, int row) in mapData.Split("\n").Select((x,i) => (x, i)))
            {
                foreach ((var c, int col) in rowString.Select((x,i) => (x,i)))
                {
                    if (c == '#')
                        walls.Add((col,row));

                    if (char.IsAsciiLetter(c))
                        units.Add(new Unit {
                            UnitType=Enum.Parse<UnitType>(c.ToString()),
                            Position = (col, row)
                        });
                }
            }

            return new GameMap
            {
                Walls = walls,
                Units = units,
                Score = 0
            };
        }

        public override object Solve1()
        {
            string mapData = @"#############
#...........#
###D#B#C#A###
  #C#A#D#B#
  #########";

            return SolveMap(mapData).Score;
        }

        public override object Solve2()
        {
            string mapData = @"#############
#...........#
###D#B#C#A###
  #D#C#B#A#
  #D#B#A#C#
  #C#A#D#B#
  #########";

            return SolveMap(mapData).Score;
        }

        public GameMap SolveMap(string mapData)
        {
            HashSet<IEnumerable<Unit>> SeenMaps = new(new SequenceComparer());

            var bestScore = int.MaxValue;
            GameMap bestMap = null;

            var startingMap = ParseMap(mapData);

            var mapStack = new Stack<GameMap>();
            mapStack.Push(startingMap);

            var mapQueue = new PriorityQueue<GameMap, int>();
            mapQueue.Enqueue(startingMap, startingMap.GetMapRank());
  
            while(mapQueue.TryDequeue(out var map, out _))
            {
                if (map.IsGameComplete())
                {
                    if (map.Score < bestScore)
                    {
                        bestScore = map.Score;
                        bestMap = map;
                    }
                    continue;
                }

                if (!SeenMaps.Add(map.Units))
                    continue;

                if (map.Score > bestScore)
                    continue;

                var moves = map.GetMoves().OrderByDescending(x => x.Item1.UnitType).ToList();
                foreach (var move in moves)
                {
                    var newMap = map.ExecuteMove(move.Item1, move.Item2);
                    mapQueue.Enqueue(newMap, newMap.GetMapRank());
                }
            }
    
            return bestMap;
        }

        public string testData = @"";
    }
}
