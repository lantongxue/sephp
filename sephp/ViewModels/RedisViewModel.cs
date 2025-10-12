using ReactiveUI;
using sephp.Share.Locale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class RedisViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Redis;

        public IScreen HostScreen { get; }

        public RedisViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
