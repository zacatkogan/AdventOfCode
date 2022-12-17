namespace AdventOfCode
{
    public class Day_17 : BaseDay
    {

        public class Rock
        {
            public List<Position> positions;
            public Position lowest => positions.OrderBy(x=> x.Y).First();
            public Position leftest => positions.OrderBy(x => x.X).First();

        }
        public List<Rock> Rocks = new List<Rock>()
        {
            new Rock() { positions = new List<Position>() {(0,0), (1,0), (2, 0), (3, 0)} },
            new Rock() { positions = new List<Position>() { (1, 0), (0, 1), (1, 1), (2, 1), (1,2)}},
            new Rock() { positions = new List<Position>() {(0,0), (1,0), (2, 0), (2, 1), (2, 2)}},
            new Rock() { positions = new List<Position>() {(0,0), (0,1), (0,2), (0,3)}},
            new Rock() { positions = new List<Position>() { (0,0), (0, 1), (1, 0), (1, 1)}}
        };

        public override object Solve1()
        {
            return "";
            return DropRocks(2022, Data);
        }
        public int DropRocks(int numRocks, string data)
        {
            HashSet<Position> blocks = new HashSet<Position>();

            var rocks = Enumerable.Repeat(Rocks, int.MaxValue).SelectMany(x => x);
            var rocksEnumerator = rocks.GetEnumerator();
            var movementEnumerator = Enumerable.Repeat(data, int.MaxValue).SelectMany(x => x).GetEnumerator();

            var rocksDropped = 0;

            bool CanMove(Rock rock, Position currentPosition, Position movement, HashSet<Position> occupiedPositions)
            {
                return rock.positions.Select(x => x + currentPosition + movement)
                    .All(p => p.X >= 0 && p.X <= 6 && p.Y > 0 && !occupiedPositions.Contains(p));
            }
            
            void SettleRock(Rock rock, Position currentPosistion, HashSet<Position> occupiedPositions)
            {
                foreach (var pos in rock.positions.Select(r => r + currentPosistion))
                    occupiedPositions.Add(pos);
            }

            while (rocksDropped < numRocks)
            {
                // get next rock
                rocksEnumerator.MoveNext();
                var currentRock = rocksEnumerator.Current;

                var highestBlock = (blocks.OrderByDescending(x => x.Y).FirstOrDefault() ?? new Position(0,0)).Y; 
                Position currentRockPosition = (2, highestBlock + 4);
                //Console.WriteLine("Dropping new block, y=" + (highestBlock+4));

                while (true)
                {
                    // push
                    movementEnumerator.MoveNext();
                    var movement = movementEnumerator.Current == '<' ? Position.Directions["L"] : Position.Directions["R"];
                    if (CanMove(currentRock, currentRockPosition, movement, blocks))
                    {
                        //Console.WriteLine("Move " + movementEnumerator.Current);
                        currentRockPosition += movement;
                    }
                    //else
                        //Console.WriteLine("Failed to move " + movementEnumerator.Current);
                    
                    // drop
                    movement = Position.Directions["D"];
                    if (CanMove(currentRock, currentRockPosition, movement, blocks))
                    {
                        //Console.WriteLine("Move down");
                        currentRockPosition += movement;
                    }
                    else
                    {
                        //Console.WriteLine("Settle Rock, y=" + currentRockPosition.Y);
                        SettleRock(currentRock, currentRockPosition, blocks);
                        rocksDropped++;
                        break;
                    }   
                }
            }

            var height = blocks.OrderByDescending(x => x.Y).FirstOrDefault();
            return (height ?? (0,0)).Y;
        }


        public static bool CanMove(Rock rock, Position currentPosition, Position movement, HashSet<Position> occupiedPositions)
        {
            return rock.positions.Select(x => x + currentPosition + movement)
                .All(p => p.X >= 0 && p.X <= 6 && p.Y > 0 && !occupiedPositions.Contains(p));
        }
            
        public static void SettleRock(Rock rock, Position currentPosistion, HashSet<Position> occupiedPositions)
        {
            foreach (var pos in rock.positions.Select(r => r + currentPosistion))
                occupiedPositions.Add(pos);
        }

        public static int GetMaxHeight(HashSet<Position> positions)
        {
            return (positions.OrderByDescending(x => x.Y).FirstOrDefault() ?? new Position(0,0)).Y; 
        }

        public List<(int numRocks, int instructionCounter, int height)> DropRocks_StackHeights(int numRocks, string data)
        {
            HashSet<Position> blocks = new HashSet<Position>();
            var towerHeights = new List<(int, int)>();

            var rocksDropped = 0;
            var instructionCounter = -1;

            Rock GetRock() => Rocks[rocksDropped % Rocks.Count];
            char GetInstruction() => data[instructionCounter % data.Length];

            var heightCache = new List<(int rock, int instruction, int height)>();
            //void Signature (int rockNo, int instructionNo);

            while (true)
            {
                // get next rock
                var currentRock = GetRock();
                var highestBlock = GetMaxHeight(blocks);
                Position currentRockPosition = (2, highestBlock + 4);

                while (true)
                {
                    // push
                    instructionCounter++;
                    var currentInstruction = GetInstruction();
                    var movement = currentInstruction == '<' ? Position.Directions["L"] : Position.Directions["R"];
                    if (CanMove(currentRock, currentRockPosition, movement, blocks))
                    {
                        currentRockPosition += movement;
                    }
                    
                    // drop
                    movement = Position.Directions["D"];
                    if (CanMove(currentRock, currentRockPosition, movement, blocks))
                        currentRockPosition += movement;
                    else
                    {
                        SettleRock(currentRock, currentRockPosition, blocks);
                        break;
                    }   
                }

                // cache the rock no and the instruction no against the height
                var height = GetMaxHeight(blocks);

                heightCache.Add((rocksDropped, instructionCounter, height));
                rocksDropped++;
                if (rocksDropped > numRocks)
                    return heightCache;
            }

        }

        public override object Solve2()
        {
            // pattern repeats when X moves have been played and a rock has just dropped
            var data = Data;

            var a = DropRocks_StackHeights(10000, data);
            var groups = a.ToList(x => (rock: x.numRocks, inst: x.instructionCounter, x.height))
                .GroupBy(x => (x.rock % 5, x.inst % data.Length))
                .Where(x => x.Count() > 1)
                .ToList();

            var repeated = groups[0];
            var l = repeated.ToList();

            // this repeats every `rocksDiff` rocks, starting at `repeatOffset` rocks dropped.
            // it increases by heightDiff every `rocksDiff` increment
            
            var heightDiff = l[2].height - l[1].height;
            var rocksDiff = l[2].rock - l[1].rock;
            var repeatStart = l[1].rock;
            var heightStart = l[1].height;

            long max = 1000000000000;

            long loops = (max - repeatStart) / rocksDiff;
            long rocks = loops * rocksDiff + repeatStart;
            long height = loops * heightDiff + heightStart;
            
            // finally, however many rocks are still left to be dropped are sent through as a delta on the repeatStart
            long remainder = max - rocks;

            // loop through 
            // height(repeatStart + remainder) - height(repeatStart)
            var remainderHeight = DropRocks((int)(repeatStart + remainder), data) - heightStart;
            return height + remainderHeight;

        }

        public string testData = @">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
    }
}

// actual:    1514285714288
//            1514285714278
// actual:    2794999999455
//            2800000000000
// estimated: 560000000000004
// est        560000000000000
