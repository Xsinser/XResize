using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Services;

namespace XResize.Bot.Models.Work
{
    public abstract class Job
    {
        public bool IsComplited { get; set; }
        public bool IsSended { get; set; }
        public bool IsInProgress { get; set; }

        public abstract Task Execute(SystemInfoService systemInfo);
    }
}
