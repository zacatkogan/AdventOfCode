namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using Microsoft.VisualBasic;
    using System.Net.Mail;
    using System.Net.Security;
    using System.Net.WebSockets;
    using System.Reflection.Emit;
    using System.Text.Json.Serialization;
    using static AdventOfCode.AoC2023.Day_22;

    public class Day_22 : BaseDay
    {
        public Dictionary<string, object> cache = new();

        public class Brick
        {
            public int BrickID;
            public Position3d Start;
            public Position3d End;

            public IEnumerable<Position3d> GetBlocks()
            {
                for (int i = Start.X; i <= End.X; i++)
                    for (int j = Start.Y; j <= End.Y; j++)
                        for (int k = Start.Z; k <= End.Z; k++)
                            yield return ((i, j, k));
            }
        }

        public IEnumerable<Brick> ParseBricks(string[] dataLines)
        {
            List<Brick> bricks = new List<Brick>();
            int id = 0;
            foreach (var  line in dataLines)
            {
                var splits = line.Split("~");
                Position3d start = new Position3d(splits[0].GetInts());
                Position3d end = new Position3d(splits[1].GetInts());
                bricks.Add(new Brick { Start = start, End = end, BrickID = id });
                id++;
            }

            return bricks;
        }

        #region Part1
        public override object Solve1()
        {
            //Data = "1,0,1~1,2,1\n0,0,2~2,0,2\n0,2,3~2,2,3\n0,0,4~0,2,4\n2,0,5~2,2,5\n0,1,6~2,1,6\n1,1,8~1,1,9";
            //DataLines = null;

            var bricks = ParseBricks(DataLines).ToList();
            var droppedBricks = new List<Brick>();
            var undroppedBricks = bricks.ToList();

            var occupiedSlots = new Dictionary<Position3d, Brick>();

            cache.Add("BrickFinalPosition", bricks.ToList());

            // drop bricks, starting with Z = 1
            for (int z = 0; z < 1000; z++)
            {
                var toDrop = undroppedBricks.Where(b => b.GetBlocks().Any(q => q.Z == z)).ToList();

                foreach (var dropping in toDrop)
                {
                    var zoffset = DropBrick(dropping, occupiedSlots);
                    dropping.Start += zoffset;
                    dropping.End += zoffset;
                    // apply zoffset, and populate occupiedSplots
                    foreach (var b in dropping.GetBlocks())
                    {
                        occupiedSlots.Add(b, dropping);
                    }

                    droppedBricks.Add(dropping);
                    undroppedBricks.Remove(dropping);
                }
            }

            cache.Add("occupiedSlots", occupiedSlots.ToDictionary());

            return droppedBricks.Count(b => !TryRemoveBrick(b, bricks, occupiedSlots).Any());
        }


        public Position3d DropBrick(Brick brick, Dictionary<Position3d, Brick> occupiedSlots)
        {
            var deltaZ = (0, 0, -1);
            Position3d prevOffset = (0, 0, 0);
            Position3d zoffset = (0, 0, 0);
            while (true)
            {
                foreach (var block in brick.GetBlocks())
                {
                    // apply offset
                    var newBlockLocation = block + zoffset;
                    if (newBlockLocation.Z < 1 || occupiedSlots.ContainsKey(newBlockLocation))
                    {
                        // occupied. Bail.
                        return prevOffset;
                    }
                }

                prevOffset = zoffset;
                zoffset += deltaZ;
            }
        }

        public IEnumerable<Brick> TryRemoveBrick(Brick brickToRemove, List<Brick> allBricks, Dictionary<Position3d, Brick> occupiedSlots)
        {
            var otherBricks = allBricks.Where(x => x != brickToRemove).ToList();
            var s = occupiedSlots.ToDictionary();

            var supportedBricks = new HashSet<Brick>();

            var bricksWhichFall = new HashSet<Brick>();

            foreach (var block in brickToRemove.GetBlocks())
            {
                var higherBlock = block + (0, 0, 1);
                if (s.TryGetValue(higherBlock, out var supportedBrick))
                {
                    if (supportedBrick != brickToRemove)
                        supportedBricks.Add(supportedBrick);
                }

                s.Remove(block);
            }
            
            foreach (var brick in supportedBricks)
            {
                foreach (var block in brick.GetBlocks())
                {
                    s.Remove(block);
                }
            }

            foreach (var brick in supportedBricks)
            {
                var offset = DropBrick(brick, s);
                if (offset != (0, 0, 0))
                    bricksWhichFall.Add(brick);
            }

            return bricksWhichFall;
        }

        #endregion

        #region Part2
        public override object Solve2()
        {
            var brickPositions = ((List<Brick>)cache["BrickFinalPosition"]).ToList();
            var occupiedSlots = ((Dictionary<Position3d, Brick>)cache["occupiedSlots"]).ToDictionary();

            var brickRelationships = brickPositions.ToDictionary(b => b, b => GetSupported_Supporting_BrickMap(b, occupiedSlots));
            var supportingMap = brickRelationships.ToDictionary(x => x.Key, x => x.Value.Supporting.ToList());
            var supportedByMap = brickRelationships.ToDictionary(x => x.Key, x => x.Value.SupportedBy.ToList());

            var count = 0L;

            foreach (var brick in brickPositions)
            {
                var affectedBricks = TryRemoveBrick(brick, supportingMap, supportedByMap);
                count += affectedBricks;
            }

            // answer: 59266

            return "";
        }

        readonly Brick Ground = new Brick();

        public (IEnumerable<Brick> Supporting, IEnumerable<Brick> SupportedBy)
            GetSupported_Supporting_BrickMap(Brick brick, Dictionary<Position3d, Brick> occupiedSpaces)
        {
            var supporting = new HashSet<Brick>();
            var supportedBy = new HashSet<Brick>();

            Console.WriteLine(brick.BrickID);

            foreach (var block in brick.GetBlocks())
            {
                var above = block + (0, 0, 1);
                var below = block + (0, 0, -1);

                if (below.Z < 1)
                {
                    supportedBy.Add(Ground);
                }
                else if (occupiedSpaces.TryGetValue(below, out var supportedByBrick))
                {
                    if (supportedByBrick != brick)
                        supportedBy.Add(supportedByBrick);
                }

                if (occupiedSpaces.TryGetValue(above, out var supportingBrick))
                {
                    if (supportingBrick != brick)
                        supporting.Add(supportingBrick);
                }
            }

            return (supporting, supportedBy);
        }


        public int TryRemoveBrick(
            Brick brickToRemove, 
            Dictionary<Brick, List<Brick>> supportingMap,
            Dictionary<Brick, List<Brick>> supportedByMap)
        {
            var movedBricks = new HashSet<Brick>() { brickToRemove };
            var queue = new Queue<Brick>();

            foreach (var removedSupporting in supportingMap[brickToRemove])
            {
                queue.Enqueue(removedSupporting);
            }

            while (queue.TryDequeue(out var brickToTest))
            {
                // check if all supporting bricks have been removed
                var supportingBricks = supportedByMap[brickToTest];
                if (supportingBricks.All(movedBricks.Contains))
                {
                    // this brick will move. all bricks that it supports need to be tested
                    movedBricks.Add(brickToTest);
                    var supportedBricks = supportingMap[brickToTest];
                    queue.Enqueue(supportedBricks);
                }
            }

            return movedBricks.Count - 1; // don't count the removed brick itself
        }

        #endregion
    }
}
