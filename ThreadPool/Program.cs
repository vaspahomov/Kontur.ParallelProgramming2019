using System;

namespace ThreadPool
{
	static class Program
	{
		static volatile bool complete;
		static void Main(string[] args)
		{
//			int concurrencyLevel = 2;
//			Process.GetCurrentProcess().ProcessorAffinity = new IntPtr((1 << concurrencyLevel) - 1);

			Console.WriteLine("\n----------=======DotNet ThreadPool Tasks tests=======----------");
			var testsDNT = new DotNetThreadPoolTaskWrapperTests();
			testsDNT.LongCalculations();
			testsDNT.ShortCalculations();
			testsDNT.ExtremelyShortCalculations();
			testsDNT.InnerShortCalculations();
			testsDNT.InnerExtremelyShortCalculations();

			Console.WriteLine("\n----------=======DotNet ThreadPool tests=======----------");
			var testsDN = new DotNetThreadPoolWrapperTests();
			testsDN.LongCalculations();
			testsDN.ShortCalculations();
			testsDN.ExtremelyShortCalculations();
			testsDN.InnerShortCalculations();
			testsDN.InnerExtremelyShortCalculations();
		}
	}
}
 