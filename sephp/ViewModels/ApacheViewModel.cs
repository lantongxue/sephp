using ReactiveUI;
using sephp.Share.Locale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class ApacheViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Apache;

        public IScreen HostScreen { get; }

        public ApacheViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
