using XResize.Bot.Enums;

namespace XResize.Bot.Job
{
    public abstract class BaseJob
    {
        public JobStateEnum JobState { get; set; }

        public BaseJob()
        {
            JobState = JobStateEnum.InQueue;
        }

        public abstract Task Execute();
    }
}