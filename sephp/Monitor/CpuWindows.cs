using System;
using System.Runtime.InteropServices;

namespace sephp.Monitor
{
    public class CpuWindows
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetSystemTimes(out FILETIME idleTime, out FILETIME kernelTime, out FILETIME userTime);

        private ulong _prevIdle;
        private ulong _prevKernel;
        private ulong _prevUser;

        public CpuWindows()
        {
            GetCpuTimes(out _prevIdle, out _prevKernel, out _prevUser);
        }

        public double GetCpuUsageAsync()
        {
            GetCpuTimes(out var idle, out var kernel, out var user);

            var sysKernel = kernel - _prevKernel;
            var sysUser = user - _prevUser;
            var sysIdle = idle - _prevIdle;

            var total = sysKernel + sysUser;
            var usage = total == 0 ? 0 : (1.0 - ((double)sysIdle / total)) * 100;

            _prevIdle = idle;
            _prevKernel = kernel;
            _prevUser = user;

            return Math.Round(usage, 2);
        }

        private static void GetCpuTimes(out ulong idle, out ulong kernel, out ulong user)
        {
            if (!GetSystemTimes(out var idleTime, out var kernelTime, out var userTime))
                throw new InvalidOperationException("GetSystemTimes failed.");

            idle = ((ulong)idleTime.dwHighDateTime << 32) | idleTime.dwLowDateTime;
            kernel = ((ulong)kernelTime.dwHighDateTime << 32) | kernelTime.dwLowDateTime;
            user = ((ulong)userTime.dwHighDateTime << 32) | userTime.dwLowDateTime;
        }

    }
}
