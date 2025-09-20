using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sephp.Monitor.System
{
    public class MemoryDarwin
    {
        public async Task<(double usedMb, double totalMb)> GetMemoryUsageAsync()
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

            var usedMb = totalPages * pageSize / 1024 / 1024 / 1024;
            var totalMb = (totalPages + freePages) * pageSize / 1024 / 1024 / 1024;
            return (Math.Round(usedMb), Math.Round(totalMb));
        }
    }
}
