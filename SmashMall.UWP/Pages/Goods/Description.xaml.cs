using SmashMall.Models.Files;
using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmashMall.UWP.Pages.Goods
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Description : Page
    {
        public Description()
        {
            this.InitializeComponent();
        }

        private Good Good { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dynamic param = e.Parameter;
            if (param == null) return;
            Good = param.Good;
            descriptionTextBlock.Text = Good.Description ?? "";
            imagesList.DataContext = Good.Images;
            RelatedItems.Frame = Frame;
            RelatedItems.Goods = param.RelatedGoods;
            RelatedItems.Property = DateTime.Now.ToString();
            goodName.Text = Good.Name;
            goodBrand.Text = $"Brand: {Good.Brand?.Name}";
            goodsFromSimilarBrand.Content = $"| Similar items from {Good.Brand?.Name}";
            sellerTextBlock.DataContext = Good;
            priceTextBlock.DataContext = Good;
            discountTextBlock.Text = $"\t-{Good.Discount}%";
            discountedPriceTextBlock.DataContext = Good;
            isSavedCheckBox.IsChecked = Good.IsSaved;
            quantityTextBlock.Text = $"{Good.Quantity} in stock";
            featuresList.DataContext = Good.KeyFeatures;
            weightTextBlock.Text = $"Weight: {Good.Weight} kg";
            sizeTextBlock.Text = $"Size: {Good.Size} (cm)";
            modelTextBlock.Text = $"Model: {Good.Model}";

            if (Good.Images.Count > 0)
            {
                imagesList.SelectedIndex = 0;
            }
        }

        private void ImagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as GridView;
            var image = gridView.SelectedItem as GoodImage;
            goodImage.DataContext = image;
        }

        private void GoodsFromSimilarBrand_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        async private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userId = App.RoamingDataContainer.Values["UserId"];
                if (userId == null) userId = "";

                var content = new StringContent(JsonSerializer.Serialize(Good), Encoding.UTF8, "application/json");
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

    }
}
