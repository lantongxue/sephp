using System;
using System.Diagnostics;

namespace sephp.Monitor.System
{
    public class CpuWindows
    {
        public CpuWindows()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        PerformanceCounter? cpuCounter = null;

        public double GetCpuUsageAsync()
        {
            return Math.Round(cpuCounter.NextValue(), 2);
        }

    }
}
