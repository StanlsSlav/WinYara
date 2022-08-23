using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using WinYara.Core.Models;

namespace WinYara.Views;

public sealed partial class RulesDetailControl : UserControl
{
    public YaraRule? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as YaraRule;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register(
        "ListDetailsMenuItem",
        typeof(YaraRule),
        typeof(RulesDetailControl),
        new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged)
    );

    public RulesDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RulesDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
