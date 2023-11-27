//using ImResizer.Interfaces;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using XResize.Bot.Models.Work;
//using XResize.Bot.Services;

//namespace XResize.Bot.HostedServices
//{
//    public class ResizerService : BaseService, IHostedService
//    {
//        private object _lock = new();
//        private int _countThreads;

//        private List<ResizeJob> _jobQuery;

//        private IResizer[] _resizers;

//        protected readonly TaskQueryService _taskQueryService;
//        public ResizerService(ILogger<ResizerService> logger,
//                              IResizer resizer) : base(logger)
//        {
//            this._countThreads = 1;
//            this._jobQuery = new(); 
            
//            _resizers = new IResizer[_countThreads];

//            for (int i = 0; i < _countThreads; i++)
//            {
//                _resizers[i] = resizer.Clone();
//            }
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (true)
//            {
//                await Task.Delay(Config.ApplicationSettings.WorkerTimeout);

//                var _freeJob = _jobQuery.FirstOrDefault(x => !x.IsInProgress);
//                if (_freeJob == null)
//                    continue;

//                var _freeThread = _resizers.FirstOrDefault(x => !x.IsResize);
//                if (_freeThread == null)
//                    continue;

//                lock (_lock)
//                {
//                    Task.Run(async () =>
//                    {
//                        try
//                        {
//                            _freeJob.IsInProgress = true;
//                            var result = await _freeThread.Resize(_freeJob.UserImage);
//                            _freeJob.Result = result;

//                            lock (_lock)
//                                _filesQuery.Remove(_freeFile);
//                        }
//                        catch (Exception ex)
//                        {

//                        }
//                    });
//                }
//            }
//        }
//    }
//}
