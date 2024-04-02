// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Marbas.Enums;
using Marbas.Interface;
using Marbas.Job;
using SkiaSharp;
using XResize.Bot.Context;
using XResize.Bot.Enums;
using XResize.Bot.Interface;
using XResize.Bot.Service;

namespace XResize.Bot.Job
{
    public class ResizeJob : BaseJob, ISlowJob, IResizeJob
    {
        public BotTypeEnum Type { get; set; }
        public string FileName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int ImagePartsCount { get; set; }
        public SKBitmap UserImage { get; set; }
        public BotService BotService { get; private set; }
        public ApplicationContext ApplicationContext { get; private set; }

        public ResizeJob(BotService botService, ApplicationContext applicationContext, BotTypeEnum botTypeEnum, string username, string userId, SKBitmap userImage, string fileName)
        {
            BotService = botService;
            Type = botTypeEnum;
            UserName = username;
            UserId = userId;
            UserImage = userImage;
            FileName = fileName;
            ApplicationContext = applicationContext;

            ImagePartsCount = (int)Math.Ceiling(userImage.Width / 64.0) * (int)Math.Ceiling(userImage.Height / 64.0);
        }

        public override async Task Execute()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = await ApplicationContext.Resizer.Resize(UserImage);
            var stream = SKImage.FromPixels(result.PeekPixels()).Encode().AsStream();
            await BotService.SendDocument(UserId, stream, FileName);

            stopwatch.Stop();

            ApplicationContext.BenchmarkingTime = new TimeOnly(stopwatch.Elapsed.Ticks / ImagePartsCount);

            JobState = JobStateEnum.Complited;
        }
    }
}
