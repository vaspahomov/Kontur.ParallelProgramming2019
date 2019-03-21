using System;

namespace ThreadPool
{
    public class DotNetThreadPoolWrapper : IThreadPool
    {
        public void EnqueueAction(Action action)
        {
			System.Threading.ThreadPool.UnsafeQueueUserWorkItem(delegate { action.Invoke(); }, null);
        }

	    public long GetTasksProcessedCount()
	    {
		    return -1;
	    }

	    public long GetWastedCycles()
	    {
		    return -1;
	    }
    }
}