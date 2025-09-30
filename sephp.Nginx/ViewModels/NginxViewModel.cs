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

        [Reactive]
        private PackageProcess _nginxProcess;

        private readonly IConfigService<NginxSettings> _config;
        public NginxSettings Settings => _config.Settings;

        private readonly ProcessService processService;

        public NginxViewModel(IScreen screen)
        {
            HostScreen = screen;

            _config = Locator.Current.GetService<IConfigService<NginxSettings>>()!;

            _config.OnChanged += _ => this.RaisePropertyChanged(nameof(Settings));

            processService = Locator.Current.GetService<ProcessService>()!;

            NginxProcess = processService.FindProcessById(_config.Settings.Pid);

            this.WhenAnyValue(x => x.NginxProcess.IsRunning)
                .Subscribe(running =>
                {
                    var pid = running ? NginxProcess.GetPid() : 0;
                    _config.Settings.Pid = pid;
                    _config.Save();
                });

        }

        public async Task Start()
        {
            NginxProcess = processService.Create("cmd");
            await NginxProcess.Start();
            
        }

        [ReactiveCommand]
        private void Stop()
        {
            if(NginxProcess != null)
            {
                NginxProcess.KillProcess();
            }
        }

        [ReactiveCommand]
        private async Task Restart()
        {
            if (NginxProcess != null)
            {
                await NginxProcess.Restart();
            }
        }
    }
}
