using ImResizer.Interfaces;
using ImResizer.Models.ESRGANSR4;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XResize.Bot.Services;

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


        public ApplicationContext(ILogger<ApplicationContext> logger)
        {
            _logger = logger;
            Resizer = new EsrganSRFourProcessor(@"OnnxModels\ESRGAN_SRx4_DF2KOST_official-ff704c303320.onnx", 1, logger);
        }
    }
}
