using sephp.Share.Services.Config;
using sephp.Share.Services.Interfaces;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Share.Services
{
    public class ConfigService
    {
        public static void RegisterConfig<T>(string yaml) where T : new()
        {
            Locator.CurrentMutable.RegisterLazySingleton<IConfigService<T>>(
                () => new YamlConfigService<T>(yaml)
            );
        }
    }
}
