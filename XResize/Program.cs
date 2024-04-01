using Marbas.HostedService;
using Marbas.HostedServices;
using Marbas.Interface;
using Marbas.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XResize.Bot.Context;
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
            services.AddSingleton<JobQueueService>();
            services.AddSingleton<ApplicationContext>();
            services.AddSingleton<BotService>();
            services.AddScoped<TelegramService>();
            services.AddHostedService<CleanerService>();
            services.AddHostedService<QueueWorkerService<ISlowJob>>();
            services.AddHostedService<QueueWorkerService<INormalJob>>();
            services.AddHostedService<QueueWorkerService<IFastJob>>();
            services.AddHostedService<SingleJobWorkerService<TelegramService>>();
        }).Build();

        await host.RunAsync();
    }
}