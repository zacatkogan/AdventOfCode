using System.Text.RegularExpressions;

namespace AdventOfCode
{
    // actual data map:
    //      1 2
    //      3
    //    4 5
    //    6

    // as a net:
    //       1
    //    >4 3 >2 >6
    //       5
    //      

    // 

    public class Day_22_P2 : BaseDay
    {
        public override int GetProblemDay() => 22;

        public enum Direction 
        {
            R = 0, D = 1, L = 2, U = 3
        }

        List<string> Directions = new() { "R", "D", "L", "U" };

        List<Position> DirectionVectors = new()
        {
            (1, 0),// R
            (0, 1),// D
            (-1, 0),// L
            (0, -1)// U
        };

        public Position GetDirectionVector(Direction dir) => DirectionVectors[(int)dir];
        public Position GetDirectionVector(int currentDir) => DirectionVectors[currentDir];

        public (Position, Direction) TransformToNewSide(Position old, Direction oldEdge, Direction newEdge)
        {
            // edges wrap smoothly
            if (oldEdge == Direction.U && newEdge == Direction.D)
                return ((old.X, 49), Direction.U); // x is the same, y is reset
            if (oldEdge == Direction.D && newEdge == Direction.U)
                return ((old.X, 0), Direction.D); // x is the same, y is reset
            if (oldEdge == Direction.L && newEdge == Direction.R)
                return ((49, old.Y), Direction.L);
            if (oldEdge == Direction.R && newEdge == Direction.L)
                return ((0, old.Y),Direction.R);

            // edges mirror
            if (oldEdge == Direction.U && newEdge == Direction.U)
                return ((49 - old.X, 0), Direction.D); // Y is the same
            if (oldEdge == Direction.D && newEdge == Direction.D)
                return ((49 - old.X, 49), Direction.U);
            if (oldEdge == Direction.R && newEdge == Direction.R)
                return ((49, 49 - old.Y), Direction.L);
            if (oldEdge == Direction.L && newEdge == Direction.L)
                return ((0, 49 - old.Y), Direction.R);

            // now only the necessary edge transforms
            if (oldEdge == Direction.U && newEdge == Direction.L)
                return ((0, old.X), Direction.R);

            if (oldEdge == Direction.L && newEdge == Direction.U)
                return ((old.Y, 0), Direction.D);

            if (oldEdge == Direction.R && newEdge == Direction.D)
                return ((old.Y, 49), Direction.U);
            
            if (oldEdge == Direction.D && newEdge == Direction.R)
                return ((49, old.X), Direction.L);

            throw new Exception();
        }

        public class MapFace
        {
            public int FaceId;
            public Dictionary<Position, MapPoint> MapData;
            public List<(int face, Direction)> MapConnections;

            public override string ToString()
            {
                return $"Face: {FaceId}";
            }
        }

        public Direction RotateDirection(Direction currentDir, string instruction)
        {
            if (instruction == "L")
            {
                var dirInt = (int)currentDir;
                dirInt--;
                if (dirInt < 0)
                    dirInt += 4;
                
                return (Direction)dirInt;
            }
            else
            {
                var dirInt = (int)currentDir;
                dirInt = (dirInt + 1) % 4;
                return (Direction)dirInt;
            }
        }









        // public static Dictionary<string, Position> DirectionVectors = new()
        // {
        //     {"U",  (0, -1)},
        //     {"D", (0, 1)},
        //     {"L", (-1, 0)},
        //     {"R", (1, 0)}
        // };

        public List<char[,]> mapgrid;

        
        List<string> Instructions;

        Regex instructionRegex = new Regex(@"\d+|[RL]");

        public List<MapFace> ParseData(string data)
        {
            var data_split = data.Split("\n").SplitOnElement("").ToList();

            var mapData = data_split[0].Select(x => x.PadRight(200, ' ')).ToList();
            var instructionsData = data_split[1].First();

            // instructions
            var instructionMatches = instructionRegex.Matches(instructionsData);
            Instructions = instructionMatches.Select(x => x.Value).ToList();

            // map
            var mapChunksKeys = (
                from i in Enumerable.Range(0,4)
                from j in Enumerable.Range(0,4)
                select (i,j)
                );
                
            Dictionary<(int, int), List<MapPoint>> mapChunks = new();

            foreach (var key in mapChunksKeys)
            {
                var chunkX = key.i;
                var chunkY = key.j;

                if (mapData[chunkY * 50][chunkX * 50] == ' ')
                {
                    mapChunks.Add(key, null);
                    continue;
                }

                var mapChunk = new List<MapPoint>();

                for (int i = 0; i < 50; i++)
                for (int j = 0; j < 50; j++)
                {
                    var mapIndexI = chunkX * 50 + i;
                    var mapIndexJ = chunkY * 50 + j;

                    mapChunk.Add(new MapPoint
                    {
                        OriginalPosition = (mapIndexI, mapIndexJ),
                        ChunkPosition = (i,j),
                        Data = mapData[mapIndexJ][mapIndexI]
                    });
                }

                mapChunks.Add(key, mapChunk);
            }

            var mapFaces = new List<MapFace>();

            // Face 0
            mapFaces.Add(new MapFace() { 
                FaceId = 0,
                MapData = mapChunks[(1,0)].ToDictionary(x => x.ChunkPosition),
                MapConnections = new()
                {
                    (1, Direction.L), // right connection
                    (2, Direction.U), // down connection
                    (3, Direction.L),
                    (5, Direction.L),
                }
            });

            // Face 1
            mapFaces.Add(new MapFace() {
                FaceId = 1,
                MapData = mapChunks[(2,0)].ToDictionary(x => x.ChunkPosition),
                MapConnections = new()
                {
                    (4, Direction.R),
                    (2, Direction.R),
                    (0, Direction.R),
                    (5, Direction.D)
                }
            });

            // Face2 / 3
            mapFaces.Add(new MapFace(){
                FaceId = 2,
                MapData = mapChunks[(1,1)].ToDictionary(x => x.ChunkPosition),
                MapConnections = new()
                {
                    (1, Direction.D), // right
                    (4, Direction.U), // down
                    (3, Direction.U), // left
                    (0, Direction.D)  // up
                }
            });

            // Face3 / 4
            mapFaces.Add(new MapFace(){
                FaceId = 3,
                MapData = mapChunks[(0,2)].ToDictionary(x => x.ChunkPosition),
                MapConnections = new()
                {
                    (4, Direction.L), // right
                    (5, Direction.U), // down
                    (0, Direction.L), // left
                    (2, Direction.L)  // up
                }
            });

            // Face4 / 5
            mapFaces.Add(new MapFace(){
                FaceId = 4,
                MapData = mapChunks[(1,2)].ToDictionary(x => x.ChunkPosition),
                MapConnections = new()
                {
                    (1, Direction.R), // right
                    (5, Direction.R), // down
                    (3, Direction.R), // left
                    (2, Direction.D)  // up
                }
            });
            
            // Face5 / 6
            mapFaces.Add(new MapFace(){
                FaceId = 5,
                MapData = mapChunks[(0,3)].ToDictionary(x => x.ChunkPosition),
                MapConnections = new()
                {
                    (4, Direction.D), // right
                    (1, Direction.U), // down
                    (0, Direction.U), // left
                    (3, Direction.D)  // up
                }
            });

            //return newMap;
            return mapFaces;
        }

        public override object Solve1()
        {
            return "";
        }

        public class MapPoint
        {
            public Position OriginalPosition;
            public Position ChunkPosition;

            public char Data;
            public static bool operator ==(MapPoint p, char c)
            {
                return c == p.Data;
            }

            public static bool operator !=(MapPoint p, char c)
            {
                return c != p.Data;
            }
        }


        public override object Solve2()
        {
            var mapData = ParseData(Data);

            var mapFace = mapData[0];
            var currentDir = Direction.R;
            Position currentPosition = (0,0);


            foreach ((var instruction, int ctr) in Instructions.Select((x, i) =>(x,i)))
            {
                if (int.TryParse(instruction, out int steps))
                {
                    for (int i = 0; i < steps; i++)
                    {
                        var testDirection = currentDir;
                        var testDirectionVector = GetDirectionVector(testDirection);
                        var testStep = currentPosition + testDirectionVector;
                        var testMap = mapFace;

                        if (testStep.X < 0 || testStep.X >= 50
                            || testStep.Y < 0 || testStep.Y >= 50)
                        {
                            // we've stepped off the map. need to get the new map no and direction
                            (int newFace, Direction newEdge) = testMap.MapConnections[(int)testDirection];
                            Console.WriteLine("Old Face: {0} {1}, New Face: {2} {3}", mapFace.FaceId, testDirection, newFace, newEdge);
                            testMap = mapData[newFace];
                            var oldPos = testStep;
                            (testStep, testDirection) = TransformToNewSide(testStep, testDirection, newEdge);
                            Console.WriteLine("Old pos: {0}, new pos: {1}", oldPos, testStep);
                        }

                        // check if the new map position is available:
                        if (testMap.MapData[testStep] != '.')
                            break;

                        // otherwise, update the position, direction, and map face
                        currentDir = testDirection;
                        currentPosition = testStep;
                        mapFace = testMap;
                    }
                }
                else
                {
                    currentDir = RotateDirection(currentDir, instruction);
                }
            }

            var mapPosition = mapFace.MapData[currentPosition].OriginalPosition;

            return (mapPosition.X + 1) * 4 + (mapPosition.Y + 1) * 1000 + (int)currentDir;


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
