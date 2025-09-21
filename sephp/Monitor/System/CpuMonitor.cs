using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace sephp.Monitor.System
{
    public class CpuMonitor
    {

        private CpuWindows? windows = null;

        private CpuLinux? linux = null;

        private CpuDarwin? darwin = null;

        public async Task<double> GetCpuUsageAsync()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (windows == null)
                {
                    windows = new CpuWindows();
                }
                return windows.GetCpuUsageAsync();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (linux == null)
                {
                    linux = new CpuLinux();
                }
                return await linux.GetCpuUsageAsync();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (darwin == null)
                {
                    darwin = new CpuDarwin();
                }
                return await darwin.GetCpuUsageAsync();
            }

            return -1;
        }
    }
}
