using SkiaSharp;
using System.Diagnostics;
using XResize.Bot.Context;

namespace XResize.Bot.Utils
{
    public static class BenchmarkingUtils
    {
        private static SKBitmap _benchmarkingImage = SKBitmap.Decode("BenchmarkingTest");

        public static async Task<TimeOnly> CalculateBenchmarkingTime(ApplicationContext applicationContext)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = await applicationContext.Resizer.Resize(_benchmarkingImage);
            SKImage.FromPixels(result.PeekPixels()).Encode().AsStream();

            stopwatch.Stop();

            return new TimeOnly(stopwatch.ElapsedTicks);
        }
    }
}