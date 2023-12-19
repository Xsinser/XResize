using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Models.Work;
using XResize.Bot.Services;

namespace XResize.Bot.HostedServices
{
    public class WorkerService : BaseService, IHostedService
    {
        protected readonly TaskQueryService _taskQueryService;
        protected readonly SystemInfoService _systemInfoService;

        public WorkerService(ILogger<WorkerService> logger, SystemInfoService systemInfoService, TaskQueryService taskQueryService) : base(logger)
        {
            _taskQueryService = taskQueryService;
            _systemInfoService = systemInfoService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var result = _taskQueryService.TryGetUncomplitedTask(out var task);

                if(result)
                    await task.Execute(_systemInfoService);
                await Task.Delay(1000);
            }
        }
    }
}
