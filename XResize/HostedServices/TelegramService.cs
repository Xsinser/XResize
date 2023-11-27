using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using XResize.Bot.Models.Work;
using XResize.Bot.Services;

namespace XResize.Bot.HostedServices
{
    public class TelegramService : BaseService, IHostedService
    {
        private readonly BotService _botService;
        private readonly TaskQueryService _taskQueryService;

        public TelegramService(ILogger<TelegramService> logger, BotService botService, TaskQueryService taskQueryService) : base(logger)
        {
            _botService = botService;
            _taskQueryService = taskQueryService;
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
            Console.ReadLine();

            await Task.Delay(10000);
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (string.IsNullOrEmpty(message.Text))
            {

                var chatId = message.Chat.Id;
                switch (message.Text)
                {
                    case "/start":
                        {
                            await _botService.SendMessage(chatId, "Добро пожаловать!", cancellationToken);
                        }
                        break;
                    case "Бенчмаркинг":
                        {
                            await _botService.SendMessage(chatId, "Бенчмаркинг", cancellationToken);
                        }
                        break;
                    case "Мои задачи":
                        {
                            await _botService.SendMessage(chatId, "Мои задачи", cancellationToken);
                        }
                        break;
                };
            }
            else
            {
                _taskQueryService.AddNewTask(new DocumentJob(_botService, 
                    _taskQueryService, 
                    message.Document.
                    FileId, 
                    message.Document.FileName,
                    message.Document.MimeType,
                    message.From.Username, 
                    message.Chat.Id.ToString()));
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}