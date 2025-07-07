using ReactiveUI;
using sephp.I18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class MysqlViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.MySQL;

        public IScreen HostScreen { get; }

        public MysqlViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
