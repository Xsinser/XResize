using ImResizer.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Enums;
using XResize.Bot.Services;

namespace XResize.Bot.Models.Work
{
    public class ResizeJob: Job
    {
        public BotTypeEnum Type { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public SKBitmap Result { get; set; }
        public SKBitmap UserImage { get; set; }

        private IResizer _resizer { get; set; }

        public ResizeJob(BotTypeEnum botTypeEnum, string username, string userId, SKBitmap userImage)
        {
            Type = botTypeEnum;
            UserName = username;
            UserId = userId;
            UserImage = userImage;
            Result = null;
            IsComplited = false;
            IsSended = false;
            IsInProgress = false;
        }

        public override Task Execute(SystemInfoService systemInfo)
        {
            throw new NotImplementedException();
        }
    }
}
