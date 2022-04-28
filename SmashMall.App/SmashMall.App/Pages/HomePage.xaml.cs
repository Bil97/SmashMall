using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Timers;
using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmashMall.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly Timer _timer = new();
        private int _topDealsCount = 0;
        private int _currentDeal = 0;
        private static List<Good> goods;

        private static NotifyPropertyChanged notify;
        // private static List<Good> randomGoods;
        // private static List<Good> newArrivals;
        // private static List<TopDeal> topDeals;

        public HomePage()
        {
            InitializeComponent();

            notify = new NotifyPropertyChanged(this);
            notify.PropertyChanged += Items_PropertyChanged;

            _timer.Interval = 1000;
            _timer.Elapsed += Timer_Elapsed;
        }

        // // public static List<Good> Goods
        // // {
        // //     get => goods;
        // //     set
        // //     {
        // //         goods = value;
        // //         notify.Property = DateTime.UtcNow.ToString();
        // //     }
        // // }
        // //
        // // public static List<Good> RandomGoods
        // // {
        // //     get => randomGoods;
        // //     set
        // //     {
        // //         randomGoods = value;
        // //         notify.Property = DateTime.UtcNow.ToString();
        // //     }
        // // }
        // //
        // // public static List<Good> NewArrivals
        // // {
        // //     get => newArrivals;
        // //     set
        // //     {
        // //         newArrivals = value;
        // //         notify.Property = DateTime.UtcNow.ToString();
        // //     }
        // // }
        // //
        // // public static List<TopDeal> TopDeals
        // // {
        // //     get => topDeals;
        // //     set
        // //     {
        // //         topDeals = value;
        // //         notify.Property = DateTime.UtcNow.ToString();
        // //     }
        // // }
        //
        // private void HomePage_OnAppearing(object sender, EventArgs e)
        // {
        //     Goods = MainPage.Goods;
        //     RandomGoods = MainPage.RandomGoods;
        //
        //     ItemsChanged();
        // }

        private void CategoriesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var category = e.Item as GoodCategory;
            var list = MainPage.Goods?.Where(c =>
                    c.Brand.Subcategory.GoodCategory.Name.Equals(category?.Name,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

            MainPageDetail.GetFrame.Navigation.PushAsync(new Items.Items(list));
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            if (_currentDeal >= _topDealsCount) _currentDeal = 0;
            TopDealsView.Position = _currentDeal++;
            _timer.Start();
        }

        private void ItemsChanged()
        {
            NewArrivalsView.Goods = MainPage.NewArrivals;
            NewArrivalsView.Property = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            ItemsYouMayLikeView.Goods = MainPage.RandomGoods;
            ItemsYouMayLikeView.Property = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            TopDealsView.BindingContext = MainPage.TopDeals;
            if (MainPage.TopDeals != null)
                _topDealsCount = MainPage.TopDeals.Count;
            _timer.Start();
        }

        private void Items_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemsChanged();
        }

        private void TopDealButton_Click(object sender, EventArgs eventArgs)
        {
            var topDeal = TopDealsView.CurrentItem as TopDeal;
            System.Diagnostics.Debug.WriteLine($"\n\n\n\n Topdeal Url: {topDeal?.PageUrl}\n\n\n");
        }
    }
}