using Marbas.Enums;
using Marbas.Interface;
using Marbas.Job;
using Marbas.Services;
using XResize.Bot.Context;
using XResize.Bot.Enums;
using XResize.Bot.Interface;
using XResize.Bot.Service;
using XResize.Bot.Utils;

namespace XResize.Bot.Job
{
    public class WaitingTimeJob : BaseJob, INormalJob
    {
        public BotTypeEnum Type { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public BotService BotService { get; private set; }
        public JobQueueService TaskQueryService { get; private set; }
        public ApplicationContext ApplicationContext { get; private set; }

        public WaitingTimeJob(BotService botService, ApplicationContext applicationContext, JobQueueService taskQueryService, BotTypeEnum botTypeEnum, string username, string userId)
        {
            BotService = botService;
            TaskQueryService = taskQueryService;
            Type = botTypeEnum;
            UserName = username;
            UserId = userId;
            ApplicationContext = applicationContext;
        }

        public override async Task Execute()
        {
            if (ApplicationContext.BenchmarkingTime == null)
            {
                ApplicationContext.BenchmarkingTime = await BenchmarkingUtils.CalculateBenchmarkingTime(ApplicationContext);
            }

            List<int> waitingTimes = new();
            int partsCount = 0;
            var jobs = TaskQueryService.GetJobs<ISlowJob>()
                .Where(x => x is IResizeJob && ((IResizeJob)x).Type == BotService.BotType)
                .Select(x => x as IResizeJob)
                .ToList();

            foreach (var job in jobs)
            {
                partsCount += job.ImagePartsCount;
                if (job.UserId == UserId)
                    waitingTimes.Add(partsCount);
            }

            for (int i = 0; i < waitingTimes.Count; i++)
            {
                await BotService.SendMessage(UserId, $"Время ожидания задачи {i + 1} {Math.Round(((decimal)(ApplicationContext.BenchmarkingTime?.ToTimeSpan().TotalSeconds * waitingTimes[i])) / 60, 2)} минут");
            }
            JobState = JobStateEnum.Complited;
        }
    }
}