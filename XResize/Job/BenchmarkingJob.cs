﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Marbas.Enums;
using Marbas.Interface;
using Marbas.Job;
using XResize.Bot.Context;
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