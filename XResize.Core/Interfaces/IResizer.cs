// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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