using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Monitor
{
    public class Cpu
    {
        private double _lastIdle = 0;
        private double _lastTotal = 0;

        private CpuWindows? monitor = null;

        public async Task<double> GetCpuUsageAsync()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if(monitor == null)
                {
                    monitor = new CpuWindows();
                }
                return monitor.GetCpuUsageAsync();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return await GetCpuUsageLinuxAsync();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return await GetCpuUsageMacOSAsync();
            }

            return -1;
        }

        private double GetCpuUsageWindows()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            return Math.Round(cpuCounter.NextValue(), 2);
        }
        private async Task<double> GetCpuUsageLinuxAsync()
        {
            var lines = await File.ReadAllLinesAsync("/proc/stat");
            var cpuLine = lines.FirstOrDefault(l => l.StartsWith("cpu "));
            var parts = cpuLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var user = double.Parse(parts[1]);
            var nice = double.Parse(parts[2]);
            var system = double.Parse(parts[3]);
            var idle = double.Parse(parts[4]);

            var total = user + nice + system + idle;
            var used = user + nice + system;

            var deltaTotal = total - _lastTotal;
            var deltaIdle = idle - _lastIdle;

            _lastTotal = total;
            _lastIdle = idle;

            if (deltaTotal == 0) return 0;
            return Math.Round((1.0 - deltaIdle / deltaTotal) * 100, 2);
        }
        private async Task<double> GetCpuUsageMacOSAsync()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "top",
                Arguments = "-l 1 | grep 'CPU usage'",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                var output = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit();

                var match = System.Text.RegularExpressions.Regex.Match(output, @"(\d+\.\d+)% user");
                if (match.Success)
                {
                    return double.Parse(match.Groups[1].Value);
                }
            }
            return -1;
        }

    }
}
