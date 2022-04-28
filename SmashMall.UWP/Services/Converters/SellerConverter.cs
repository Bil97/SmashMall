using SmashMall.Models.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SmashMall.Services.Converters
{
    public class SellerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Good good = new Good();
            string sellerInfo;
            if (value is Good)
            {
                good = value as Good;
            }
            if (good.UserDetails?.Roles?.Contains("Admin") == true)
            {
                sellerInfo = $"Sold by: SmashMall";
            }
            else sellerInfo = $"Sold by: {good.UserDetails?.Name}";
            return sellerInfo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
