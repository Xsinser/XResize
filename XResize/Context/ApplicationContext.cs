using ImResizer.Interfaces;
using ImResizer.Models.ESRGANSR4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XResize.Bot.Context
{
    public class ApplicationContext
    {
        private readonly ILogger _logger;

        public TimeOnly? BenchmarkingTime { get; set; } = null;

        private IResizer _resizer;

        public IResizer Resizer
        {
            get => _resizer.Clone();
            private set
            {
                _resizer = value;
            }
        }

        public string TelegramToken { get; set; }

        public ApplicationContext(ILogger<ApplicationContext> logger, IConfiguration configuration)
        {
            _logger = logger;
            TelegramToken = configuration.GetValue<string>("TelegramBotToken")!;
            logger.LogInformation($"TelegramBotToken: {TelegramToken}");
            Resizer = new EsrganSRFourProcessor(Path.Combine(System.IO.Directory.GetCurrentDirectory(), Path.Combine("OnnxModels","ESRGAN_SRx4_DF2KOST_official-ff704c303320.onnx")), configuration.GetValue<int>("ResizerThreadCount"), logger);
        }
    }
}