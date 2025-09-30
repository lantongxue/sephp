using ReactiveUI;
using ReactiveUI.SourceGenerators;
using sephp.Nginx.Locale;
using sephp.Nginx.Models;
using sephp.Share.Models;
using sephp.Share.Services;
using sephp.Share.Services.Interfaces;
using Splat;
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

        private readonly IConfigService<NginxSettings> _config;
        public NginxSettings Settings => _config.Settings;

        [Reactive]
        private NginxPackage _package;

        public NginxViewModel(IScreen screen)
        {
            HostScreen = screen;

            _config = Locator.Current.GetService<IConfigService<NginxSettings>>()!;

            _config.OnChanged += _ => this.RaisePropertyChanged(nameof(Settings));

            Package = new NginxPackage()
            {
                Version = _config.Settings.Version,
                PlatformBinrary = _config.Settings.PlatformBinrary,
                Process = PackageProcess.FindProcessById(_config.Settings.Pid)
            };

            this.WhenAnyValue(x => x.Package.Process.IsRunning)
                .Subscribe(running =>
                {
                    var pid = running ? Package.Process.GetPid() : 0;
                    _config.Settings.Pid = pid;
                    _config.Save();
                });

        }

        public async Task Start()
        {
            await Package.Process.Start();
        }

        [ReactiveCommand]
        private void Stop()
        {
            
        }

        [ReactiveCommand]
        private async Task Restart()
        {
            
        }
    }
}
