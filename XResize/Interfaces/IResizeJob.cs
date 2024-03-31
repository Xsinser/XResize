using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
