using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Avalonia;
using sephp.Nginx.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using TextMateSharp.Grammars;
using sephp.Nginx.Locale;

namespace sephp.Nginx.Views;

public partial class NginxView : ReactiveUserControl<NginxViewModel>
{
    private WindowNotificationManager? _manager;
    public NginxView()
    {
        InitializeComponent();

        EditorInit();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var topLevel = TopLevel.GetTopLevel(this);
        _manager = new WindowNotificationManager(topLevel) { MaxItems = 3 };
    }

    private async void NginxButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button? btn = sender as Button;
        if (btn == null)
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

    protected void EditorInit()
    {
        var ConfigEditor = this.FindControl<TextEditor>("ConfigEditor");

        var registryOptions = new RegistryOptions(ThemeName.DarkPlus);
        var textMateInstallation = ConfigEditor.InstallTextMate(registryOptions);

        textMateInstallation.SetGrammarFile("Highlight/nginx.tmLanguage.json");
    }

    private async void SaveConfButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button? btn = sender as Button;
        if (btn == null)
        {
            return;
        }

        if (btn.Command is ReactiveCommand<Unit, Unit> cmd)
        {
            await cmd.Execute();
        }
        var notify = new Avalonia.Controls.Notifications.Notification(Resource.Tips, Resource.SaveConfigSuccess, NotificationType.Success);
        _manager?.Show(notify);
    }

    private async void ReloadConfButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button? btn = sender as Button;
        if (btn == null)
        {
            return;
        }

        if (btn.Command is ReactiveCommand<Unit, Unit> cmd)
        {
            await cmd.Execute();
        }
        var notify = new Avalonia.Controls.Notifications.Notification(Resource.Tips, Resource.ReloadConfigSuccess, NotificationType.Success);
        _manager?.Show(notify);
    }
}