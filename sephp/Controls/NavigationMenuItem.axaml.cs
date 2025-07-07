using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace sephp.Controls;

public partial class NavigationMenuItem : RadioButton
{
    public NavigationMenuItem()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<Geometry?> IconProperty =
            AvaloniaProperty.Register<NavigationMenuItem, Geometry?>(nameof(Icon));

    public Geometry? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<IBrush?> IconColorProperty =
            AvaloniaProperty.Register<NavigationMenuItem, IBrush?>(nameof(IconColor), Brushes.Black);

    public IBrush? IconColor
    {
        get => GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }
}