using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Services;

namespace XResize.Bot.HostedServices
{
    public class CleanerService : BaseService
    {
        protected readonly TaskQueryService _taskQueryService;
        public CleanerService(ILogger<CleanerService> logger, TaskQueryService taskQueryService) : base(logger)
        {
            _taskQueryService = taskQueryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

        }
    }
}
