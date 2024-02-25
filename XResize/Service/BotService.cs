using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using XResize.Bot.Enums;

namespace XResize.Bot.Service
{
    /// <summary>
    /// Single
    /// </summary>
    public class BotService
    {
        private readonly TelegramBotClient _client;
        private readonly ILogger _logger;

        public BotTypeEnum BotType { get; private set; }

        public BotService(ILogger<BotService> logger)
        {
            BotType = BotTypeEnum.Telegram;
            _logger = logger;
            _client = new TelegramBotClient("6867996261:AAETOAZCUBV2cBP8C-s26TDz91_Lzqz8cIA");
        }

        public TelegramBotClient GetClient()
        {
            return _client;
        }

        public async Task SendMessage(ChatId chatId, string messageText, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboard = new(new[] { new KeyboardButton[] { "Бенчмаркинг", "Мои задачи" } }) { ResizeKeyboard = true };
            await _client.SendTextMessageAsync(chatId, messageText, replyMarkup: replyKeyboard, cancellationToken: cancellationToken);
        }

        public async Task SendMessage(ChatId chatId, string messageText)
        {
            ReplyKeyboardMarkup replyKeyboard = new(new[] { new KeyboardButton[] { "Бенчмаркинг", "Мои задачи" } }) { ResizeKeyboard = true };
            await _client.SendTextMessageAsync(chatId, messageText, replyMarkup: replyKeyboard);
        }

        public async Task SendDocument(ChatId chatId, Stream documentStream, string fileName)
        {
            await _client.SendDocumentAsync(chatId, InputFile.FromStream(documentStream, fileName));
        }

        public async Task<SKBitmap> GetDocument(string fileId)
        {
            await using var stream = new MemoryStream();
            var fileInfo  = await _client.GetFileAsync(fileId);
            await _client.DownloadFileAsync(fileInfo.FilePath!, stream);
            stream.Position = 0;
            return SKBitmap.Decode(stream);
        }
    }
}
