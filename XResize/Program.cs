using Marbas.HostedService;
using Marbas.HostedServices;
using Marbas.Interface;
using Marbas.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XResize.Bot.Context;
using XResize.Bot.HostedServices;
using XResize.Bot.Service;

namespace XResize;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.AddJsonFile("appsettings.json");
            configBuilder.AddEnvironmentVariables();
        });
        var host = builder.ConfigureServices(services =>
        {
            services.AddLogging();
            services.AddSingleton<TaskQueueService, TaskQueueService>();
            services.AddSingleton<ApplicationContext, ApplicationContext>();
            services.AddSingleton<BotService, BotService>();
            services.AddHostedService<CleanerService>();
            services.AddHostedService<WorkerService<ISlowJob>>();
            services.AddHostedService<WorkerService<INormalJob>>();
            services.AddHostedService<WorkerService<IFastJob>>();
            services.AddHostedService<TelegramService>();
        }).Build();

        await host.RunAsync();
    }
}