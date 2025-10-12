using sephp.Share.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Nginx.Models
{
    public class NginxPackage : Package
    {
        public override string Id { get; set; } = "nginx";

        public override async Task Reload()
        {
            var info = GetStartInfo("-s", "reload");
            PackageProcess p = new PackageProcess(info);
            await p.Start();
        }
    }
}
