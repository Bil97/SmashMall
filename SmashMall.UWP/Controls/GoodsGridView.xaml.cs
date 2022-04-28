using SmashMall.UWP;
using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.UWP.Pages.Goods;
using SmashMall.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SmashMall.Uno.Shared.Controls
{
    public sealed partial class GoodsGridView : UserControl
    {

        private string property;

        public Frame Frame { get; set; }
        public List<Good> Goods { get; set; } = new List<Good>();
        private readonly NotifyPropertyChanged Notify = new NotifyPropertyChanged();
        public string Property { get => property; set { property = value; Notify.Property = "Property"; } }

        public GoodsGridView()
        {
            this.InitializeComponent();
            Notify = new NotifyPropertyChanged(this);
            Notify.PropertyChanged += Notify_PropertyChanged;
        }

        private void Notify_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            GoodsView.DataContext = Goods;
        }

        private void GridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var parent = sender as FrameworkElement;
            // Here I'm calculating the number of columns I want based on the width of the page
            var columns = Math.Ceiling(parent.ActualWidth / 200);
            ((ItemsWrapGrid)this.GoodsView.ItemsPanelRoot).ItemWidth = e.NewSize.Width / columns;
        }

        private void GoodsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemsList = sender as GridView;
            var good = itemsList.SelectedItem as Good;
            var goods = MainPage.Goods.Where(x => x.Brand?.Id == good?.Brand?.Id).ToList();

            if (Frame != null)
            {
                Frame.Navigate(typeof(Description), new
                {
                    Good = good,
                    RelatedGoods = MainPage.RandomGoods
                });
            }
        }

        async private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            var good = (Good)b.CommandParameter;

            try
            {
                var userId = App.RoamingDataContainer.Values["UserId"];
                if (userId == null) userId = "";

                var content = new StringContent(JsonSerializer.Serialize(good), Encoding.UTF8, "application/json");
                var result = await UnAuthorizedHttp.Client.PostAsync($"api/cart/{userId}", content);

                if (result.IsSuccessStatusCode)
                {
                    using (var stream = await result.Content.ReadAsStreamAsync())
                    {
                        var cart = await JsonSerializer.DeserializeAsync<CartModel>(stream, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        App.RoamingDataContainer.Values["UserId"] = cart.UserId;
                        MainPage.ItemsCount = cart.ItemsCount;
                        MainPage.TotalPrice = cart.TotalPrice;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message, "An error has occured");
                await md.ShowAsync();
            }
        }

        async private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var b = sender as CheckBox;
                var good = (Good)b.CommandParameter;

                var savedToken = App.RoamingDataContainer.Values["AuthToken"];
                if (string.IsNullOrWhiteSpace($"{savedToken}"))
                {
                    b.IsChecked = false;
                    MainPage.LoginCreateAccount = true;
                    return;
                }

                var content = new StringContent(JsonSerializer.Serialize(good), Encoding.UTF8, "application/json");
                /*var result =*/ await AuthorizedHttp.Client.PostAsync($"api/Goods/saved", content);
                //if (!result.IsSuccessStatusCode)
                //{
                //    MessageDialog md = new MessageDialog(result.StatusCode.ToString());
                //    await md.ShowAsync();
                //}
            }
            catch (HttpRequestException ex)
            {
                MessageDialog md = new MessageDialog(ex.Message, "An error occured");
                await md.ShowAsync();
            }
        }

        async private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                var b = sender as CheckBox;
                var good = (Good)b.CommandParameter;

                var savedToken = App.RoamingDataContainer.Values["AuthToken"];
                if (string.IsNullOrWhiteSpace($"{savedToken}"))
                {
                    return;
                }

                await AuthorizedHttp.Client.DeleteAsync($"api/Goods/saved/{good.Id}");
            }
            catch (HttpRequestException ex)
            {
                MessageDialog md = new MessageDialog(ex.Message, "An error occured");
                await md.ShowAsync();
            }
        }
    }
}
