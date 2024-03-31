using Marbas.Enums;
using Marbas.Interfaces;

namespace Marbas.Job
{
    public abstract class BaseJob: IJob
    {
        public JobStateEnum JobState { get; set; }

        public BaseJob()
        {
            JobState = JobStateEnum.InQueue;
        }

        public abstract Task Execute();
    }
}