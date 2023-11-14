using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Enums;

namespace XResize.Bot.Models
{
    public class BotTaskModel
    {
        public BotTypeEnum Type { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public SKBitmap Result { get; set; }
        public SKBitmap UserImage { get; set; }
        public bool IsComplited { get; set; }
        public bool IsSended { get; set; }
        public bool IsInProgress { get; set; }

        public BotTaskModel(BotTypeEnum botTypeEnum, string username, string userId, SKBitmap userImage)
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
    }
}
