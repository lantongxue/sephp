using System;
using System.Runtime.InteropServices;

namespace sephp.Monitor.System
{
    public class MemoryWindows
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;             // 当前内存使用率（百分比）
            public ulong ullTotalPhys;            // 总物理内存（字节）
            public ulong ullAvailPhys;            // 可用物理内存（字节）
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

        public (double usedMb, double totalMb) GetMemoryUsage()
        {
            var memStatus = new MEMORYSTATUSEX();
            memStatus.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            if (!GlobalMemoryStatusEx(ref memStatus))
                throw new InvalidOperationException("无法获取内存状态");
            var totalMb = memStatus.ullTotalPhys / 1024f / 1024;
            var availableMb = memStatus.ullAvailPhys / 1024f / 1024;
            var usedMb = totalMb - availableMb;
            return (Math.Round(usedMb, 2), Math.Round(totalMb, 2));
        }
    }
}
