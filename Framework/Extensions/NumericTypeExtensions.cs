using System.Globalization;

namespace Framework.Extensions
{
    public static class NumericTypeExtensions
    {
        public static string ToCurrency(this double amount, int decimalDigits = 2)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone
                ();
            culture.NumberFormat.CurrencyNegativePattern = 1;
            culture.NumberFormat.NegativeSign = "−";
            culture.NumberFormat.CurrencyDecimalDigits = decimalDigits;
            return amount.ToString("C", culture);
        }

        public static string ToCurrency(this decimal amount, int decimalDigits = 2)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.CurrencyNegativePattern = 1;
            culture.NumberFormat.NegativeSign = "−";
            culture.NumberFormat.CurrencyDecimalDigits = decimalDigits;
            return amount.ToString("C", culture);
        }

        public static string ToPercentage(this decimal percent, int decimalDigits = 2)
        {
            var format = "#0";
            if (decimalDigits != 0)
                format = format + "." + new string('0', decimalDigits);
            format = format + "%";
            return percent.ToString(format);
        }
    }
}