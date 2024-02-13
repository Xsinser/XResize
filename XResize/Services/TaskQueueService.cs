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

namespace XResize.Bot.Services
{
    /// <summary>
    /// Single
    /// </summary>
    public class TaskQueueService
    {
        private readonly SynchronizedCollection<Job> _taskQuery = new();
        private readonly object _locker = new();

        public TaskQueueService()
        {

        }

        public void AddNewTask(Job task)
        {
            lock (_locker)
            {
                _taskQuery.Add(task);
            }
        }

        public bool TryGetUncomplitedTask(out Job botTask)
        {
            lock (_locker)
            {
                botTask = _taskQuery.FirstOrDefault(x => x.JobState == Enums.JobStateEnum.InQueue);
                if (botTask == null)
                    return false;

                botTask.JobState = Enums.JobStateEnum.InProgress;
                return true;
            }
        }

        public void RemoveComplitedTask()
        {
            lock (_locker)
            {
                foreach (var task in _taskQuery.Where(x => x.JobState == Enums.JobStateEnum.Complited))
                    _taskQuery.Remove(task);
            }
        }
    }
}
