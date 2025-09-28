using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Share.Services
{
    public class ProcessService
    {
        public Process Start(string executor, params string[] args)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(executor, args);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            return process;
        }
    }
}
