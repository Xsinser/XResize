using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using XResize.Bot.Context;
using XResize.Bot.HostedService;
using XResize.Bot.HostedServices;
using XResize.Bot.Interface;
using XResize.Bot.Service;
using XResize.Bot.Services;

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