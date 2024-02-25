using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XResize.Bot.Models.Work;

using XResize.Bot.Job;
using XResize.Bot.Interface;
using XResize.Bot.Enums;

namespace XResize.Bot.Services
{
    /// <summary>
    /// Single
    /// </summary>
    public class TaskQueueService
    {
        private SynchronizedCollection<BaseJob> _taskQuery = new();
        private readonly object _locker = new();

        public TaskQueueService()
        {

        }

        public void AddNewTask(BaseJob task)
        {
            lock (_locker)
            {
                _taskQuery.Add(task);
            }
        }

        public bool TryGetUncomplitedTask(out BaseJob? botTask)
        {
            lock (_locker)
            {
                botTask = _taskQuery.FirstOrDefault(x => x.JobState == JobStateEnum.InQueue);
                if (botTask == null)
                    return false;

                botTask.JobState = JobStateEnum.InProgress;
                return true;
            }
        }

        public bool TryGetUncomplitedTask(out BaseJob? botTask, Type type)
        {
            lock (_locker)
            {
                botTask = _taskQuery.FirstOrDefault(x => x.JobState == JobStateEnum.InQueue && (x.GetType() == type || x.GetType().GetInterfaces().Contains(type)));
                if (botTask == null)
                    return false;

                botTask.JobState = JobStateEnum.InProgress;
                return true;
            }
        }

        public bool TryGetUncomplitedTask(out BaseJob? botTask, IEnumerable<Type> types)
        {
            lock (_locker)
            {
                botTask = _taskQuery.FirstOrDefault(x => x.JobState == JobStateEnum.InQueue && types.Contains(x.GetType()));
                if (botTask == null)
                    return false;

                botTask.JobState = JobStateEnum.InProgress;
                return true;
            }
        }

        public IEnumerable<ISlowJob> GetSlowTasks(BotTypeEnum botType)
        {
            lock (_locker)
            {
                return _taskQuery.Where(x => x is ISlowJob)
                                 .Select(x => (ISlowJob)x)
                                 .Where(x => x.Type == botType)
                                 .ToList();
            }
        }

        public void RemoveComplitedTask()
        {
            lock (_locker)
            {
                foreach (var task in _taskQuery.Where(x => x.JobState == JobStateEnum.Complited).ToList())
                    _taskQuery.Remove(task);

            }
        }
    }
}
