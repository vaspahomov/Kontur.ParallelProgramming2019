using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            
            var data = Enumerable.Range(0, 10000000);
            
            sw.Start();

            var a = data.AsParallel().Select(x => Math.Sin(x)).Sum();
            Console.WriteLine($"res {a}");
            Console.WriteLine(sw.ElapsedMilliseconds);
            
            data = Enumerable.Range(0, 10000000);
            
            sw.Start();
            
            a = data.Select(x => Math.Sin(x)).Sum();
            Console.WriteLine($"res {a}");

            Console.WriteLine(sw.ElapsedMilliseconds);

            data = Enumerable.Range(0, 10000000);
            
            sw.Restart();
            var enumerator = data.GetEnumerator();

            var collection1 = new List<int>();
            var collection2 = new List<int>();
            var collection3 = new List<int>();
            var collection4 = new List<int>();

            var i = 0;
            var j = 1;
            var f = true;
            while (f)
            {
                for (var x = 0; x < j; x++)
                {
                    if (!enumerator.MoveNext())
                    {
                        f = false;
                        break;
                    }
                    
                    collection1.Add(enumerator.Current);
                }
                if (!f) break;
                for (var x = 0; x < j; x++)
                {
                    if (!enumerator.MoveNext())
                    {
                        f = false;
                        break;
                    }
                    
                    collection2.Add(enumerator.Current);
                }
                if (!f) break;
                for (var x = 0; x < j; x++)
                {
                    if (!enumerator.MoveNext())
                    {
                        f = false;
                        break;
                    }
                    
                    collection3.Add(enumerator.Current);
                }
                if (!f) break;
                for (var x = 0; x < j; x++)
                {
                    if (!enumerator.MoveNext())
                    {
                        f = false;
                        break;
                    }
                    
                    collection4.Add(enumerator.Current);
                }
                if (!f) break;
                j*=2;
            }
            
            var collections = new IEnumerable<int>[]{collection1, collection2, collection3, collection4};

            var result = 0.0;
            
            Parallel.ForEach(collections, (y) =>
            {
                var res = y.Select(h => Math.Sin(h)).Sum();
                result += res;
            });
            
            Console.WriteLine($"res {result}");
            
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
