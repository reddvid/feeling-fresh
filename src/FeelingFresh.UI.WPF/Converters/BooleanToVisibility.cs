using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FeelingFresh.UI.WPF.Converters;

public class BooleanToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value is not null and true) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}