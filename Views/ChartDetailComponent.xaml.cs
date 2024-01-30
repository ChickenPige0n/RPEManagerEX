using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using RPEManagerEX.Core.Models;

namespace RPEManagerEX.Views;

public sealed partial class ChartDetailComponent : UserControl
{
    public SampleOrder? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as SampleOrder;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(SampleOrder), typeof(ChartDetailComponent), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public ChartDetailComponent()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ChartDetailComponent control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
