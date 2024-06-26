﻿using Microsoft.Extensions.Logging;

namespace XResize.Bot.HostedServices
{
    public abstract class BaseService : IDisposable
    {
        private Task _executingTask;
        protected readonly ILogger _logger;
        private readonly CancellationTokenSource _stoppingCts = new();

        public BaseService(ILogger<BaseService> logger)
        {
            _logger = logger;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            if (_executingTask.IsCompleted)
                return _executingTask;

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

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}