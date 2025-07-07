using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using sephp.ViewModels;

namespace sephp.Views;

public partial class WebsiteView : ReactiveUserControl<WebsiteViewModel>
{
    public WebsiteView()
    {
        InitializeComponent();
    }
}