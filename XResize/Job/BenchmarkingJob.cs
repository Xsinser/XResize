using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Context;
using XResize.Bot.Enums;
using XResize.Bot.Interface;
using XResize.Bot.Job;
using XResize.Bot.Service;
using XResize.Bot.Utils;

namespace XResize.Bot.Models.Job
{
    public class BenchmarkingJob : BaseJob, INormalJob
    {
        public BotService BotService { get; private set; }
        public ApplicationContext ApplicationContext { get; private set; }
        public string UserId { get; set; }

        public BenchmarkingJob(BotService botService, ApplicationContext applicationContext, string userId)
        {
            BotService = botService;
            UserId = userId;
            ApplicationContext = applicationContext;
        }

        public override async Task Execute()
        {
            if (ApplicationContext.BenchmarkingTime == null)
            {
                ApplicationContext.BenchmarkingTime = await BenchmarkingUtils.CalculateBenchmarkingTime(ApplicationContext);
            }

            await BotService.SendMessage(UserId, $"64x64 pixel excecution time {ApplicationContext.BenchmarkingTime?.ToLongTimeString()}");

            JobState = JobStateEnum.Complited;
        }
    }
}
