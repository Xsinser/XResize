using ImResizer.Interfaces;
using ImResizer.Models.ESRGANSR4;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XResize.Bot.Services
{
    public class SystemInfoService
    {
        private readonly ILogger _logger;

        public IResizer Resizer { get; private set; }

        public SystemInfoService(ILogger<SystemInfoService> logger)
        {
            _logger = logger; 
            Resizer = new EsrganSRFourProcessor("", 1, logger);
        }
    }
}
