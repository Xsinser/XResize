using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XResize.Bot.Services;

namespace XResize.Bot.HostedServices
{
    public class CleanerService : BaseService, IHostedService
    {
        protected readonly TaskQueueService taskQueryService;

        public CleanerService(ILogger<CleanerService> logger,
                              TaskQueueService taskQueryService) : base(logger)
        {
            this.taskQueryService = taskQueryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                taskQueryService.RemoveComplitedTask();

                await Task.Delay(100000);
            }
        }
    }
}