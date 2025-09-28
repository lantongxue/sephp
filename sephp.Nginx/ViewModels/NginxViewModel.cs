using ReactiveUI;
using ReactiveUI.SourceGenerators;
using sephp.Nginx.Locale;
using sephp.Nginx.Models;
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

        [Reactive]
        private bool _isRunning = false;

        private readonly IConfigService<NginxSettings> _config;
        public NginxSettings Settings => _config.Settings;

        private readonly ProcessService processService;

        public NginxViewModel(IScreen screen)
        {
            HostScreen = screen;

            _config = Locator.Current.GetService<IConfigService<NginxSettings>>()!;

            _config.OnChanged += _ => this.RaisePropertyChanged(nameof(Settings));

            processService = Locator.Current.GetService<ProcessService>()!;
        }

        public async Task Start()
        {
            await Task.Run(() =>
            {
                var p = processService.Start("cmd");
                _config.Settings.Pid = p.Id;
                _config.Save();
                IsRunning = true;
            });
            
        }
    }
}
