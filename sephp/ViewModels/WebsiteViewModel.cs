using ReactiveUI;
using sephp.Share.Locale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class WebsiteViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Website;

        public IScreen HostScreen { get; }

        public WebsiteViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
