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

        public T? FindPackage<T>(string id, Version version) where T : new()
        {
            string packageDir = Path.Combine(Package.Directory, id, version.ToString());
            if (!Directory.Exists(packageDir))
            {
                //return default(T);
            }

            var package = new T();

            return package;
        }
    }
}
