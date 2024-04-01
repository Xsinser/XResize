using Marbas.HostedService;
using Marbas.Interfaces;
using Marbas.Job;
using Marbas.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marbas.HostedServices
{
    public class SingleJobWorkerService<TJob> : BaseService, IHostedService
        where TJob : BaseJob
    {
        private readonly TJob _job;

        public SingleJobWorkerService(ILogger<QueueWorkerService<TJob>> logger,
                                      TJob job) : base(logger)
        {
            _job = job;
            logger.LogInformation("WorkerService has been started");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Task.Run(() => _job!.Execute());
            }
        }
    }
}
