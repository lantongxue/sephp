using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using sephp.Monitor.System.Api;

namespace sephp.Monitor.System
{
    public class MemoryDarwin
    {
        public Task<(double, double)> GetMemoryUsageAsync()
        {
            // 1. 获取总内存
            ulong len = sizeof(ulong);
            ulong totalMem;
            int[] mib = { Darwin.CTL_HW, Darwin.HW_MEMSIZE };
            if (Darwin.sysctl(mib, 2, out totalMem, ref len, IntPtr.Zero, 0) != 0)
            {
                throw new Exception("sysctl HW_MEMSIZE failed");
            }

            // 2. 获取 VM 统计
            int count = Darwin.HOST_VM_INFO_COUNT;
            int size = Marshal.SizeOf(typeof(Darwin.vm_statistics));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                if (Darwin.host_statistics(Darwin.mach_host_self(), Darwin.HOST_VM_INFO, ptr, ref count) != 0)
                {
                    throw new Exception("host_statistics failed");
                }
                var vmstat = (Darwin.vm_statistics)Marshal.PtrToStructure(ptr, typeof(Darwin.vm_statistics));
                
                ulong pageSize = (ulong)Environment.SystemPageSize;
                //ulong free      = (ulong)(vmstat.free_count + vmstat.speculative_count) * pageSize;
                ulong active    = (ulong)vmstat.active_count * pageSize;
                ulong inactive  = (ulong)vmstat.inactive_count * pageSize;
                ulong wired     = (ulong)vmstat.wire_count * pageSize;

                ulong used = active + inactive + wired;
                
                return Task.FromResult((Math.Round(used / 1024d / 1024, 2), Math.Round(totalMem / 1024d / 1024, 2)));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
