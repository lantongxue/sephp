using Avalonia;
using Avalonia.Controls;
using Avalonia.Rendering;
using ReactiveUI;
using sephp.MessageBusRequests;
using sephp.Models;
using sephp.Services.Interfaces;
using Splat;
using System;


namespace sephp.Views
{
    public partial class MainWindow : Window
    {
        IConfigService<AppSettings> config = Locator.Current.GetService<IConfigService<AppSettings>>()!;
        public MainWindow()
        {
            InitializeComponent();

            this.GetObservable(WindowStateProperty).Subscribe(state =>
            {
                if (state == WindowState.Normal)
                {
                    Width = config.Settings.Width;
                    Height = config.Settings.Height;
                }
            });

            MessageBus.Current.Listen<DebugOverlayRequest>().Subscribe(msg =>
            {
                ShowDebugOverlays(msg.Show);
            });

            
            ShowDebugOverlays(config.Settings.DebugOverlay);
        }

        protected void ShowDebugOverlays(bool show)
        {
            if (show)
            {
                this.RendererDiagnostics.DebugOverlays =
                RendererDebugOverlays.Fps |
                RendererDebugOverlays.LayoutTimeGraph |
                RendererDebugOverlays.RenderTimeGraph;
            }
            else
            {
                this.RendererDiagnostics.DebugOverlays = RendererDebugOverlays.None;
            }
        }

        private void Window_Resized(object? sender, Avalonia.Controls.WindowResizedEventArgs e)
        {
            config.Settings.Width = Width;
            config.Settings.Height = Height;
            config.Save();
        }
    }
}