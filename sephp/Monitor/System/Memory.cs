using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace sephp.Monitor.System
{
    public class Memory
    {

        private MemoryWindows? windows = null;

        private MemoryLinux? linux = null;

        private MemoryDarwin? darwin = null;

        public async Task<(double usedMb, double totalMb)> GetMemoryUsageAsync()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if(windows == null)
                {
                    windows = new MemoryWindows();
                }
                return windows.GetMemoryUsage();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (linux == null)
                {
                    linux = new MemoryLinux();
                }
                return await linux.GetMemoryUsageAsync();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (darwin == null)
                {
                    darwin = new MemoryDarwin();
                }
                return await darwin.GetMemoryUsageAsync();
            }

            return (-1, -1);
        }
    }

}
