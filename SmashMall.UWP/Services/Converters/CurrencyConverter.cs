using SmashMall.Models.Goods;
using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace SmashMall.Services.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Good good = new Good();
            string culture = BaseApi.Culture;
            if (value is Good)
            {
                good = value as Good;
            }

            if (parameter is string param)
            {
                if (param == "Discount")
                {
                    return $" {Convert(good.Price, good.Discount).ToString("C2", CultureInfo.CreateSpecificCulture(culture))}";
                }
            }

            return $" {Convert(good.Price, 0).ToString("C2", CultureInfo.CreateSpecificCulture(culture))}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
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
