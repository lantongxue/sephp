using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using sephp.Monitor.System.Api;

namespace sephp.Monitor.System
{
    public class CpuDarwin
    {
        
        
        static Darwin.host_cpu_load_info GetCpuLoadInfo()
        {
            int count = Darwin.HOST_CPU_LOAD_INFO_COUNT;
            int size = Marshal.SizeOf(typeof(Darwin.host_cpu_load_info));
            IntPtr ptr = Marshal.AllocHGlobal(size);

            try
            {
                int result = Darwin.host_statistics(Darwin.mach_host_self(),
                    Darwin.HOST_CPU_LOAD_INFO,
                    ptr,
                    ref count);

                if (result != 0)
                {
                    throw new Exception("host_statistics failed: " + result);
                }
                return (Darwin.host_cpu_load_info)Marshal.PtrToStructure(ptr, typeof(Darwin.host_cpu_load_info));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
        
        public Task<double> GetCpuUsageAsync()
        {
            var info1 = GetCpuLoadInfo();
            Thread.Sleep(1000); // 采样间隔 1 秒
            var info2 = GetCpuLoadInfo();

            uint user = info2.cpu_ticks[0] - info1.cpu_ticks[0];
            uint system = info2.cpu_ticks[1] - info1.cpu_ticks[1];
            uint idle = info2.cpu_ticks[2] - info1.cpu_ticks[2];
            uint nice = info2.cpu_ticks[3] - info1.cpu_ticks[3];

            uint total = user + system + idle + nice;

            return Task.FromResult(Math.Round((user + system + nice) * 100.0 / total, 2));
        }
    }
}
