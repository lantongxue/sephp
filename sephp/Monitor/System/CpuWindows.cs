using System;
using System.Diagnostics;

namespace sephp.Monitor.System
{
    public class CpuWindows
    {
        public CpuWindows()
        {
#pragma warning disable CA1416 // 验证平台兼容性
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
#pragma warning restore CA1416 // 验证平台兼容性
        }

        PerformanceCounter? cpuCounter = null;

        public double GetCpuUsageAsync()
        {
#pragma warning disable CA1416 // 验证平台兼容性
            return Math.Round(cpuCounter!.NextValue(), 2);
#pragma warning restore CA1416 // 验证平台兼容性
        }

    }
}
