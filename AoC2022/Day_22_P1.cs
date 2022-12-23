using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day_22_P1 : BaseDay
    {
        List<string> Directions = new() { "R", "D", "L", "U" };
        public static Dictionary<string, Position> DirectionVectors = new()
        {
            {"U",  (0, -1)},
            {"D", (0, 1)},
            {"L", (-1, 0)},
            {"R", (1, 0)}
        };

        int GetNewDirection(int currentDir, string rotate)
        {
            if (rotate == "R")
                return (currentDir + 1) % 4;
            else if (rotate == "L")
            {
                currentDir--;
                if (currentDir < 0)
                    currentDir += 4;

                return currentDir;
            }
            else // do a 180;
            {
                currentDir -= 2;
                if (currentDir < 0)
                    currentDir += 4;

                return currentDir;
            }
        }

        char GetMapValue(Position p)
        {
            return Map[p.Y][p.X];
        }

        Position GetDirectionVector(int currentDir)
        {
            return DirectionVectors[Directions[currentDir]];
        }

        public (int min, int max) GetRowBounds(int rowNum)
        {
            var xMin = 0;
            var xMax = 0;
            var row = Map[rowNum];
            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] != ' ')
                {
                    xMin = i;
                    break;
                }
            }

            for (int i = row.Length-1; i >= 0; i--)
            {
                if (row[i] != ' ')
                {
                    xMax = i;
                    break;
                }
            }

            return (xMin, xMax);
        }

        public (int min, int max) GetColBounds(int colNum)
        {
            int yMin = 0;
            int yMax = 0;

            for (int i = 0; i < Map.Length; i++)
            {
                if (Map[i][colNum] != ' ')
                {
                    yMin = i;
                    break;
                }
            }

            for (int i = Map.Length-1; i >= 0; i--)
            {
                if (Map[i][colNum] != ' ')
                {
                    yMax = i;
                    break;
                }
            }

            return (yMin, yMax);
        }

        List<string> Instructions;
        char[][] Map;

        Regex instructionRegex = new Regex(@"\d+|[RL]");

        public void ParseData(string data)
        {
            var data_split = data.Split("\n").SplitOnElement("").ToList();

            var mapData = data_split[0];
            var instructionsData = data_split[1].First();

            var mapWidth = mapData.Max(x => x.Length);
            char[][] map = mapData.Select(x => x.PadRight(mapWidth, ' ').ToCharArray()).ToArray();
            Map = map;

            var instructionMatches = instructionRegex.Matches(instructionsData);
            Instructions = instructionMatches.Select(x => x.Value).ToList();
        }

        public Position Move(Position start, int direction, int steps)
        {
            var dirVector = GetDirectionVector(direction);

            var position = start;

            if (dirVector.X == 0)
            {
                var bounds = GetColBounds(start.X);

                for (int i = 0; i < steps; i++)
                {
                    var testPosition = position + dirVector;
                    // wrap around if necessary
                    if (testPosition.Y > bounds.max)
                        testPosition.Y = bounds.min;
                    if (testPosition.Y < bounds.min)
                        testPosition.Y = bounds.max;

                    if (GetMapValue(testPosition) != '.')
                        // stop
                        break;

                    // update our position
                    position = testPosition;
                }

                // look in Y direction
            }
            else
            {
                // look in X direction
                var bounds = GetRowBounds(start.Y);

                for (int i = 0; i < steps; i++)
                {
                    var testPosition = position + dirVector;
                    // wrap around if necessary
                    if (testPosition.X > bounds.max)
                        testPosition.X = bounds.min;
                    if (testPosition.X < bounds.min)
                        testPosition.X = bounds.max;

                    if (GetMapValue(testPosition) != '.')
                        // stop
                        break;

                    // update our position
                    position = testPosition;
                }
            }

            return position;
        }

        public override object Solve1()
        {
            ParseData(Data);

            var row0 = GetRowBounds(0);

            var startX = Map[0].Select((c, i) => (c, i)).First(x => x.c == '.').i;
            Position position = (row0.min, 0);
            var direction = 0; // facing right

            foreach (var instruction in Instructions)
            {
                if (int.TryParse(instruction, out int steps))
                {
                    position = Move(position, direction, steps);
                }
                else
                {
                    direction = GetNewDirection(direction, instruction);
                }
            }

            Console.WriteLine($"Position: {position}, dir: {direction}");

            return (position.X + 1) * 4 + (position.Y + 1) * 1000 + direction;

            return "";
        }

        public override object Solve2()
        {
            throw new NotImplementedException();
        }

        public string testData = @"        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5";
    }
}
