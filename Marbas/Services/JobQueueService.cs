using Marbas.Enums;
using Marbas.Interfaces;
using Marbas.Job;

namespace Marbas.Services
{
    /// <summary>
    /// Single
    /// </summary>
    public class JobQueueService
    {
        private SynchronizedCollection<BaseJob> _jobQuery = new();
        private readonly object _locker = new();

        public JobQueueService()
        {
        }

        public void AddNewJob(BaseJob job)
        {
            lock (_locker)
            {
                _jobQuery.Add(job);
            }
        }

        public bool TryGetUncomplitedJob(out BaseJob? botJob)
        {
            lock (_locker)
            {
                botJob = _jobQuery.FirstOrDefault(x => x.JobState == JobStateEnum.InQueue);
                if (botJob == null)
                    return false;

                botJob.JobState = JobStateEnum.InProgress;
                return true;
            }
        }

        public bool TryGetUncomplitedJob(out BaseJob? botTask, Type type)
        {
            lock (_locker)
            {
                botTask = _jobQuery.FirstOrDefault(x => x.JobState == JobStateEnum.InQueue && (x.GetType() == type || x.GetType().GetInterfaces().Contains(type)));
                if (botTask == null)
                    return false;

                botTask.JobState = JobStateEnum.InProgress;
                return true;
            }
        }

        public bool TryGetUncomplitedJob(out BaseJob? botJob, IEnumerable<Type> types)
        {
            lock (_locker)
            {
                botJob = _jobQuery.FirstOrDefault(x => x.JobState == JobStateEnum.InQueue && types.Contains(x.GetType()));
                if (botJob == null)
                    return false;

                botJob.JobState = JobStateEnum.InProgress;
                return true;
            }
        }

        public IEnumerable<BaseJob> GetJobs<T>() where T : IJob
        {
            lock (_locker)
            {
                return _jobQuery.Where(x => x is T)
                                .ToList();
            }
        }

        public void RemoveComplitedJob()
        {
            lock (_locker)
            {
                foreach (var job in _jobQuery.Where(x => x.JobState == JobStateEnum.Complited).ToList())
                    _jobQuery.Remove(job);
            }
        }
    }
}