using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace AutoPartApp.Converters
{
    /// <summary>
    /// Converts a DataGrid item and its ItemsControl to a 1-based row number for display.
    /// Ensures stable row numbers even with virtualization and scrolling.
    /// </summary>
    public class RowNumberConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts the current item and its ItemsControl to a 1-based row number as a string.
        /// </summary>
        /// <param name="values">
        /// values[0]: The current item (row data).
        /// values[1]: The ItemsControl (DataGrid).
        /// </param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// The 1-based row number as a string, or an empty string if the index cannot be determined.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values[0];
            var itemsControl = values[1] as ItemsControl;
            if (item == null || itemsControl == null)
                return "";

            int index = itemsControl.Items.IndexOf(item);
            return (index >= 0 ? (index + 1).ToString() : "");
        }

        /// <summary>
        /// Not implemented. Throws <see cref="NotImplementedException"/> because row number conversion is one-way only. Requred by IMultiValueConverter.
        /// </summary>
        /// <param name="value">The value produced by the binding target.</param>
        /// <param name="targetTypes">The array of types to convert to.</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Nothing. Always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}