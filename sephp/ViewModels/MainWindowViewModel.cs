using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using Semi.Avalonia;
using System;
using System.Threading.Tasks;

namespace sephp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        public string RepoUrl { get; } = "https://github.com/lantongxue/sephp";
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
