using System.Windows;
using System.Windows.Controls;

namespace Nexus.Desktop.Controls;

public partial class NexusCard : UserControl
{
    public NexusCard()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty CardContentProperty =
        DependencyProperty.Register(
            nameof(CardContent),
            typeof(object),
            typeof(NexusCard),
            new PropertyMetadata(null));

    public static readonly DependencyProperty CardPaddingProperty =
        DependencyProperty.Register(
            nameof(CardPadding),
            typeof(Thickness),
            typeof(NexusCard),
            new PropertyMetadata(new Thickness(22)));

    public object? CardContent
    {
        get => GetValue(CardContentProperty);
        set => SetValue(CardContentProperty, value);
    }

    public Thickness CardPadding
    {
        get => (Thickness)GetValue(CardPaddingProperty);
        set => SetValue(CardPaddingProperty, value);
    }
}
