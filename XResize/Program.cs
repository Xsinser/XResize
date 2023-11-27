using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddSingleton<TaskQueryService, TaskQueryService>();
            services.AddSingleton<BotService, BotService>();
            services.AddHostedService<TelegramService>();            
			services.AddHostedService<CleanerService>();            
			services.AddHostedService<CallbackService >();
			services.AddHostedService<WorkerService >();            
        }).Build();

        await host.RunAsync();
    }
}
