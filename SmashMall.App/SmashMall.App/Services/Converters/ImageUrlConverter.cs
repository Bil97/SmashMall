using System;
using System.Globalization;
using System.Linq;
using SmashMall.Models.Files;
using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.Services;
using Xamarin.Forms;

namespace SmashMall.App.Services.Converters
{
    public class ImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = BaseApi.DefaultImageApi;
            switch (value)
            {
                case Good good:
                    //var good = value as Good;
                    url = $"{BaseApi.ItemsPhotosApi}/{good.Images?.FirstOrDefault()?.Name}";
                    break;
                case TopDeal topDeal:
                    url = $"{BaseApi.ItemsPhotosApi}/{topDeal.Image?.Name}";
                    break;
                case GoodImage image:
                    url = $"{BaseApi.ItemsPhotosApi}/{image.Name}";
                    break;
            }
            //else
            //{
            //    MessageDialog md = new MessageDialog(value.GetType().ToString());
            //    md.ShowAsync();
            //}

            return url;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
