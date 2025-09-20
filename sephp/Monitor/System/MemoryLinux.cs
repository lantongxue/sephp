using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sephp.Monitor.System
{
    public class MemoryLinux
    {
        public async Task<(double usedMb, double totalMb)> GetMemoryUsageAsync()
        {
            var lines = await File.ReadAllLinesAsync("/proc/meminfo");
            var totalLine = lines.FirstOrDefault(l => l.StartsWith("MemTotal"));
            var freeLine = lines.FirstOrDefault(l => l.StartsWith("MemAvailable"));
            var totalKb = double.Parse(totalLine.Split(':')[1].Trim().Split(' ')[0]);
            var freeKb = double.Parse(freeLine.Split(':')[1].Trim().Split(' ')[0]);
            var usedMb = (totalKb - freeKb) / 1024;
            var totalMb = totalKb / 1024;
            return (Math.Round(usedMb), Math.Round(totalMb));
        }
    }
}
