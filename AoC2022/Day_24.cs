using System.Collections.Concurrent;
namespace AdventOfCode
{
    public class Day_24 : BaseDay
    {
        public record struct Blizzard
        {
            public string Direction;
            public Position Position;
        }

        public static Dictionary<string, Position> DirectionVectors = new()
        {
            {"^",  (0, -1)},
            {"v", (0, 1)},
            {"<", (-1, 0)},
            {">", (1, 0)}
        };

        public void ParseData(string data)
        {
            List<Blizzard> blizzards = new();
            HashSet<Position> walls = new();
            var splitData = data.Split("\n");

            Position mapDimensions = (splitData.First().Length, splitData.Length);

            for (int i = 0; i < mapDimensions.X; i++)
            for (int j = 0; j < mapDimensions.Y; j++)
            {
                var mapChar = splitData[j][i];
                if (mapChar == '#')
                    walls.Add((i, j));
                else if (mapChar == '.')
                    continue;
                else
                    blizzards.Add(new Blizzard{
                        Position = (i,j),
                        Direction = mapChar.ToString()
                    });
            }

            MapDimensions = mapDimensions;
            Blizzards = blizzards;
            Walls = walls;
            EndingPosition = MapDimensions - (2,1);
        }

        public List<Blizzard> Blizzards;
        public HashSet<Position> Walls;
        public Position MapDimensions;

        public static Position StartingPosition = (1, 0);
        public static Position EndingPosition;
        Dictionary<int, List<Blizzard>> blizzardsOnTurns = new();
        
        List<Blizzard> GetBlizzardDayForTurn(int turn)
        {
            if (turn == 0)
                return Blizzards;
            
            if (blizzardsOnTurns.ContainsKey(turn))
                return blizzardsOnTurns[turn];

            else
            {
                var previousTurn = GetBlizzardDayForTurn(turn - 1);
                
                // update the positions of all blizzards
                var currentTurn = new List<Blizzard>();
                blizzardsOnTurns.Add(turn, currentTurn);

                foreach (var blizzard in previousTurn)
                {
                    var thisBlizzard = blizzard;
                    var dir = DirectionVectors[blizzard.Direction];
                    thisBlizzard.Position += dir;

                    // check if we need to wrap
                    if (thisBlizzard.Position.X < 1)
                        thisBlizzard.Position.X = MapDimensions.X - 2;

                    if (thisBlizzard.Position.X > (MapDimensions.X - 2))
                        thisBlizzard.Position.X = 1;

                    if (thisBlizzard.Position.Y < 1)
                        thisBlizzard.Position.Y = MapDimensions.Y - 2;

                    if (thisBlizzard.Position.Y > (MapDimensions.Y - 2))
                        thisBlizzard.Position.Y = 1;    
                    
                    currentTurn.Add(thisBlizzard);
                }

                return currentTurn;
            }
        }

        Dictionary<int, HashSet<Position>> occupiedSpacesForTurn = new();
        HashSet<Position> GetOccupedSpacesForTurn(int turn)
        {
            if (occupiedSpacesForTurn.ContainsKey(turn))
                return occupiedSpacesForTurn[turn];

            var walls = Walls;
            var blizzards = GetBlizzardDayForTurn(turn)
                .Select(x => x.Position);

            var spaces = walls.Concat(blizzards).ToHashSet();
            occupiedSpacesForTurn.Add(turn, spaces);
            return spaces;
        }

        public override object Solve1()
        {
            ParseData(Data);

            HashSet<(Position, int)> visitedLocations = new();
            
            var maxDistance = Position.ManhattanDistance(StartingPosition, EndingPosition);

            int bestTurns = int.MaxValue;

            var queue = new PriorityQueue<(Position pos, int turn), int>();
            queue.Enqueue((pos: StartingPosition, turn: 0), Position.ManhattanDistance(StartingPosition, EndingPosition));
            
            while (queue.TryDequeue(out var pos_turn, out int priority))
            {
                var currentTurn = pos_turn.turn;
                var nextTurn = currentTurn + 1;
                var currentPos = pos_turn.pos;

                // bail early

                if (visitedLocations.Contains(pos_turn))
                    continue;

                visitedLocations.Add(pos_turn);

                if (pos_turn.turn > bestTurns)
                    continue;

                // check if we've hit the exit yet
                if (currentPos == EndingPosition)
                {
                    if (currentTurn < bestTurns)
                        bestTurns = currentTurn;
                    continue;
                }

                var occupiedLocationsNextTurn = GetOccupedSpacesForTurn(nextTurn);
                if (!occupiedLocationsNextTurn.Contains(currentPos))
                    queue.Enqueue((currentPos, nextTurn), Position.ManhattanDistance(currentPos, EndingPosition) + nextTurn);

                // check if any destinations will be occupied next turn
                var newLocations = DirectionVectors.Values.Select(p => p + currentPos)
                    .Where(p => Position.ManhattanDistance(p, EndingPosition) <= maxDistance
                        && !occupiedLocationsNextTurn.Contains(p))
                    .ToList();

                newLocations.ForEach(p => 
                        queue.Enqueue((p, nextTurn), Position.ManhattanDistance(p, EndingPosition) + nextTurn)
                        );
            }

            return bestTurns;
        }

        public record struct Tracker
        {
            public override int GetHashCode()
            {
                return HashCode.Combine(Position.GetHashCode(), Turn.GetHashCode(), mode.GetHashCode());
            }

            public Position Position;
            public int Turn;
            public int mode;
            public List<int> modeTransitions;

            public int Mode { get => mode; set => mode = value; }
            
            

            public Position Destination => Mode % 2 == 0 ? EndingPosition : StartingPosition;
            public int GetPriority() 
            {
                var maxDistance = Position.ManhattanDistance(StartingPosition, EndingPosition);
                var currentDistance = Position.ManhattanDistance(Position, EndingPosition);
                var remainingTraversals = 2 - Mode;
                return (remainingTraversals * maxDistance + currentDistance) + 100 * Turn;
            }
        }

        public override object Solve2()
        {
            ParseData(Data);

            HashSet<Tracker> visitedLocations = new();
            
            var maxDistance = Position.ManhattanDistance(StartingPosition, EndingPosition);
            int bestTurns = int.MaxValue;

            var queue = new PriorityQueue<Tracker, int>();

            var startTracker = new Tracker{ Position = StartingPosition, Mode = 0, Turn = 0 };
            queue.Enqueue(startTracker, startTracker.GetPriority());
            
            while (queue.TryDequeue(out var tracker, out int priority))
            {
                var currentTurn = tracker.Turn;
                var nextTurn = currentTurn + 1;
                var currentPos = tracker.Position;

                // bail early

                if (visitedLocations.Contains(tracker))
                    continue;

                visitedLocations.Add(tracker);

                if (tracker.Turn > bestTurns)
                    continue;

                // check if we've hit the exit yet
                if (tracker.Position == tracker.Destination)
                {
                    // we've returned to the finish
                    if (tracker.Mode == 2)
                    {
                        if (currentTurn < bestTurns)
                            bestTurns = currentTurn;
                        
                        continue;
                    }
                    else
                        tracker.Mode += 1;
                }

                var occupiedLocationsNextTurn = GetOccupedSpacesForTurn(nextTurn);
                if (!occupiedLocationsNextTurn.Contains(currentPos))
                {
                    var t = tracker;
                    t.Turn++;
                    queue.Enqueue(t, t.GetPriority());
                }
                    

                // check if any destinations will be occupied next turn
                DirectionVectors.Values.Select(p => p + currentPos)
                    .Where(p => Position.ManhattanDistance(currentPos, EndingPosition) <= maxDistance
                        && !occupiedLocationsNextTurn.Contains(p))
                    .ToList()
                    .ForEach(p => 
                    {
                        var t = tracker;
                        t.Position = p;
                        t.Turn++;
                        queue.Enqueue(t, t.GetPriority());
                    });
            }

            return bestTurns;
        }

        public string testData = @"#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#";
    }
}
