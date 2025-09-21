using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using sephp.Models;
using sephp.Services.Config;
using sephp.Services.Interfaces;
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
            Locator.CurrentMutable.RegisterLazySingleton<IConfigService<AppSettings>>(
                () => new YamlConfigService<AppSettings>("Config/app.yaml")
            );
        }

    }
}