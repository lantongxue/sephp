using ReactiveUI;
using ReactiveUI.SourceGenerators;
using sephp.Nginx.Locale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Nginx.ViewModels
{
    public partial class NginxViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Nginx;

        public IScreen HostScreen { get; }

        [Reactive]
        private bool _isRunning = false;

        public NginxViewModel(IScreen screen)
        {
            HostScreen = screen;
        }

        public async Task Start()
        {
            await Task.Delay(3000);
            IsRunning = true;
        }
    }
}
