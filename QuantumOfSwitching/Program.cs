using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace FindTimeQuant
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var processorNum = args.Length > 0 ? int.Parse(args[0]) - 1 : 0;
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr) (1 << processorNum);

            var results = new List<long>();
            var currentThread = Thread.CurrentThread;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var thread1 = new Thread(() =>
            {
                for (var i = 0; i < 1000000000; i++)
                {
                    if (currentThread == Thread.CurrentThread) continue;
                    results.Add(stopwatch.ElapsedMilliseconds);
                    //Console.WriteLine(stopwatch.ElapsedMilliseconds);
                    currentThread = Thread.CurrentThread;
                    stopwatch.Restart();
                }
            });

            var thread2 = new Thread(() =>
            {
                for (var i = 0; i < 1000000000; i++)
                {
                    if (currentThread == Thread.CurrentThread) continue;
                    results.Add(stopwatch.ElapsedMilliseconds);
                    //Console.WriteLine(stopwatch.ElapsedMilliseconds);
                    currentThread = Thread.CurrentThread;
                    stopwatch.Restart();
                }
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Elapsed quantum of switching: {results.Sum() / results.Count}");
        }
    }
}