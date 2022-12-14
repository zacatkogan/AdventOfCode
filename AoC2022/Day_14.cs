namespace AdventOfCode
{
    public class Day_14 : BaseDay
    {
        public override object Solve1()
        {
            var grid = ParseData();
            var maxY = 0;
            
            for (int i = 0; i < grid.GetLength(1); i++)
                if (grid.Row(i).Any(x => x!= 0))
                    maxY = i;

            var sand = 0;

            var movePriority = new List<Position>() {
                (0, 1),
                (-1, 1),
                (1, 1)
            };

            bool isClear(Position p) => 
                grid[p.X, p.Y] == 0;


            // loop forever
            while (true)
            {
                Position p = (500, 0);
                // drop sand
                while (true)
                {
                    var move = movePriority.FirstOrDefault(x => isClear(p + x));
                    if (move == null)
                    {
                        grid[p.X, p.Y] = 1;
                        break;
                    }

                    p = p + move;
                    if (p.Y > maxY)
                        goto end;
                }

                
                sand++;
            }

            end:
            return sand;

        }

        public override object Solve2()
        {
            var grid = ParseDataToDict();
            var maxY = grid.Keys.Select(x => x.Y).Max();
            var floorY = maxY + 2;

            var sand = 0;

            var movePriority = new List<Position>() {
                (0, 1),
                (-1, 1),
                (1, 1)
            };

            bool isClear(Position p) => 
                !grid.ContainsKey((p.X, p.Y));


            // loop forever
            while (true)
            {
                Position p = (500, 0);
                // drop sand
                while (true)
                {
                    var move = movePriority.FirstOrDefault(x => isClear(p + x));
                    if (move == null || (p.Y + 1) == floorY)
                    {
                        if (grid.ContainsKey((p.X, p.Y)))
                            goto end;
                        grid.Add((p.X, p.Y), 1);
                        break;
                    }

                    p = p + move;
                }

                sand++;
            }

            end:
            return sand;        }

        public string testData = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";

        public int[,] ParseData()
        {
            int[,] grid = new int[1000,1000];

            foreach (var row in Data.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var coords = row.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var parsedCoords = Enumerable.Zip(coords[..^1], coords[1..])
                .ToList();

                parsedCoords.ForEach(x => 
                {
                    var start = x.First.Split(",").Select(int.Parse).ToList();
                    var end = x.Second.Split(",").Select(int.Parse).ToList();

                    var XCoord = (new[] { start[0], end[0]}).OrderBy(z => z).ToList();
                    var YCoord = (new[] { start[1], end[1]}).OrderBy(z => z).ToList();

                    for (int i = XCoord[0]; i <= XCoord[1]; i++)
                    for (int j = YCoord[0]; j <= YCoord[1]; j++)
                        grid[i,j] = -1;
                });
            }

            return grid;
        }


        public Dictionary<Position, int> ParseDataToDict()
        {
            Dictionary<Position, int> map = new Dictionary<Position, int>();

            foreach (var row in Data.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var coords = row.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var parsedCoords = Enumerable.Zip(coords[..^1], coords[1..])
                .ToList();

                parsedCoords.ForEach(x => 
                {
                    var start = x.First.Split(",").Select(int.Parse).ToList();
                    var end = x.Second.Split(",").Select(int.Parse).ToList();

                    var XCoord = (new[] { start[0], end[0]}).OrderBy(z => z).ToList();
                    var YCoord = (new[] { start[1], end[1]}).OrderBy(z => z).ToList();

                    for (int i = XCoord[0]; i <= XCoord[1]; i++)
                    for (int j = YCoord[0]; j <= YCoord[1]; j++)
                        map.TryAdd((i,j), -1);
                });
            }

            return map;
        }

        
    }



    // // public class GroundMap
    // // {
    // //     private GroundMap()
    // //     {
    // //     }

    // //     public GroundMap(IEnumerable<ClayVein> clayVeins)
    // //     {
    // //         this.ClayVeins = clayVeins.ToList();
    // //         this.Data = GenerateDataArray();
    // //     }

    // //     GroundType[,] GenerateDataArray()
    // //     {
    // //         this.MaxX = Math.Max(ClayVeins.Max(d => d.StartX), ClayVeins.Max(d => d.EndX));
    // //         this.MaxY = Math.Max(ClayVeins.Max(d => d.StartY), ClayVeins.Max(d => d.EndY));

    // //         var dataArray = new GroundType[MaxX+1, MaxY+1];

    // //         foreach (var vein in ClayVeins)
    // //         {
    // //             for (int i = vein.StartX; i <= vein.EndX; i++)
    // //             for (int j = vein.StartY; j <= vein.EndY; j++)
    // //                 dataArray[i,j] = GroundType.Clay;
    // //         }

    // //         // set the Spring at X=500, Y=0
    // //         dataArray[500,0] = GroundType.Spring;

    // //         return dataArray;
    // //     }

    // //     public GroundType[,] Data;

    // //     public int MaxX;
    // //     public int MaxY;
    // //     private List<ClayVein> ClayVeins;

    // //         public class ClayVein
    // // {
    // //     public int StartX;
    // //     public int EndX;
    // //     public int StartY;
    // //     public int EndY;

    // //     public override string ToString()
    // //     {
    // //         return $"X: {StartX}..{EndX}, Y: {StartY}..{EndY}";
    // //     }


    // public class ClayVein{
    //     public static ClayVein Parse(string s)
    //     {
    //         var c = new ClayVein();
    //         // data like "x=100, y=200..205"
    //         var dirData = s.Split(", ");
    //         foreach (var dir in dirData)
    //         {
    //             if (dir.StartsWith("x"))
    //             {
    //                 (c.StartX, c.EndX) = ParseLength(dir.Substring(2));
    //             }
    //             else if (dir.StartsWith("y"))
    //             {
    //                 (c.StartY, c.EndY) = ParseLength(dir.Substring(2));
    //             }
    //         }
    //         return c;
    //     }

    //     private static (int start, int stop) ParseLength(string s)
    //     {
    //         if (s.Contains(".."))
    //         {
    //             var data = s.Split("..");
    //             return (int.Parse(data[0]), int.Parse(data[1]));
    //         }
    //         else
    //         {
    //             var i = int.Parse(s);
    //             return (i, i);
    //         }
    //     }

    //     public int StartX;
    //     public int StartY;
    //     public int EndX;
    //     public int EndY;
    // }
    
}
