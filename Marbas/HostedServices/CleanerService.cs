// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Marbas.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Marbas.HostedServices
{
    public class CleanerService : BaseService, IHostedService
    {
        protected readonly JobQueueService taskQueryService;

        public CleanerService(ILogger<CleanerService> logger,
                              JobQueueService taskQueryService) : base(logger)
        {
            this.taskQueryService = taskQueryService;
            logger.LogInformation("CleanerService has been started");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                taskQueryService.RemoveComplitedJob();

                await Task.Delay(100000);
            }
        }
    }
}