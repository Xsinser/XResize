using ImResizer.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SkiaSharp;
using XResize.Models;

namespace ImResizer.Models.ESRGANSR4
{
    public class EsrganSRFourProcessor : Processor, IResizer
    {
        private const int InputWidth = 64, OutputWidth = 256,
                          InputHeight = 64, OutputHeight = 256,
                          CountChannels = 3;

        private const float InputWidthF = 64,
                            InputHeightF = 64;

        private const int OutputArraySize = OutputWidth * OutputHeight,
                          InputArraySize = InputWidth * InputHeight;

        private readonly string ModelPath;

        private int _maxTaskCount = 2;
        private InferenceSession[] _models;
        private ILogger _logger;

        public bool IsResize { get; set; }

        public EsrganSRFourProcessor(string modelPath, int maxTaskCount, ILogger logger)
        {
            ModelPath = modelPath;
            this._maxTaskCount = maxTaskCount;
            this._logger = logger;
            Initialize();
        }

        private void Initialize()
        {
            this._models = new InferenceSession[_maxTaskCount];

            for (int i = 0; i < _maxTaskCount; i++)
            {
                this._models[i] = new InferenceSession(ModelPath);
            }
        }

        protected override void ImageToSmallImages(int numberElementByHeight, int numberElementByWidth, SKBitmap image, out SKBitmap[][] bitmaps)
        {
            bitmaps = new SKBitmap[numberElementByHeight][];

            int arrayCoef = 0;
            if (numberElementByHeight == numberElementByWidth ||
                numberElementByWidth > numberElementByHeight)
                arrayCoef = numberElementByHeight;
            else
                arrayCoef = numberElementByWidth;

            for (int height = 0; height < bitmaps.Length; height++)
            {
                bitmaps[height] = new SKBitmap[numberElementByWidth];
                for (int width = 0; width < bitmaps[height].Length; width++)
                    bitmaps[height][width] = new SKBitmap(InputWidth, InputHeight);
            }

            for (var w = 0; w < image.Width; w++)
            {
                for (var h = 0; h < image.Height; h++)
                {
                    var currentH = h == 0 ? 0 : h;
                    var currentW = w == 0 ? 0 : w;

                    int currentElementPixelW = w - ((int)(currentW / InputWidthF)) * InputWidth;
                    int currentElementPixelH = h - ((int)(currentH / InputHeightF)) * InputHeight;

                    var elementWidthNumber = (int)(currentW / InputWidthF);
                    var elementHeightNumber = (int)(currentH / InputHeightF);

                    bitmaps[elementHeightNumber][elementWidthNumber].SetPixel(currentElementPixelW, currentElementPixelH, image.GetPixel(w, h));
                }
            }
        }

        protected override SKBitmap ImagesToFullImage(in SKBitmap[][] bitmaps, int width, int height)
        {
            //количество блоков по высоте и ширине
            int numberElementByWidth = 0;
            int numberElementByHeight = 0;

            //добавлялись ли блоки, если объекты влезают за размер
            bool wHasEmptyBlock = false;
            bool hHasEmptyBlock = false;

            if ((width % OutputWidth) > 0)
            {
                numberElementByWidth = (width / OutputWidth + 1);
                wHasEmptyBlock = true;
            }
            else
                numberElementByWidth = (width / OutputWidth);

            if ((height % OutputHeight) > 0)
            {
                numberElementByHeight = (height / OutputHeight + 1);
                hHasEmptyBlock = true;
            }
            else
                numberElementByHeight = (height / OutputHeight);

            var result = new SKBitmap(width, height);

            for (int h = 0; h < bitmaps.Length; h++)
                for (int w = 0; w < bitmaps[h].Length; w++)
                {
                    var currentBitmap = bitmaps[h][w];

                    var bitmapWidth = wHasEmptyBlock && w + 1 == numberElementByWidth ? width - (numberElementByWidth - 1) * OutputWidth : currentBitmap.Width;
                    var bitmapHeight = hHasEmptyBlock && h + 1 == numberElementByHeight ? height - (numberElementByHeight - 1) * OutputHeight : currentBitmap.Height;

                    for (var bitmapW = 0; bitmapW < bitmapWidth; bitmapW++)
                        for (var bitmapH = 0; bitmapH < bitmapHeight; bitmapH++)
                            result.SetPixel((w * OutputWidth) + bitmapW,
                                            (h * OutputHeight) + bitmapH,
                                            currentBitmap.GetPixel(bitmapW, bitmapH));
                }
            return result;
        }

        protected override SKBitmap Resizer(SKBitmap bitmap, InferenceSession session)
        {
            var pixels = new int[InputArraySize];

            Tensor<float> input = new DenseTensor<float>(new[] { 1, CountChannels, InputWidth, InputHeight });
            Tensor<float> output = new DenseTensor<float>(new[] { 1, CountChannels, OutputWidth, OutputHeight });

            for (var i = 0; i < bitmap.Width; i++)
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var colr = bitmap.GetPixel(i, j);

                    input[0, 0, j, i] = ((float)(colr.Blue) / (float)255.0);
                    input[0, 1, j, i] = ((float)(colr.Green) / (float)255.0);
                    input[0, 2, j, i] = ((float)(colr.Red) / (float)255.0);
                }

            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("input.1", input) };

            var results = session.Run(inputs) as List<DisposableNamedOnnxValue>;
            var bytes = results.First().AsEnumerable<float>().ToArray();

            return SaveAsFile(bytes);
        }

        protected override SKBitmap SaveAsFile(float[] bufer)
        {
            List<SKColor> color = new List<SKColor>();
            for (int i = 0; i < OutputArraySize; i += 1)
            {
                int b = (int)Math.Round((bufer[i] * (float)255.0), MidpointRounding.AwayFromZero);
                int g = (int)Math.Round((bufer[i + OutputArraySize * 1] * (float)255.0), MidpointRounding.AwayFromZero);
                int r = (int)Math.Round((bufer[i + OutputArraySize * 2] * (float)255.0), MidpointRounding.AwayFromZero);

                color.Add(new SKColor((byte)(r > 255 ? 255 : r < 0 ? 0 : r), (byte)(g > 255 ? 255 : g < 0 ? 0 : g), (byte)(b > 255 ? 255 : b < 0 ? 0 : b)));
            }

            var result = new SKBitmap(OutputWidth, OutputHeight);

            int colorNum = 0;
            for (int x = 0; x < OutputHeight; x++)
            {
                for (int y = 0; y < OutputWidth; y++)
                {
                    result.SetPixel(y, x, color[colorNum++]);
                }
            }
            return result;
        }

        public override async Task<SKBitmap?> Resize(SKBitmap image)
        {
            if (IsResize)
                throw new Exception();
            SKBitmap? result = null;

            try
            {
                IsResize = true;

                _logger.LogInformation("Start resize image");

                int numberElementByWidth = (image.Width % InputWidth) > 0 ? (image.Width / InputWidth + 1) : (image.Width / InputWidth);
                int numberElementByHeight = (image.Height % InputHeight) > 0 ? (image.Height / InputHeight + 1) : (image.Height / InputHeight);

                ImageToSmallImages(numberElementByHeight, numberElementByWidth, image, out SKBitmap[][] elements);

                var resizedBitmaps = new SKBitmap[numberElementByHeight][];

                for (int height = 0; height < numberElementByHeight; height++)
                    resizedBitmaps[height] = new SKBitmap[numberElementByWidth];

                //h,w
                var threads = new List<(int, int)>[_maxTaskCount].Select(x => new List<(int, int)>()).ToArray();

                int lastThreadIndex = _maxTaskCount - 1;

                for (int imageHeightNumber = 0, threadsNumber = 0; imageHeightNumber < elements.Length; imageHeightNumber++)
                    for (int imageWidthNumber = 0; imageWidthNumber < elements[imageHeightNumber].Length; imageWidthNumber++)
                    {
                        threads[threadsNumber].Add((imageHeightNumber, imageWidthNumber));

                        threadsNumber = threadsNumber == lastThreadIndex ? 0 : threadsNumber + 1;
                    }

                List<Task> tasks = new List<Task>();

                for (int i = 0; i < threads.Length; i++)
                {
                    var currentIterationIndex = i;

                    tasks.Add(Task.Run(() =>
                    {
                        foreach (var item in threads[currentIterationIndex])
                            resizedBitmaps[item.Item1][item.Item2] = Resizer(elements[item.Item1][item.Item2], _models[currentIterationIndex]);
                    }));
                }

                await Task.WhenAll(tasks);

                result = ImagesToFullImage(in resizedBitmaps, image.Width * 4, image.Height * 4);

                IsResize = false;

                _logger.LogInformation("Resize complited");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Resize");
            }
            return result;
        }

        /// <summary>
        /// IResizer typeof EsrganSRFourProcessor -> EsrganSRFourProcessor -> new IReszer typeof EsrganSRFourProcessor
        /// </summary>
        /// <returns></returns>
        public IResizer Clone() =>
            new EsrganSRFourProcessor(this.ModelPath, this._maxTaskCount, _logger);
    }
}