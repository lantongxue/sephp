using Avalonia.Controls;
using Avalonia.Rendering;

namespace sephp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleSwitch_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var switchBtn = e.Source as ToggleSwitch;
            if(switchBtn!.IsChecked!.Value)
            {
                this.RendererDiagnostics.DebugOverlays =
                RendererDebugOverlays.Fps |
                RendererDebugOverlays.LayoutTimeGraph |
                RendererDebugOverlays.RenderTimeGraph;
            } else
            {
                this.RendererDiagnostics.DebugOverlays = RendererDebugOverlays.None;
            }
        }
    }
}