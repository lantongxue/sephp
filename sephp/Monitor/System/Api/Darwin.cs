using System;
using System.Runtime.InteropServices;

namespace sephp.Monitor.System.Api;

public class Darwin
{
    // Mach types
    public const int HOST_CPU_LOAD_INFO = 3;
    public const int HOST_CPU_LOAD_INFO_COUNT = (4);
    // host_statistics constants
    public const int HOST_VM_INFO = 2;
    public const int HOST_VM_INFO_COUNT = 38;
    // sysctl constants
    public const int CTL_HW = 6;
    public const int HW_MEMSIZE = 24;

    [StructLayout(LayoutKind.Sequential)]
    public struct host_cpu_load_info
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] cpu_ticks;
    }
        
    [DllImport("/usr/lib/libSystem.dylib")]
    public static extern int host_statistics(
        IntPtr host,
        int flavor,
        IntPtr info,
        ref int count);

    [DllImport("/usr/lib/libSystem.dylib")]
    public static extern IntPtr mach_host_self();
    
    [DllImport("/usr/lib/libSystem.dylib")]
    public static extern int sysctl(int[] name, uint namelen, out ulong oldp, ref ulong oldlenp, IntPtr newp, uint newlen);

    
    [StructLayout(LayoutKind.Sequential)]
    public struct vm_statistics
    {
        public uint free_count;
        public uint active_count;
        public uint inactive_count;
        public uint wire_count;
        public uint zero_fill_count;
        public uint reactivations;
        public uint pageins;
        public uint pageouts;
        public uint faults;
        public uint cow_faults;
        public uint lookups;
        public uint hits;
        public uint purgeable_count;
        public uint purges;
        public uint speculative_count;
        // 结构体还有更多字段，但这里只需要前几个
    }
}