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
    public abstract class Job
    {
        //public bool IsComplited { get; set; }
        //public bool IsSended { get; set; }
        //public bool IsInProgress { get; set; }

        public JobStateEnum JobState { get; set; }

        public abstract Task Execute(ApplicationContext appContext);
    }
}
