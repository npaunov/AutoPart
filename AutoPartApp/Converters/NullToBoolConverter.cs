using System.Globalization;
using System.Windows.Data;

namespace AutoPartApp;

/// <summary>
/// Converts a null value to false and non-null to true (for enabling/disabling controls).
/// </summary>
public class NullToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value != null;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
