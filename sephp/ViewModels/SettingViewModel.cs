using Avalonia.Platform;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using sephp.Share.Locale;
using sephp.MessageBusRequests;
using sephp.Models;
using sephp.Share.Services.Interfaces;
using Splat;
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

        private readonly IConfigService<AppSettings> _config;

        public AppSettings Settings => _config.Settings;

        public SettingViewModel(IScreen screen)
        {
            HostScreen = screen;

            _config = Locator.Current.GetService<IConfigService<AppSettings>>()!;

            _config.OnChanged += _ => this.RaisePropertyChanged(nameof(Settings));

        }

        [ReactiveCommand]
        private void ToggleDebugMode()
        {
            MessageBus.Current.SendMessage(new DebugOverlayRequest(Settings.DebugOverlay));
            _config.Save();
        }
    }
}
