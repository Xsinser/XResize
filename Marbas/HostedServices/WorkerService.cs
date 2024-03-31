using Marbas.HostedServices;
using Marbas.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Marbas.HostedService
{
    public class WorkerService<WorketRype> : BaseService, IHostedService
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
                var result = _taskQueryService.TryGetUncomplitedJob(out var task, typeof(WorketRype));

                if (result)
                    await task!.Execute();
                await Task.Delay(1000);
            }
        }
    }
}