using Microsoft.ML.OnnxRuntime;
using SkiaSharp;
using System.Drawing;

namespace XResize.Models
{
    public abstract class Processor
    {
        public abstract Task<SKBitmap?> Resize(SKBitmap  image);

        private protected abstract void ImageToSmallImages(int numberElementByHeight, int numberElementByWidth,
                                                           ref SKBitmap  image, out SKBitmap [][] bitmaps);

        private protected abstract SKBitmap  ImagesToFullImage(in SKBitmap [][] bitmaps, int width, int height);

        private protected abstract SKBitmap  Resizer(ref SKBitmap  bitmap, ref InferenceSession session);

        private protected abstract SKBitmap  SaveAsFile(ref float[] bufer);
    }
}
