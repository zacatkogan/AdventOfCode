using System.Text.RegularExpressions;

namespace AdventOfCode.AoC2021
{
    public class Day_22 : BaseDay
    {
        public override int GetProblemYear() => 2021;

        string regexString = @"(on|off) x=(-?\d+)\.\.(-?\d+),y=(-?\d+)\.\.(-?\d+),z=(-?\d+)\.\.(-?\d+)";

        public IEnumerable<CubeInfo> ParseData(string data)
        {
            foreach (var row in data.Split("\n"))
            {
                var re = new Regex(regexString);
                
                var groups = re.Match(row).Groups;

                var p1 = new Position3dLong
                {
                    X = long.Parse(groups[2].Value),
                    Y = long.Parse(groups[4].Value),
                    Z = long.Parse(groups[6].Value)
                };

                var p2 = new Position3dLong
                {
                    X = long.Parse(groups[3].Value),
                    Y = long.Parse(groups[5].Value),
                    Z = long.Parse(groups[7].Value)
                };

                yield return new CubeInfo{
                    Action = groups[1].Value,
                    Corner1 = p1,
                    Corner2 = p2
                };
            }
        }

        public class CubeInfo
        {
            public long GetSize()
            {
                return (Action == "on" ? 1 : -1)
                    * (Corner2.X - Corner1.X + 1) 
                    * (Corner2.Y - Corner1.Y + 1)
                    * (Corner2.Z - Corner1.Z + 1);
            }

            public string Action;
            public Position3dLong Corner1;
            public Position3dLong Corner2;
        }

        public (Position3dLong, Position3dLong)? GetIntersection(CubeInfo cube1, CubeInfo cube2)
        {
            var minX = Math.Max(cube1.Corner1.X, cube2.Corner1.X);
            var maxX = Math.Min(cube1.Corner2.X, cube2.Corner2.X);
            var minY = Math.Max(cube1.Corner1.Y, cube2.Corner1.Y);
            var maxY = Math.Min(cube1.Corner2.Y, cube2.Corner2.Y);
            var minZ = Math.Max(cube1.Corner1.Z, cube2.Corner1.Z);
            var maxZ = Math.Min(cube1.Corner2.Z, cube2.Corner2.Z);

            if (minX > maxX || minY > maxY || minZ > maxZ)
                return null;

            return ((minX, minY, minZ), (maxX, maxY, maxZ));
        }

        public override object Solve1()
        {
            var data = ParseData(Data).ToList();

            var boundingBox = new CubeInfo{ Corner1 = (-50, -50, -50), Corner2 = (50, 50, 50) };

            List<CubeInfo> instructions = new();

            foreach (var cube in data)
            {
                var smallerCube = GetIntersection(cube, boundingBox);
                if (!smallerCube.HasValue)
                    continue;

                cube.Corner1 = smallerCube.Value.Item1;
                cube.Corner2 = smallerCube.Value.Item2;

                List<CubeInfo> merge = new();

                if (cube.Action == "on")
                    merge.Add(cube);

                // generate compensating instructions
                foreach (var instruction in instructions)
                {
                    var intersection = GetIntersection(cube, instruction);
                    if (intersection.HasValue)
                    {
                        var compensation = new CubeInfo
                        { 
                            Action = instruction.Action == "on" ? "off" : "on",
                            Corner1 = intersection.Value.Item1,
                            Corner2 = intersection.Value.Item2

                        };
                        merge.Add(compensation);
                    }
                }

                instructions.AddRange(merge);
            }

            return instructions.Sum(x => x.GetSize());
        }

        public override object Solve2()
        {
            var data = ParseData(Data).ToList();

            List<CubeInfo> instructions = new();

            foreach (var cube in data)
            {
                List<CubeInfo> merge = new();

                if (cube.Action == "on")
                    merge.Add(cube);

                // generate compensating instructions
                foreach (var instruction in instructions)
                {
                    var intersection = GetIntersection(cube, instruction);
                    if (intersection.HasValue)
                    {
                        var compensation = new CubeInfo
                        { 
                            Action = instruction.Action == "on" ? "off" : "on",
                            Corner1 = intersection.Value.Item1,
                            Corner2 = intersection.Value.Item2

                        };
                        merge.Add(compensation);
                    }
                }

                instructions.AddRange(merge);
            }

            return instructions.Sum(x => x.GetSize());        
        }
    }
}
