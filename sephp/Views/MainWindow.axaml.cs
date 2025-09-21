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
        
        public MainWindow()
        {
            InitializeComponent();

            this.GetObservable(WindowStateProperty).Subscribe(state =>
            {
                if (state == WindowState.Normal)
                {
                    Width = 1200;
                    Height = 600;
                }
            });

            MessageBus.Current.Listen<DebugOverlayRequest>().Subscribe(msg =>
            {
                ShowDebugOverlays(msg.Show);
            });

            IConfigService<AppSettings> config = Locator.Current.GetService<IConfigService<AppSettings>>()!;
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
    }
}