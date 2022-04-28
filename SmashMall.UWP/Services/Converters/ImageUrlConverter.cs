using SmashMall.Models.Files;
using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;

namespace SmashMall.Services.Converters
{
    public class ImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string url=BaseApi.DefaultImageApi;
            if (value is Good good)
            {
                //var good = value as Good;
                url = $"{BaseApi.ItemsPhotosApi}/{good.Images?.FirstOrDefault()?.Name}";
            }
            else if(value is TopDeal topDeal)
            {
                url = $"{BaseApi.ItemsPhotosApi}/{topDeal.Image?.Name}";
            }
            else if(value is GoodImage image)
            {
                url = $"{BaseApi.ItemsPhotosApi}/{image.Name}";
            }
            //else
            //{
            //    MessageDialog md = new MessageDialog(value.GetType().ToString());
            //    md.ShowAsync();
            //}

            return url;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
