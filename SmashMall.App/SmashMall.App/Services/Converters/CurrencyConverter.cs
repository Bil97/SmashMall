using System;
using System.Globalization;
using SmashMall.Models.Goods;
using Xamarin.Forms;

namespace SmashMall.App.Services.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var good = new Good();
            if (value is Good good1)
            {
                good = good1;
            }

            if (!(parameter is string param)) return $" {Convert(good.Price, 0).ToString("C2", culture)}";
            return param == "Discount" ? $" {Convert(good.Price, good.Discount).ToString("C2", culture)}" : $" {Convert(good.Price, 0).ToString("C2", culture)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private decimal Convert(decimal amount, double discount)
        {
            // Do the conversion here and asign the value to ConvertedAmount

            discount = (100 - discount);
            var price = (decimal)discount / 100 * amount;

            return price;
        }
    }
}
