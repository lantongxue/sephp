using Avalonia;
using Avalonia.Controls;
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

namespace sephp.Nginx.Views;

public partial class NginxView : ReactiveUserControl<NginxViewModel>
{
    public NginxView()
    {
        InitializeComponent();

        EditorInit();
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

    protected void EditorInit()
    {
        var AccessLogEditor = this.FindControl<TextEditor>("AccessLogEditor");
        var registryOptions = new RegistryOptions(ThemeName.DarkPlus);
        var textMateInstallation = AccessLogEditor.InstallTextMate(registryOptions);

        textMateInstallation.SetGrammarFile("Highlight/nginx.tmLanguage.json");
    }
}