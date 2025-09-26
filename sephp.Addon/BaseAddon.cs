
using sephp.Share.Services;

namespace sephp.Addon
{
    public abstract class BaseAddon
    {
        public abstract Information GetInformation();

        public void RegisterConfig<T>(string yaml) where T : new()
        {
            ConfigService.RegisterConfig<T>(yaml);
        }
    }
}
