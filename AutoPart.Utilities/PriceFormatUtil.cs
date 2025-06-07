using System.Globalization;

namespace AutoPart.Utilities
{
    /// <summary>
    /// Provides a shared number format for displaying prices consistently across the app.
    /// </summary>
    public static class PriceFormatUtil
    {
        public static readonly NumberFormatInfo NumberFormat = new NumberFormatInfo
        {
            NumberGroupSeparator = " ",
            NumberDecimalDigits = 2,
            NumberDecimalSeparator = "."
        };
    }
}
