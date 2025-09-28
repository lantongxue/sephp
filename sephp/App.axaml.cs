using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using sephp.Models;
using sephp.Share.Services;
using sephp.Share.Services.Interfaces;
using sephp.ViewModels;
using sephp.Views;
using Splat;

namespace sephp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            RegisterMyServices();
            RegisterConfig();
            RegisterMyAddons();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        protected void RegisterMyServices()
        {
            Locator.CurrentMutable.RegisterLazySingleton<ProcessService>(
                () => new ProcessService()
            );
        }

        protected void RegisterConfig()
        {
            ConfigService.RegisterConfig<AppSettings>("Config/sephp.yaml");
        }

        protected void RegisterMyAddons()
        {
            sephp.Nginx.MyAddon.Bootstrap();
        }
    }
}