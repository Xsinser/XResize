using XResize.Bot.Enums;

namespace XResize.Bot.Interface
{
    public interface ISlowJob
    {
        BotTypeEnum Type { get; set; }
        string UserId { get; set; }
        int ImagePartsCount { get; set; }
    }
}