using ReactiveUI;
using sephp.I18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class OverviewViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Overview;

        public IScreen HostScreen { get; }

        public OverviewViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
