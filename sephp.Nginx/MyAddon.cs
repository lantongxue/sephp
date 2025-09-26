using sephp.Addon;
using sephp.Nginx.Models;

namespace sephp.Nginx
{
    public class MyAddon : BaseAddon
    {
        public override Information GetInformation()
        {
            return new Information()
            {
                Name = "Nginx",
                Author = "SEPHP",
                Description = "This is a nginx plugin of sephp",
                Uri = new Uri("https://github.com/lantongxue/sephp"),
                Version = new Version(1, 0, 0)
            };
        }

        public void Bootstrap()
        {
            this.RegisterConfig<NginxConfig>("Config/nginx.yaml");
        }
    }
}
