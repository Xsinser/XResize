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

        public TimeOnly BenchmarkingTime { get; set; }

        public IResizer Resizer { get; private set; }

        public ApplicationContext(ILogger<ApplicationContext> logger)
        {
            _logger = logger;
            Resizer = new EsrganSRFourProcessor("", 1, logger);
        }
    }
}
