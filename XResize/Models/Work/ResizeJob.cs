using ImResizer.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Context;
using XResize.Bot.Enums;
using XResize.Bot.Services;

namespace XResize.Bot.Models.Work
{
    public class ResizeJob : Job
    {
        public BotTypeEnum Type { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public SKBitmap UserImage { get; set; }
        public BotService BotService { get; private set; }

        public ResizeJob(BotService botService, BotTypeEnum botTypeEnum, string username, string userId, SKBitmap userImage)
        {
            BotService = botService;
            Type = botTypeEnum;
            UserName = username;
            UserId = userId;
            UserImage = userImage;
            JobState = JobStateEnum.InQueue;
        }

        public override async Task Execute(ApplicationContext appContext)
        {
            var result = await appContext.Resizer.Resize(UserImage);
            var stream = SKImage.FromPixels(result.PeekPixels()).Encode().AsStream();
            await BotService.SendDocument(UserId, stream);
        }
    }
}
