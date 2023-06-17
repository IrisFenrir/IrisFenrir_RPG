using System;
using System.Diagnostics;

namespace IrisFenrir
{
    public class TimeHelper
    {
        public static long CalculateTime(Action action, int times)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < times; i++)
            {
                action();
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
