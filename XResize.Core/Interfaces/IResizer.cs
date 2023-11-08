using SkiaSharp;

namespace ImResizer.Interfaces
{
    public interface IResizer
    {
        bool IsResize { get; set; }

        Task<SKBitmap?> Resize(SKBitmap image);

        IResizer Clone();
    }
}