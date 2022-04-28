using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmashMall.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {

        private readonly DispatcherTimer timer = new DispatcherTimer();
        private int topDealsCount = 0;
        private int currentDeal = 0;
        private static List<Good> goods;

        private static NotifyPropertyChanged Notify;
        private static List<Good> randomGoods;
        private static List<Good> newArrivals;
        private static List<TopDeal> topDeals;

        public HomePage()
        {
            this.InitializeComponent();

            Notify = new NotifyPropertyChanged(this);
            Notify.PropertyChanged += Items_PropertyChanged;

            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
        }

        public static List<Good> Goods
        {
            get => goods; set
            {
                goods = value;
                Notify.Property = DateTime.UtcNow.ToString();
            }
        }
        public static List<Good> RandomGoods
        {
            get => randomGoods; set
            {
                randomGoods = value;
                Notify.Property = DateTime.UtcNow.ToString();
            }
        }
        public static List<Good> NewArrivals
        {
            get => newArrivals; set
            {
                newArrivals = value;
                Notify.Property = DateTime.UtcNow.ToString();
            }
        }
        public static List<TopDeal> TopDeals
        {
            get => topDeals; set
            {
                topDeals = value;
                Notify.Property = DateTime.UtcNow.ToString();
            }
        }


        private void Timer_Tick(object sender, object e)
        {
            timer.Stop();
            if (currentDeal >= topDealsCount) currentDeal = 0;
            topDealsView.SelectedIndex = currentDeal++;
            timer.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dynamic param = e.Parameter;
            if (param == null)
            {
                return;
            }
            Goods = MainPage.Goods;
            RandomGoods = MainPage.RandomGoods;
            NewArrivals = param.NewArrivals;
            TopDeals = param.TopDeals;
            ItemsChanged();
        }

        private void ItemsChanged()
        {
            NewArrivalsView.Goods = NewArrivals;
            NewArrivalsView.Property = DateTime.Now.ToString();
            NewArrivalsView.Frame = Frame;

            ItemsYouMayLikeView.Goods = RandomGoods;
            ItemsYouMayLikeView.Property = DateTime.Now.ToString();
            ItemsYouMayLikeView.Frame = Frame;

            topDealsView.DataContext = TopDeals; ;
            if (TopDeals != null)
                topDealsCount = TopDeals.Count;
            timer.Start();
        }

        private void Items_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemsChanged();
        }

        private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            var category = listBox.SelectedItem as GoodCategory;
            var goods = Goods?.Where(c => c.Brand.Subcategory.GoodCategory.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)).ToList();

            Frame.Navigate(typeof(Items.Items), new
            {
                Goods = goods,
                RandomGoods
            });

        }

        /*async*/
        private void TopDealButton_Click(object sender, RoutedEventArgs e)
        {
            var topDeal = topDealsView.SelectedItem as TopDeal;
            System.Diagnostics.Debug.WriteLine($"\n\n\n\n Topdeal Url: {topDeal.PageUrl}\n\n\n");
        }
    }
}
