using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XResize.Bot.HostedServices;
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
            logger.LogInformation("WorkerService has been started");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var result = _taskQueryService.TryGetUncomplitedTask(out var task, typeof(WorketRype));

                if (result)
                    await task!.Execute();
                await Task.Delay(1000);
            }
        }
    }
}