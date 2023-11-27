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
            services.AddHostedService<TelegramService>();
            services.AddSingleton<BotService>();
            services.AddSingleton<TaskQueryService>();
        }).Build();

        await host.RunAsync();
    }
}