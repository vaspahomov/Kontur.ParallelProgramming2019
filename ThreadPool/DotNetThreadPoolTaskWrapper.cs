using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
	public class DotNetThreadPoolTaskWrapper : IThreadPool
	{
		public void EnqueueAction(Action action)
		{
			Task.Run(action);
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
