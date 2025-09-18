using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace sephp.Monitor
{
    public class Memory
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


        public async Task<(double usedMb, double totalMb)> GetMemoryUsageAsync()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var memStatus = new MEMORYSTATUSEX();
                memStatus.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));

                if (!GlobalMemoryStatusEx(ref memStatus))
                    throw new InvalidOperationException("无法获取内存状态");

                var totalMb = memStatus.ullTotalPhys / 1024.0 / 1024;
                var availableMb = memStatus.ullAvailPhys / 1024.0 / 1024;
                var usedMb = totalMb - availableMb;
                var usagePercent = memStatus.dwMemoryLoad;

                return (Math.Round(usedMb, 2), Math.Round(totalMb, 2));

            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var lines = await File.ReadAllLinesAsync("/proc/meminfo");
                var totalLine = lines.FirstOrDefault(l => l.StartsWith("MemTotal"));
                var freeLine = lines.FirstOrDefault(l => l.StartsWith("MemAvailable"));

                var totalKb = double.Parse(totalLine.Split(':')[1].Trim().Split(' ')[0]);
                var freeKb = double.Parse(freeLine.Split(':')[1].Trim().Split(' ')[0]);

                var usedMb = (totalKb - freeKb) / 1024;
                var totalMb = totalKb / 1024;
                return (Math.Round(usedMb, 2), Math.Round(totalMb, 2));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "vm_stat",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                var output = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit();

                var pageSize = 4096.0;
                var totalPages = 0.0;
                var freePages = 0.0;

                foreach (var line in output.Split('\n'))
                {
                    if (line.Contains("Pages free")) freePages = double.Parse(Regex.Match(line, @"\d+").Value);
                    if (line.Contains("Pages active") || line.Contains("Pages inactive") || line.Contains("Pages wired down"))
                        totalPages += double.Parse(Regex.Match(line, @"\d+").Value);
                }

                var usedMb = totalPages * pageSize / 1024 / 1024;
                var totalMb = (totalPages + freePages) * pageSize / 1024 / 1024;
                return (Math.Round(usedMb, 2), Math.Round(totalMb, 2));
            }

            return (-1, -1);
        }
    }

}
