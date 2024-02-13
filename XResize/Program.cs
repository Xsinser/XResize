using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XResize.Bot.Context;
using XResize.Bot.HostedServices;
using XResize.Bot.Services;

namespace XResize;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
        {
            services.AddLogging();
            services.AddSingleton<TaskQueueService, TaskQueueService>();
            services.AddSingleton<ApplicationContext, ApplicationContext>();
            services.AddSingleton<BotService, BotService>();
            services.AddHostedService<TelegramService>();
            services.AddHostedService<CleanerService>();
            services.AddHostedService<WorkerService>();
        }).Build();

        await host.RunAsync();
    }
}
