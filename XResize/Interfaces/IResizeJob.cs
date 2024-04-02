// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using XResize.Bot.Enums;

namespace XResize.Bot.Interface
{
    public interface IResizeJob
    {
        BotTypeEnum Type { get; set; }
        string UserId { get; set; }
        int ImagePartsCount { get; set; }
    }
}