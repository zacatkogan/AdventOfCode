namespace AdventOfCode
{
    public class Day_18 : BaseDay
    {
        public override object Solve1()
        {
            var blocks = Data.Split("\n").Select(x => x.Split(","))
                .Select(x => new Position3d(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
                .ToList();

            var blocksHash = blocks.ToHashSet();

            return blocks.Sum(b => 
                Position3d.Neighbors.Where(n => !blocksHash.Contains(b + n)).Count());
                
        }

        public override object Solve2()
        {
            var blocks = Data.Split("\n").Select(x => x.Split(","))
                .Select(x => new Position3d(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
                .ToList();

            var blocksHash = blocks.ToHashSet();
            var externalSpaces = new HashSet<Position3d>();

            var startingBlock = blocks.OrderBy(b => b.X).First();
            var externalBlock = startingBlock + (-1, 0, 0);

            var externalDimsMin = new Position3d(
                blocks.OrderBy(b => b.X).First().X - 1, 
                blocks.OrderBy(b => b.Y).First().Y - 1,
                blocks.OrderBy(b => b.Z).First().Z - 1);

            var externalDimsMax = new Position3d(
                blocks.OrderByDescending(b => b.X).First().X + 1, 
                blocks.OrderByDescending(b => b.Y).First().Y + 1,
                blocks.OrderByDescending(b => b.Z).First().Z + 1);

            externalSpaces.Add(externalBlock);
            var externalSpacesQueue = new Queue<Position3d>();
            externalSpacesQueue.Enqueue(externalBlock);

            // enumerate all neighbors of the external block, where 
            while (externalSpacesQueue.TryDequeue(out Position3d extBlock))
            {
                var newExternalBlocks = Position3d.Neighbors.Select(n => n + extBlock)
                    .Where(n => !externalSpaces.Contains(n) && !blocksHash.Contains(n))
                    .Where(b => Enumerable.Zip(b, externalDimsMin).All(x => x.First >= x.Second)
                        && Enumerable.Zip(b, externalDimsMax).All(x => x.First <= x.Second)
                    ).ToList();
                
                newExternalBlocks.ForEach(b => externalSpaces.Add(b));
                newExternalBlocks.ForEach(b => externalSpacesQueue.Enqueue(b));
            }

            return blocks.Sum(b => 
                Position3d.Neighbors.Where(n => externalSpaces.Contains(b + n)).Count());

        }

        public string testData = @"";
    }
}
