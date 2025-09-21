using Avalonia;
using Avalonia.Controls;
using Avalonia.Rendering;
using ReactiveUI;
using sephp.MessageBusRequests;
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
                if(msg.Show)
                {
                    this.RendererDiagnostics.DebugOverlays =
                    RendererDebugOverlays.Fps |
                    RendererDebugOverlays.LayoutTimeGraph |
                    RendererDebugOverlays.RenderTimeGraph;
                } else
                {
                    this.RendererDiagnostics.DebugOverlays = RendererDebugOverlays.None;
                }
            });

        }
    }
}