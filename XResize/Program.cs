// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Marbas.HostedService;
using Marbas.HostedServices;
using Marbas.Interface;
using Marbas.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XResize.Bot.Context;
using XResize.Bot.Job;
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
            services.AddSingleton<JobQueueService>();// очередь задач
            services.AddSingleton<ApplicationContext>();// контекст приложения
            services.AddSingleton<BotService>();// обретка для упрощения работы с Telegram.Api.Lib

            services.AddHostedService<CleanerService>();//чистит очередь от выполненных задач
            services.AddHostedService<QueueWorkerService<ISlowJob>>();/// получает из очереди тяжелые таски, и выполняет
            services.AddHostedService<QueueWorkerService<INormalJob>>();/// получает из очереди обычные таски, и выполняет
            services.AddHostedService<QueueWorkerService<IFastJob>>();/// получает из очереди легкие таски, и выполняет

            services.AddScoped<TelegramJob>();
            services.AddHostedService<SingleJobWorkerService<TelegramJob>>();/// выполняет Job, переданный в TJob, 1 раз в отдельном потоке. Не ожидает завершения Job
        }).Build();

        await host.RunAsync();
    }
}
