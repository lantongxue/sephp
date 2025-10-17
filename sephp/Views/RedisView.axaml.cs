using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;
using sephp.ViewModels;

namespace sephp.Views;

public partial class RedisView : ReactiveUserControl<RedisViewModel>
{
    public RedisView()
    {
        InitializeComponent();
    }
}