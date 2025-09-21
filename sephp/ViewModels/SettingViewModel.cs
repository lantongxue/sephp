using Avalonia.Platform;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using sephp.I18n;
using sephp.MessageBusRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public partial class SettingViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Setting;

        public IScreen HostScreen { get; }

        public bool ShowDebugOverlay { get;set; } = false;

        public SettingViewModel(IScreen screen)
        {
            HostScreen = screen;
        }

        [ReactiveCommand]
        private void ToggleDebugMode()
        {
            MessageBus.Current.SendMessage(new DebugOverlayRequest(ShowDebugOverlay));

        }
    }
}
