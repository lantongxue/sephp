using ReactiveUI;
using sephp.Share.Locale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class AboutViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.About;

        public IScreen HostScreen { get; }

        public AboutViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
