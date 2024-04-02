// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.ML.OnnxRuntime;
using SkiaSharp;

namespace XResize.Models
{
    public abstract class Processor
    {
        public abstract Task<SKBitmap?> Resize(SKBitmap image);

        protected abstract void ImageToSmallImages(int numberElementByHeight, int numberElementByWidth,
                                                            SKBitmap image, out SKBitmap[][] bitmaps);

        protected abstract SKBitmap ImagesToFullImage(in SKBitmap[][] bitmaps, int width, int height);

        protected abstract SKBitmap Resizer(SKBitmap bitmap, InferenceSession session);

        protected abstract SKBitmap SaveAsFile(float[] bufer);
    }
}