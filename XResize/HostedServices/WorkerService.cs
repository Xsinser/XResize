using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Context;
using XResize.Bot.Models.Work;
using XResize.Bot.Services;

namespace XResize.Bot.HostedServices
{
    public class WorkerService : BaseService, IHostedService
    {
        protected readonly TaskQueueService _taskQueryService;
        protected readonly ApplicationContext _appContext;

        public WorkerService(ILogger<WorkerService> logger, ApplicationContext appContext, TaskQueueService taskQueryService) : base(logger)
        {
            _taskQueryService = taskQueryService;
            _appContext = appContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var result = _taskQueryService.TryGetUncomplitedTask(out var task);

                if(result)
                    await task.Execute(_appContext);
                await Task.Delay(1000);
            }
        }
    }
}
