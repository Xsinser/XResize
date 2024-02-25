using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Context;
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
