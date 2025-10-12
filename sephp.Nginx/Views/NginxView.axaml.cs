using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using DynamicData.Binding;
using ReactiveUI;
using sephp.Nginx.ViewModels;
using System.Reactive;
using System.Reactive.Linq;

namespace sephp.Nginx.Views;

public partial class NginxView : ReactiveUserControl<NginxViewModel>
{
    public NginxView()
    {
        InitializeComponent();
    }

    private async void NginxButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button? btn = sender as Button;
        if(btn == null)
        {
            return;
        }
        //btn.IsEnabled = false;
        btn.Classes.Add("Loading");

        if (btn.Command is ReactiveCommand<Unit, Unit> cmd)
        {
            await cmd.Execute();
        }

        btn.Classes.Remove("Loading");
    }
}