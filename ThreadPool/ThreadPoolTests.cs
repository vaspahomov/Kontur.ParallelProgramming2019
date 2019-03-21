using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadPool
{
	public class DotNetThreadPoolWrapperTests : ThreadPoolTests<DotNetThreadPoolWrapper> { }
	public class DotNetThreadPoolTaskWrapperTests : ThreadPoolTests<DotNetThreadPoolTaskWrapper> { }

	public abstract class ThreadPoolTests<T> where T : IThreadPool, new()
	{
		private T threadPool;

		public void LongCalculations()
		{
			Console.Write("LongCalculations test: ");
			threadPool = new T();
			var timer = Stopwatch.StartNew();
			long enqueueMs;

			const int actionsCount = 1 * 1000;

			using(CountdownEvent cev = new CountdownEvent(actionsCount))
			{
				Action sumAction = () =>
				{
					cev.Signal();
					Thread.SpinWait(1000 * 1000);
				};
				for(int i = 0; i < actionsCount; i++)
				{
					threadPool.EnqueueAction(sumAction);
				}
				enqueueMs = timer.ElapsedMilliseconds;
				cev.Wait();
			}
			timer.Stop();
			Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]", timer.ElapsedMilliseconds, enqueueMs, (threadPool.GetWastedCycles() / Environment.ProcessorCount) / 10000, threadPool.GetTasksProcessedCount());
		}

		public void ShortCalculations()
		{
			Console.Write("ShortCalculations test: ");
			threadPool = new T();
			var timer = Stopwatch.StartNew();
			long enqueueMs;

			const int actionsCount = 1 * 1000 * 1000;

			using(CountdownEvent cev = new CountdownEvent(actionsCount))
			{
				Action sumAction = () =>
				{
					cev.Signal();
					Thread.SpinWait(1000);
				};
				for(int i = 0; i < actionsCount; i++)
				{ 
					threadPool.EnqueueAction(sumAction);
				}
				enqueueMs = timer.ElapsedMilliseconds;
				cev.Wait();
			}
			timer.Stop();
			Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]", timer.ElapsedMilliseconds, enqueueMs, (threadPool.GetWastedCycles() / Environment.ProcessorCount) / 10000, threadPool.GetTasksProcessedCount());
		}

		public void ExtremelyShortCalculations()
		{
			Console.Write("ExtremelyShortCalculations test: ");
			threadPool = new T();
			var timer = Stopwatch.StartNew();
			long enqueueMs;

			const int actionsCount = 1 * 1000 * 1000;

			using(CountdownEvent cev = new CountdownEvent(actionsCount))
			{
				Action sumAction = () =>
				{
					cev.Signal();
				};
				for(int i = 0; i < actionsCount; i++)
				{
					threadPool.EnqueueAction(sumAction);
				}
				enqueueMs = timer.ElapsedMilliseconds;
				cev.Wait();
			}
			timer.Stop();
			Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]", timer.ElapsedMilliseconds, enqueueMs, (threadPool.GetWastedCycles() / Environment.ProcessorCount) / 10000, threadPool.GetTasksProcessedCount());
		}

		public void InnerShortCalculations()
		{
			Console.Write("InnerCalculations test: ");
			threadPool = new T();
			var timer = Stopwatch.StartNew();
			long enqueueMs;

			const int actionsCount = 1 * 1000;
			const int subactionsCount = 1 * 1000;

			using(CountdownEvent outerEvent = new CountdownEvent(actionsCount))
			using(CountdownEvent innerEvent = new CountdownEvent(actionsCount * subactionsCount))
			{
				Action innerAction = () =>
				{
					innerEvent.Signal();
					Thread.SpinWait(1000);
				};
				Action outerAction = () =>
				{
					for(int i = 0; i < subactionsCount; i++)
					{
						threadPool.EnqueueAction(innerAction);
					}
					outerEvent.Signal();
				};

				for(int i = 0; i < actionsCount; i++)
				{
					threadPool.EnqueueAction(outerAction);
				}

				outerEvent.Wait();
				enqueueMs = timer.ElapsedMilliseconds;
				innerEvent.Wait();
			}
			timer.Stop();
			Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]", timer.ElapsedMilliseconds, enqueueMs, (threadPool.GetWastedCycles() / Environment.ProcessorCount) / 10000, threadPool.GetTasksProcessedCount());
		}

		public void InnerExtremelyShortCalculations()
		{
			Console.Write("InnerExtremelyShortCalculations test: ");
			threadPool = new T();
			var timer = Stopwatch.StartNew();
			long enqueueMs;

			const int actionsCount = 1 * 1000;
			const int subactionsCount = 1 * 1000;

			using(CountdownEvent outerEvent = new CountdownEvent(actionsCount))
			using(CountdownEvent innerEvent = new CountdownEvent(actionsCount * subactionsCount))
			{
				Action innerAction = () =>
				{
					innerEvent.Signal();
				};
				Action outerAction = () =>
				{
					for(int i = 0; i < subactionsCount; i++)
					{
						threadPool.EnqueueAction(innerAction);
					}
					outerEvent.Signal();
				};

				for(int i = 0; i < actionsCount; i++)
				{
					threadPool.EnqueueAction(outerAction);
				}

				outerEvent.Wait();
				enqueueMs = timer.ElapsedMilliseconds;
				innerEvent.Wait();
			}
			timer.Stop();
			Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]", timer.ElapsedMilliseconds, enqueueMs, (threadPool.GetWastedCycles() / Environment.ProcessorCount) / 10000, threadPool.GetTasksProcessedCount());
		}
	}
}