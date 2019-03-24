using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class ThreadPoolWithQueue:IThreadPool
    {
        private int countOfExecuted = 0;
        
        private readonly Queue<Action> tasks = new Queue<Action>();
        private readonly List<Thread> executingThreads = new List<Thread>();
        private const int LimitCountOfExecutingTasks = 4;
        
        public ThreadPoolWithQueue()
        {
            for(var i =0; i< LimitCountOfExecutingTasks; i++)
                executingThreads.Add(new Thread(() =>
                {
                    while (true)
                    {
                        Action task;
                        lock (tasks)
                        {
                            while (tasks.Count == 0)
                            {
                                threadWaiting++;
                                try { Monitor.Wait(tasks); }
                                finally { threadWaiting--; }
                            }
                            task = tasks.Dequeue();
                        }
                        
                        task.Invoke(); 
                        countOfExecuted++;
                    }
                }));
            
            foreach (var executingThread in executingThreads)
            {
                executingThread.Start();
            }
        }
        
        public void EnqueueAction(Action action)
        {
            lock (tasks)
            {
                tasks.Enqueue(action);
                if(threadWaiting > 0)
                    Monitor.Pulse(tasks);
            }
        }

        private int threadWaiting = 0; 
        public long GetTasksProcessedCount()
        {
            return countOfExecuted;
        }

        public long GetWastedCycles()
        {
            return -1;
        }
    }
}