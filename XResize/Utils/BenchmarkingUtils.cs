using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Context;

namespace XResize.Bot.Utils
{
    public static class BenchmarkingUtils
    {
        private static SKBitmap _benchmarkingImage = SKBitmap.Decode("64.jpg");

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
