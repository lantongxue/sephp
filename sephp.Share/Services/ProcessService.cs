using sephp.Share.Models;
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
        public PackageProcess Create(string executor, params string[] args)
        {
            PackageProcess process = new PackageProcess(executor, args);
            return process;
        }

        public PackageProcess FindProcessById(int id)
        {
            return PackageProcess.FindProcessById(id);
        }
    }
}
