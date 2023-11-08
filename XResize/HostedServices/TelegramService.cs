using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XResize.Bot.HostedServices
{
    public class TelegramService : IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _stoppingCts = new ();

        public TelegramService(ILogger<TelegramService> logger)
        {
            _logger = logger;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)            
                return;            

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                                                              cancellationToken));
            }

        }

        protected async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(10000);
        }

        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}
