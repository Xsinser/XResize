using ImResizer.Interfaces;
using ImResizer.Models.ESRGANSR4;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace XResizer.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => { });
            ILogger logger = factory.CreateLogger("Program");
            IResizer resizer = new EsrganSRFourProcessor(@"D:\Проекты\XResize\XResize\XResize.Core\OnnxModels\ESRGAN_SRx4_DF2KOST_official-ff704c303320.onnx",
                                             2,
                                             logger);
            SKBitmap bitmap = SKBitmap.Decode("D:\\Unity Games\\Site\\Архив\\TestConsole\\64.jpg");
            SKBitmap result = null;
            result = Task.Run(async () => await resizer.Resize(bitmap)).Result;
            SKCanvas canvas = new SKCanvas(result);
            SKData data = SKImage.FromBitmap(result).Encode(SKEncodedImageFormat.Jpeg, 100);
            using (var s = File.Open("D:\\Unity Games\\Site\\Архив\\TestConsole\\256.jpg", FileMode.CreateNew))
                data.SaveTo(s);
        }
    }
}