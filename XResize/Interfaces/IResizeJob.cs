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