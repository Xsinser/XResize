using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Services;

namespace XResize.Bot.HostedServices
{
    internal class CallbackService : BaseService, IHostedService
    {
        protected readonly TaskQueryService _taskQueryService;
        public CallbackService(ILogger<CallbackService> logger, TaskQueryService taskQueryService) : base(logger)
        {
            _taskQueryService = taskQueryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

        }
    }
}
