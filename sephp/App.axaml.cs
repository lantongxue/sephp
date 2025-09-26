using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using sephp.Models;
using sephp.Share.Services;
using sephp.ViewModels;
using sephp.Views;

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
            ConfigService.RegisterConfig<AppSettings>("Config/sephp.yaml");
        }

    }
}