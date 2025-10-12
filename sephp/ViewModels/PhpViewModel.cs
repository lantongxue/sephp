using ReactiveUI;
using sephp.Share.Locale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class PhpViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.PHP;

        public IScreen HostScreen { get; }

        public PhpViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
