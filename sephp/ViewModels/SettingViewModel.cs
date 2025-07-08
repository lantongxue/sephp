using ReactiveUI;
using sephp.I18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class SettingViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Setting;

        public IScreen HostScreen { get; }

        public SettingViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
