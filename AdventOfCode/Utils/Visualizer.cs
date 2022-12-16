using SkiaSharp;
using ImageMagick;
namespace AdventOfCode
{
    public static class Visualizer
    {
        public static SKBitmap ConvertToImage<T>(T[,] array, Dictionary<T, SKColor> colorMap) where T : notnull
        {
            var maxX = array.GetLength(0);
            var maxY = array.GetLength(1);
            
            var bmp = new SKBitmap(maxX, maxY, SKColorType.Rgb565, SKAlphaType.Opaque);

            for (int i = 0; i < maxX; i++)
            for (int j = 0; j < maxY; j++)
            {
                var groundType = array[i,j];
                var targetColor = colorMap[groundType];
                bmp.SetPixel(i,j, targetColor);
            }

            return bmp;
        }

        public static void WriteToImage<T>(T[,] array, Dictionary<T, SKColor> colorMap, string path) where T : notnull
        {
            var bmp = ConvertToImage(array, colorMap);
            var skData = SKImage.FromBitmap(bmp).Encode(SKEncodedImageFormat.Png, 100);

            using (var fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                skData.SaveTo(fileStream);
            }
        }

        public static void WriteToImage(SkiaSharp.SKBitmap bitmap, string path)
        {
            var skData = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Png, 100);

            using (var fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                skData.SaveTo(fileStream);
            }
        }

    }
}
