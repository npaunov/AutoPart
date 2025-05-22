using System.Globalization;

namespace AutoPartApp.Utilities
{
    /// <summary>
    /// Provides a shared number format for displaying prices consistently across the app.
    /// </summary>
    public static class PriceFormat
    {
        public static readonly NumberFormatInfo NumberFormat = new NumberFormatInfo
        {
            NumberGroupSeparator = " ",
            NumberDecimalDigits = 2,
            NumberDecimalSeparator = "."
        };
    }
}
