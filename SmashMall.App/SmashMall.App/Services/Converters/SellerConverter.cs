using System;
using System.Globalization;
using SmashMall.Models.Goods;
using Xamarin.Forms;

namespace SmashMall.App.Services.Converters
{
    public class SellerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var good = new Good();
            if (value is Good good1)
            {
                good = good1;
            }

            var sellerInfo = good.UserDetails?.Roles?.Contains("Admin") == true
                ? "Sold by: SmashMall"
                : $"Sold by: {good.UserDetails?.Name}";
            return sellerInfo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}