// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Marbas.HostedServices;
using Marbas.Interfaces;
using Marbas.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Marbas.HostedService
{
    public class QueueWorkerService<WorkerType> : BaseService, IHostedService
        where WorkerType : IJob
    {
        protected readonly JobQueueService _jobQueryService;

        public QueueWorkerService(ILogger<QueueWorkerService<WorkerType>> logger,
                                 JobQueueService jobQueryService) : base(logger)
        {
            _jobQueryService = jobQueryService;
            logger.LogInformation("WorkerService has been started");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var result = _jobQueryService.TryGetUncomplitedJob(out var job, typeof(WorkerType));

                if (result)
                    await job!.Execute();
                await Task.Delay(1000);
            }
        }
    }
}