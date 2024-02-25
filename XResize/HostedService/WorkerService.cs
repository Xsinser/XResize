using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.HostedServices;
using XResize.Bot.Job;
using XResize.Bot.Services;

namespace XResize.Bot.HostedService
{
    internal class WorkerService<WorketRype> : BaseService, IHostedService
    {
        protected readonly TaskQueueService _taskQueryService;

        public WorkerService(ILogger<WorkerService<WorketRype>> logger,
                                 TaskQueueService taskQueryService) : base(logger)
        {
            _taskQueryService = taskQueryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var result = _taskQueryService.TryGetUncomplitedTask(out var task, typeof(WorketRype));

                if (result)
                    try
                    {
                        await task!.Execute();
                    }
                    catch (Exception ex)
                    {

                    }
                await Task.Delay(1000);
            }
        }
    }
}
