﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Services;

namespace XResize.Bot.Models.Work
{
    public class BenchmarkingJob : Job
    {
        public BotService BotService { get; private set; }

        public BenchmarkingJob(BotService botService)
        {
            BotService = botService;
        }

        public override async Task Execute(SystemInfoService systemInfo)
        {
            if (systemInfo.BenchmarkingTime == null)
            {
                var stopwatch = new Stopwatch();

                stopwatch.Start();

                var result = await systemInfo.Resizer.Resize(UserImage);
                var stream = SKImage.FromPixels(result.PeekPixels()).Encode().AsStream();

                stopwatch.Stop();

                systemInfo.BenchmarkingTime = new TimeOnly(stopwatch.ElapsedTicks);
            }

            await BotService.SendMessage();
        }
    }
}
