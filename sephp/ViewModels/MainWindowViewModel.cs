using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Semi.Avalonia;
using sephp.Models;
using sephp.Nginx.ViewModels;
using sephp.Share.Services.Interfaces;
using Splat;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sephp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        [Reactive]
        private string? _repoUrl  = "https://github.com/lantongxue/sephp";

        [Reactive]
        private string? _currentPageTitle;

        private readonly IConfigService<AppSettings> _config;

        private Dictionary<string, ThemeVariant> _themes = new()
        {
            {"Auto", ThemeVariant.Default},
            {"Aquatic", SemiTheme.Aquatic},
            {"Desert", SemiTheme.Desert},
            {"Dusk", SemiTheme.Dusk},
            {"NightSky", SemiTheme.NightSky}
        };

        public List<MenuItemViewModel> MenuItems { get; }
        public MainWindowViewModel()
        {
            MenuItems = [];
            foreach (var theme in _themes)
            {
                MenuItems.Add(new MenuItemViewModel
                {
                    Header = theme.Key,
                    Command = SelectThemeCommand,
                    CommandParameter = theme.Key
                });
            }

            Router.CurrentViewModel.Subscribe(x =>
            {
                if (x != null)
                {
                    CurrentPageTitle = x.UrlPathSegment ?? "Page Error";
                }
            });

            _config = Locator.Current.GetService<IConfigService<AppSettings>>()!;

            SelectTheme(_config.Settings.Theme);

            Router.Navigate.Execute(new OverviewViewModel(this));
        }
        
        [ReactiveCommand]
        private IObservable<IRoutableViewModel> GoView(string param)
        {
            IRoutableViewModel? viewModel = null;
            switch (param)
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

        [ReactiveCommand]
        private void SelectTheme(string? key)
        {
            var app = Application.Current;
            if (app is null || key is null) return;

            if(_themes.TryGetValue(key, out var theme))
            {
                app.RequestedThemeVariant = theme;
                app.UnregisterFollowSystemTheme();

                _config.Settings.Theme = key;
                _config.Save();
            }
            
        }

        public async Task OpenRepoUrl()
        {
            var launcher = ResolveDefaultTopLevel()?.Launcher;
            if (launcher is not null)
            {
                await launcher.LaunchUriAsync(new Uri(RepoUrl!));
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

    public class MenuItemViewModel
    {
        public string? Header { get; set; }
        public ICommand? Command { get; set; }
        public object? CommandParameter { get; set; }
        public IList<MenuItemViewModel>? Items { get; set; }
    }
}