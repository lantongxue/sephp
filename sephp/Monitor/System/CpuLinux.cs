using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sephp.Monitor.System
{
    public class CpuLinux
    {
        private double _lastIdle = 0;
        private double _lastTotal = 0;

        public async Task<double> GetCpuUsageAsync()
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
    }
}
