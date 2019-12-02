using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace OdmWordCount
{
    class Program
    {
        private static string displayElapsedTime(TimeSpan ts)
            => String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            var n = 10;
            var singleTimespanList = new List<long>();
            var multiTimespanList = new List<long>();

            for (var i = 0 ; i < n; i++) {
                stopWatch.Reset();
                stopWatch.Start();
                WordCountService.GenerateWordCount();
                stopWatch.Stop();
                singleTimespanList.Add(stopWatch.Elapsed.Ticks);
            }

            for (var i = 0 ; i < n; i++) {
                stopWatch.Reset();
                stopWatch.Start();
                MultiTWordCountService.GenerateWordCount();
                stopWatch.Stop();
                multiTimespanList.Add(stopWatch.Elapsed.Ticks);
            }

            var averageElapsedSingle = new TimeSpan(Convert.ToInt64(singleTimespanList.Average()));
            Console.WriteLine("Average RunTime Single: " + displayElapsedTime(averageElapsedSingle));
            var averageElapsedMulti = new TimeSpan(Convert.ToInt64(multiTimespanList.Average()));
            Console.WriteLine("Average RunTime Multi:  " + displayElapsedTime(averageElapsedMulti));
        }
    }
}
