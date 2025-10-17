using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;
using Avalonia.Rendering;
using sephp.ViewModels;

namespace sephp.Views;

public partial class SettingView : ReactiveUserControl<SettingViewModel>
{
    public SettingView()
    {
        InitializeComponent();

    }
}