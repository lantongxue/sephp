using sephp.Addon;
using sephp.Nginx.Models;

namespace sephp.Nginx
{
    public class MyAddon : BaseAddon
    {
        private static MyAddon? _instance;
        public override Information GetInformation()
        {
            return new Information()
            {
                Name = "Nginx",
                Author = "SEPHP",
                Description = "This is a nginx plugin of sephp",
                Website = "https://github.com/lantongxue/sephp",
                Version = new Version(1, 0, 0)
            };
        }

        public static MyAddon Bootstrap()
        {
            if(_instance == null)
            {
                _instance = new MyAddon();
            }
            _instance.RegisterConfig<NginxSettings>("Config/nginx.yaml");
            return _instance;
        }
    }
}
