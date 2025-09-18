using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Semi.Avalonia;
using sephp.I18n;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();
        
        public string RepoUrl { get; } = "https://github.com/lantongxue/sephp";

        public ReactiveCommand<string, IRoutableViewModel> GoView { get; }

        [Reactive]
        public string CurrentPageTitle { get; set; } = Resource.Overview;
        public MainWindowViewModel()
        {
            Router.CurrentViewModel.Subscribe(x =>
            {
                if(x != null)
                {
                    CurrentPageTitle = x.UrlPathSegment ?? "Page Error";
                }
            });

            GoView = ReactiveCommand.CreateFromObservable<string, IRoutableViewModel>(GoViewExecute);

            Router.Navigate.Execute(new OverviewViewModel(this));
        }

        private IObservable<IRoutableViewModel> GoViewExecute(string param)
        {
            IRoutableViewModel? viewModel = null;
            switch(param)
            {
                
                case "Website":
                    viewModel = new WebsiteViewModel(this);
                    break;
                case "PHP":
                    viewModel = new PhpViewModel(this);
                    break;
                case "MySQL":
                    viewModel = new MysqlViewModel(this);
                    break;
                case "Redis":
                    viewModel = new RedisViewModel(this);
                    break;
                case "Nginx":
                    viewModel = new NginxViewModel(this);
                    break;
                case "Apache":
                    viewModel = new ApacheViewModel(this);
                    break;
                case "Setting":
                    viewModel = new SettingViewModel(this);
                    break;
                case "About":
                    viewModel = new AboutViewModel(this);
                    break;
                case "Overview":
                default:
                    viewModel = new OverviewViewModel(this);
                    break;
            }
            return Router.Navigate.Execute(viewModel);
        }

        public void ToggleTheme()
        {
            var app = Application.Current;
            if (app is null) return;
            var theme = app.ActualThemeVariant;
            app.RequestedThemeVariant = theme == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
            app.UnregisterFollowSystemTheme();
        }

        public async Task OpenRepoUrl()
        {
            var launcher = ResolveDefaultTopLevel()?.Launcher;
            if (launcher is not null)
            {
                await launcher.LaunchUriAsync(new Uri(RepoUrl));
            }
        }

        private static TopLevel? ResolveDefaultTopLevel()
        {
            return Application.Current?.ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktopLifetime => desktopLifetime.MainWindow,
                ISingleViewApplicationLifetime singleView => TopLevel.GetTopLevel(singleView.MainView),
                _ => null
            };
        }


    }
}
