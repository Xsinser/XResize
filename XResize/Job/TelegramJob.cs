using Marbas.HostedServices;
using Marbas.Job;
using Marbas.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using XResize.Bot.Context;
using XResize.Bot.Models.Job;
using XResize.Bot.Models.Work;
using XResize.Bot.Service;

namespace XResize.Bot.Job
{
    public class TelegramJob : BaseJob
    {
        private readonly BotService _botService;
        private readonly JobQueueService _taskQueueService;
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger _logger;

        public TelegramJob(ILogger<TelegramJob> logger,
                               ApplicationContext applicationContext,
                               BotService botService,
                               JobQueueService taskQueueService) : base()
        {
            _botService = botService;
            _taskQueueService = taskQueueService;
            _applicationContext = applicationContext;
            _logger = logger;

            logger.LogInformation("TelegramService has been started");
        }

        public override async Task Execute()
        {
            var client = _botService.GetClient();
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            client.StartReceiving(updateHandler: HandleUpdateAsync,
                                  pollingErrorHandler: HandlePollingErrorAsync,
                                  receiverOptions: receiverOptions);
            _logger.LogInformation("Client has been started");
            JobState = Marbas.Enums.JobStateEnum.InProgress;
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
                            _taskQueueService.AddNewJob(new BenchmarkingJob(_botService, _applicationContext, chatId.ToString()));
                        }
                        break;

                    case "Мои задачи":
                        {
                            _taskQueueService.AddNewJob(new WaitingTimeJob(_botService, _applicationContext, _taskQueueService, Enums.BotTypeEnum.Telegram, message.From.Username, chatId.ToString()));
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
                    _taskQueueService.AddNewJob(new LoaderJob(_botService,
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
            JobState = Marbas.Enums.JobStateEnum.Complited;
            return Task.CompletedTask;
        }
    }
}