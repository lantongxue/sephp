using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Semi.Avalonia;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using sephp.Nginx.ViewModels;

namespace sephp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        public string RepoUrl { get; } = "https://github.com/lantongxue/sephp";

        public ReactiveCommand<string, IRoutableViewModel> GoView { get; }

        [Reactive]
        private string? _currentPageTitle;

        [Reactive]
        private ThemeVariant? _selectedThemeVariant;

        public IReadOnlyList<MenuItemViewModel> MenuItems { get; }
        public MainWindowViewModel()
        {
            MenuItems =
            [
                new MenuItemViewModel
                {
                    Header = "Theme",
                    Items =
                    [
                        new MenuItemViewModel
                        {
                            Header = "Auto",
                            Command = FollowSystemThemeCommand
                        },
                        new MenuItemViewModel
                        {
                            Header = "Aquatic",
                            Command = SelectThemeCommand,
                            CommandParameter = SemiTheme.Aquatic
                        },
                        new MenuItemViewModel
                        {
                            Header = "Desert",
                            Command = SelectThemeCommand,
                            CommandParameter = SemiTheme.Desert
                        },
                        new MenuItemViewModel
                        {
                            Header = "Dusk",
                            Command = SelectThemeCommand,
                            CommandParameter = SemiTheme.Dusk
                        },
                        new MenuItemViewModel
                        {
                            Header = "NightSky",
                            Command = SelectThemeCommand,
                            CommandParameter = SemiTheme.NightSky
                        }
                    ]
                }
            ];

            Router.CurrentViewModel.Subscribe(x =>
            {
                if (x != null)
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
        private void FollowSystemTheme()
        {
            Application.Current?.RegisterFollowSystemTheme();
        }

        [ReactiveCommand]
        private void SelectTheme(object? obj)
        {
            var app = Application.Current;
            if (app is null) return;
            app.RequestedThemeVariant = obj as ThemeVariant;
            app.UnregisterFollowSystemTheme();

            var a = Thread.CurrentThread.CurrentCulture;
            
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

    public class MenuItemViewModel
    {
        public string? Header { get; set; }
        public ICommand? Command { get; set; }
        public object? CommandParameter { get; set; }
        public IList<MenuItemViewModel>? Items { get; set; }
    }
}