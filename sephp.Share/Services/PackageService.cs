using sephp.Share.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Share.Services
{
    public class PackageService
    {
        public string GetPackageDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packages");
        }

        public Package? FindPackage(string id, Version version)
        {
            string packageDir = Path.Combine(Package.Directory, id, version.ToString());
            if (!Directory.Exists(packageDir))
            {
                return null;
            }

            Package package = new Package()
            {
                Id = id,
                Version = version.ToString()
            };

            return package;
        }
    }
}
