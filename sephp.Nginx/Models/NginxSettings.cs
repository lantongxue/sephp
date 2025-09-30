using sephp.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Nginx.Models
{
    public class NginxSettings
    {
        public string? Version { get; set; }

        public int Pid {  get; set; }

        public PlatformBinrary PlatformBinrary { get; set; } = new PlatformBinrary()
        {
            Windows = "nginx.exe",
            Linux = "nginx",
            Darwin = "nginx"
        };
    }
}
