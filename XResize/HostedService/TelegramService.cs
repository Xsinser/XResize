using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using XResize.Bot.Context;
using XResize.Bot.Job;
using XResize.Bot.Models.Job;
using XResize.Bot.Models.Work;
using XResize.Bot.Service;
using XResize.Bot.Services;

namespace XResize.Bot.HostedServices
{
    public class TelegramService : BaseService, IHostedService
    {
        private readonly BotService _botService;
        private readonly TaskQueueService _taskQueueService;
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger _logger;

        public TelegramService(ILogger<TelegramService> logger, 
                               ApplicationContext applicationContext,
                               BotService botService, 
                               TaskQueueService taskQueueService) : base(logger)
        {
            _botService = botService;
            _taskQueueService = taskQueueService;
            _applicationContext = applicationContext;
            _logger = logger;

            logger.LogInformation("TelegramService has been started");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var client = _botService.GetClient();
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            client.StartReceiving(updateHandler: HandleUpdateAsync,
                                  pollingErrorHandler: HandlePollingErrorAsync,
                                  receiverOptions: receiverOptions,
                                  cancellationToken: stoppingToken);
            _logger.LogInformation("Client has been started");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            var chatId = message.Chat.Id;

            _logger.LogInformation($"{chatId} send message");

            if (!string.IsNullOrEmpty(message.Text))
            {
                switch (message.Text)
                {
                    case "/start":
                        {
                            await _botService.SendMessage(chatId, "Добро пожаловать!", cancellationToken);
                        }
                        break;

                    case "Бенчмаркинг":
                        {
                            _taskQueueService.AddNewTask(new BenchmarkingJob(_botService, _applicationContext, chatId.ToString()));
                        }
                        break;

                    case "Мои задачи":
                        {
                            _taskQueueService.AddNewTask(new WaitingTimeJob(_botService, _applicationContext, _taskQueueService, Enums.BotTypeEnum.Telegram, message.From.Username, chatId.ToString()));
                        }
                        break;
                };
            }
            else
            {
                if (message.Document == null)
                    _botService.SendMessage(message.Chat.Id.ToString(), "Файл не прикреплен!");
                else if (message.Document.FileName.Split(".").Last().ToLower() is not "jpg" or "jpeg")
                    _botService.SendMessage(message.Chat.Id.ToString(), "Поддерживается только jpeg формат");
                else
                    _taskQueueService.AddNewTask(new LoaderJob(_botService,
                        _taskQueueService,
                        _applicationContext,
                        message.Document.
                        FileId,
                        message.Document.FileName,
                        message.Document.MimeType,
                        message.From.Username,
                        message.Chat.Id.ToString()));
            }
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}