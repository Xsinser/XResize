// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Marbas.Enums;
using Marbas.Interface;
using Marbas.Job;
using Marbas.Services;
using XResize.Bot.Context;
using XResize.Bot.Enums;
using XResize.Bot.Job;
using XResize.Bot.Service;

namespace XResize.Bot.Models.Work
{
    public class LoaderJob : BaseJob, IFastJob
    {
        public BotService BotService { get; private set; }
        public JobQueueService TaskQueryService { get; private set; }
        public ApplicationContext ApplicationContext { get; private set; }

        public string FileId { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }

        public LoaderJob(BotService botService,
                         JobQueueService taskQueryService,
                         ApplicationContext applicationContext,
                         string fileId,
                         string fileName,
                         string mimeType,
                         string userName,
                         string userId)
        {
            BotService = botService;
            TaskQueryService = taskQueryService;
            FileId = fileId;
            FileName = fileName;
            MimeType = mimeType;
            UserName = userName;
            UserId = userId;
            ApplicationContext = applicationContext;
        }

        public override async Task Execute()
        {
            var document = await BotService.GetDocument(FileId);
            TaskQueryService.AddNewJob(new ResizeJob(BotService, ApplicationContext, BotTypeEnum.Telegram, UserName, UserId, document, FileName));

            JobState = JobStateEnum.Complited;
        }
    }
}