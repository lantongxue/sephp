using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace sephp.Monitor.System
{
    public class CpuDarwin
    {
        public async Task<double> GetCpuUsageAsync()
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

                var match = Regex.Match(output, @"(\d+\.\d+)% user");
                if (match.Success)
                {
                    return double.Parse(match.Groups[1].Value);
                }
            }
            return -1;
        }
    }
}
