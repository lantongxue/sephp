using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using sephp.I18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public partial class NginxViewModel : ViewModelBase, IRoutableViewModel
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
