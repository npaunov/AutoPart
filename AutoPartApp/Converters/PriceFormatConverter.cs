using System.Globalization;
using System.Windows.Data;
using AutoPart.Utilities;

namespace AutoPartApp.Converters
{
    /// <summary>
    /// Converts decimal or double price values to a formatted string using a shared number format.
    /// Ensures prices are displayed with two decimal places and a space as the thousands separator.
    /// </summary>
    public class PriceFormatConverter : IValueConverter
    {
        /// <summary>
        /// Converts a decimal or double value to a formatted price string using the shared number format.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A formatted string representation of the price, or the original value if not a decimal or double.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal dec)
                return dec.ToString("N2", PriceFormatUtil.NumberFormat);
            if (value is double dbl)
                return dbl.ToString("N2", PriceFormatUtil.NumberFormat);
            return value;
        }

        /// <summary>
        /// Not implemented. Throws <see cref="NotImplementedException"/> because price formatting is one-way only.
        /// </summary>
        /// <param name="value">The value produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Nothing. Always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
