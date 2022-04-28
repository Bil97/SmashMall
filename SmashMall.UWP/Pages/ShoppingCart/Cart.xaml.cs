using SmashMall.Models.Items;
using SmashMall.Models.Orders;
using SmashMall.UWP.Pages.Goods;
using SmashMall.Services;
using SmashMall.Services.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmashMall.UWP.Pages.ShoppingCart
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Cart : Page
    {
        public Cart()
        {
            this.InitializeComponent();
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await GetGoods();
        }
        async private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await GetGoods();
        }

        async private Task GetGoods()
        {
            try
            {
                var userId = App.RoamingDataContainer.Values["UserId"];
                if (userId == null) userId = "";
                var result = await UnAuthorizedHttp.Client.GetAsync($"api/cart/{userId}");

                if (result.IsSuccessStatusCode)
                {
                    using (var stream = await result.Content.ReadAsStreamAsync())
                    {
                        var goods = await JsonSerializer.DeserializeAsync<List<GoodCart>>(stream,
                           new JsonSerializerOptions
                           {
                               PropertyNameCaseInsensitive = true
                           });

                        TotalPriceTextBlock.Text = new CurrencyConverter2(goods.Sum(x => x.TotalPrice)).Currency;
                        GoodsView.DataContext = goods;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async private Task ChangeQuantity(Guid cartId, int quantity)
        {
            try
            {
                if (quantity <= 0) await RemoveFromCart(cartId);

                var cartItem = new GoodCartModel { CartId = cartId, Quantity = quantity };
                var userId = App.RoamingDataContainer.Values["UserId"];
                if (userId == null) userId = "";

                //var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                HttpResponseMessage result;
                //if (authState.User.Identity.IsAuthenticated)
                //{

                //    result = await _httpClient.PutAsJsonAsync<GoodCartModel>($"api/cart/{userId}", cartItem);
                //}
                //else
                //{
                var content = new StringContent(JsonSerializer.Serialize(cartItem), Encoding.UTF8, "application/json");
                result = await UnAuthorizedHttp.Client.PutAsync($"api/cart/{userId}", content);
                //}
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

                        await GetGoods();
                    }
                }
            }
            catch { }
        }

        async private Task RemoveFromCart(Guid cartId)
        {
            try
            {
                var result = await UnAuthorizedHttp.Client.DeleteAsync($"api/cart/{cartId}");
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

                        await GetGoods();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message, "An error occured");
                await md.ShowAsync();
            }
        }
        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var parent = sender as FrameworkElement;
            // Here I'm calculating the number of columns I want based on the width of the page
            var columns = Math.Ceiling(parent.ActualWidth / 900);
            ((ItemsWrapGrid)this.GoodsView.ItemsPanelRoot).ItemWidth = e.NewSize.Width / columns;
        }

        private void GoodsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemsList = sender as GridView;
            var good = itemsList.SelectedItem as GoodCart;
            var goods = MainPage.Goods.Where(x => x.Brand?.Id == good?.Good?.Brand?.Id).ToList();
            if (Frame != null)
            {
                Frame.Navigate(typeof(Description), new
                {
                    Good = good.Good,
                    RelatedGoods = MainPage.RandomGoods
                });
            }
        }


    }
}
