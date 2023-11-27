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
    public class TaskQueryService
    {
        private readonly SynchronizedCollection<Job> _taskQuery = new();
        private readonly object _locker = new ();

        public TaskQueryService()
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
                botTask = _taskQuery.FirstOrDefault(x=>!x.IsInProgress && !x.IsSended && !x.IsComplited);
                if(botTask == null)
                    return false;

                botTask.IsInProgress = true;
                return true;
            }
        }

        public void RemoveComplitedTask()
        {
            lock (_locker)
            {
                foreach (var task in _taskQuery.Where(x=>x.IsSended))
                    _taskQuery.Remove(task);
            }
        }
    }
}
