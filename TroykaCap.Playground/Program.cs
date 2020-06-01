using System;
using System.Diagnostics;

namespace TroykaCap.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Init");
            
            var sw = Stopwatch.StartNew();
            var expander = new GpioExpander();
            sw.Stop();
            Console.WriteLine($"Initialized GPIO Expander in {sw.ElapsedMilliseconds}ms");

            const int readCount = 10000;
            var result = 0;
            Console.WriteLine($"Performing {readCount} reads");
            sw.Restart();
            for (int i = 0; i < readCount; ++i)
            {
                result = expander.DigitalReadPort();
            }
            sw.Stop();
            Console.WriteLine($"Performed {readCount} port reads in {sw.ElapsedMilliseconds}ms ({(float)sw.ElapsedMilliseconds/(float)readCount} per read)");
            Console.WriteLine($"Last read: {result}");
            
            Console.WriteLine("Resetting expander");
            expander.Reset();

            Console.WriteLine("Press ENTER to quit");
            Console.ReadLine();
        }
    }
}