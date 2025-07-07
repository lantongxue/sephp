using ReactiveUI;
using sephp.I18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class NginxViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Nginx;

        public IScreen HostScreen { get; }

        public NginxViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
