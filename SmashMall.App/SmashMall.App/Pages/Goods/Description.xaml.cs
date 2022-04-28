using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using SmashMall.Data;
using SmashMall.Models.Files;
using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmashMall.App.Pages.Goods
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Description : ContentPage
    {
        private readonly Database _database = new();

        public Description(Good good, List<Good> relatedGoods)
        {
            InitializeComponent();

            Good = good;
            RelatedItems.Goods = relatedGoods;

            DescriptionTextBlock.Text = Good.Description ?? "";
            ImagesList.BindingContext = Good.Images;

            RelatedItems.Property = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            GoodName.Text = Good.Name;
            GoodBrand.Text = $"Brand: {Good.Brand?.Name}";
            GoodsFromSimilarBrand.Text = $"| Similar items from {Good.Brand?.Name}";

            SellerTextBlock.BindingContext = Good;
            PriceTextBlock.BindingContext = Good;
            DiscountTextBlock.Text = $"\t-{Good.Discount}%";
            DiscountedPriceTextBlock.BindingContext = Good;
            IsSavedCheckBox.IsChecked = Good.IsSaved;
            QuantityTextBlock.Text = $"{Good.Quantity} in stock";
            FeaturesList.BindingContext = Good.KeyFeatures;
            WeightText.Text = $"Weight: {Good.Weight} kg";
            SizeText.Text = $"Size: {Good.Size} (cm)";
            ModelText.Text = $"Model: {Good.Model}";

            // if (Good.Images.Count <= 0) return;
            // ImagesList. = 0;
        }

        private Good Good { get; set; }

        private void ImagesList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var image = e.Item as GoodImage;
            GoodImage.BindingContext = image;
        }

        private void GoodsFromSimilarBrand_Click(object sender, EventArgs e)
        {
        }

        private async void AddToCartButton_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                var user = _database.GetUser();
                user.Id ??= "";

                var content = new StringContent(JsonSerializer.Serialize(Good), Encoding.UTF8, "application/json");
                var result = await UnAuthorizedHttp.Client.PostAsync($"api/cart/{user.Id}", content);

                if (!result.IsSuccessStatusCode) return;
                using var stream = await result.Content.ReadAsStreamAsync();
                var cart = await JsonSerializer.DeserializeAsync<CartModel>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (cart == null) return;
                user.Id = cart.UserId;
                MainPage.ItemsCount = cart.ItemsCount;
                MainPage.TotalPrice = cart.TotalPrice;
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, "An error has occured", "OK");
            }
        }
    }
}