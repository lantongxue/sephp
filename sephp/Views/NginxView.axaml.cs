using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DynamicData.Binding;
using ReactiveUI;
using sephp.ViewModels;
using System.Reactive.Linq;

namespace sephp.Views;

public partial class NginxView : ReactiveUserControl<NginxViewModel>
{
    public NginxView()
    {
        InitializeComponent();
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button? btn = sender as Button;
        if(btn == null)
        {
            return;
        }
        btn.IsEnabled = false;
        btn.Classes.Add("Loading");

        await this.ViewModel!.Start();

        btn.Classes.Remove("Loading");
    }
}