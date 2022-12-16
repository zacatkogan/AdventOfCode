using SkiaSharp;
using AdventOfCode;
using ImageMagick;
using ImageMagick.ImageOptimizers;
namespace AoC2022.Visualizations
{
    public class Day_14
    {
        List<Position> MovePriority = new List<Position>() {
            (0, 1),
            (-1, 1),
            (1, 1)
        };

        public string Data = new AdventOfCode.Utils.Downloader().GetData(14, 2022);

        public void Solve1Visualization()
        {
            var frames = new List<SKBitmap>();

            var grid = ParseDataToDict();
            var abyssY = grid.Keys.Max(k => k.Y);

            var minX = grid.Keys.Min(k => k.X);
            var maxX = grid.Keys.Max(k => k.X);

            var xOffset = (1000 - 200)/2;

            var maxY = abyssY + 10;

            var baseImage = new SKBitmap(200, maxY, SKColorType.Rgb565, SKAlphaType.Opaque);
            foreach (var p in grid.Keys)
            {
                baseImage.SetPixel(p.X-xOffset, p.Y, SKColors.DarkGray);
            }

            var tickCount = 0;
            var activeGrains = new List<SandGrain>();
            int? maxFrames = null;
            
            while (true)
            {
                if (tickCount % 3 == 0)
                {
                    // spawn a new sand-grain
                    activeGrains.Add(new SandGrain((500, 0)));
                }

                // draw a new frame every tick
                var frame = baseImage.Copy();
                foreach (var p in grid.AsEnumerable().Where(x => x.Value > 0)
                    .Concat(activeGrains.Select(x => KeyValuePair.Create(x.Pos, 2))))
                {
                    SKColor color = SKColors.Yellow;
                    var pos = p.Key;

                    if (p.Value == 2)
                        color = SKColors.LightYellow;
                    
                    frame.SetPixel(pos.X-xOffset, pos.Y, color);
                }

                frames.Add(frame);

                var settledGrains = new List<SandGrain>();
                
                // update active grains
                foreach (var grain in activeGrains)
                {
                    var move = MovePriority.FirstOrDefault(x => !grid.ContainsKey(grain.Pos + x));
                    if (move == null)
                    {
                        settledGrains.Add(grain);
                        grid.Add(grain.Pos, 1);
                        continue;
                    }

                    grain.Pos += move;

                    // remove any that have passed beyond the grid
                    if (grain.Pos.Y >= maxY)
                        settledGrains.Add(grain);
                }

                // see if any active grains have crossed into the abyss
                if (maxFrames == null && activeGrains.Any(x => x.Pos.Y > abyssY))
                {
                    maxFrames = tickCount + 10;
                }

                tickCount++;

                activeGrains = activeGrains.Except(settledGrains).ToList();

                if (tickCount >= maxFrames)
                    break;
            }

            // using (FileStream stream = System.IO.File.Create("./img.gif"))
            // using (SKManagedStream skStream = new SKManagedStream(stream))
            // using (SKCodec codec = SKCodec.Create(skStream))
            // {
            //     codec.
            // }


            Directory.CreateDirectory("./Visualizer/Day14");
            for (int i = 0; i < frames.Count; i++)
            {
                AdventOfCode.Visualizer.WriteToImage(frames[i], $"./Visualizer/Day14/{i:0000}.png");
            }

        }

        public void CreateGif()
        {
            var files = Directory.GetFiles("./Visualizer/Day14", "*.png").OrderBy(x => x);

            using (var imageCollection = new ImageMagick.MagickImageCollection())
            {
                foreach (var file in files)
                    imageCollection.Add(file);
                
                imageCollection[0].AnimationIterations = 0;
                //imageCollection.Last().AnimationDelay = 200;

                WriteWithAnimationDelay(imageCollection, 1);
                WriteWithAnimationDelay(imageCollection, 2);
                WriteWithAnimationDelay(imageCollection, 3);
                WriteWithAnimationDelay(imageCollection, 4);
                WriteWithAnimationDelay(imageCollection, 5);
                WriteWithAnimationDelay(imageCollection, 6);
                WriteWithAnimationDelay(imageCollection, 7);
                WriteWithAnimationDelay(imageCollection, 8);
                WriteWithAnimationDelay(imageCollection, 9);
                WriteWithAnimationDelay(imageCollection, 0);
            }
        }

        void WriteWithAnimationDelay(MagickImageCollection imageCollection, int animationDelay)
        {
            foreach (var image in imageCollection)
                image.AnimationDelay = animationDelay;

            imageCollection.Write($"Day_14_p1_{animationDelay}.gif");
        }

        public class SandGrain
        {
            public SandGrain(Position pos)
            {
                Pos = pos;
            }
            public Position Pos;
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
}
