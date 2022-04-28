using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using SmashMall.App.Pages.Goods;
using SmashMall.Data;
using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmashMall.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoodsView : ContentView
    {
        private string _property;

        public List<Good> Goods { get; set; } = new();
        private readonly NotifyPropertyChanged _notify;

        public string Property
        {
            get => _property;
            set
            {
                _property = value;
                _notify.Property = "Property";
            }
        }

        private readonly Database _database = new();

        public GoodsView()
        {
            InitializeComponent();
            _notify = new NotifyPropertyChanged(this);
            _notify.PropertyChanged += Notify_PropertyChanged;
        }

        private void Notify_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            GoodsListView.BindingContext = Goods;
        }

        private async void AddToCartButton_Click(object sender, EventArgs e)
        {
            if (sender is not Button b) return;
            var good = (Good)b.CommandParameter;

            try
            {
                var user = _database.GetUser();
                user.Id ??= "";

                var content = new StringContent(JsonSerializer.Serialize(good), Encoding.UTF8, "application/json");
                var result = await UnAuthorizedHttp.Client.PostAsync($"api/cart/{user.Id}", content);

                if (!result.IsSuccessStatusCode) return;
                using var stream = await result.Content.ReadAsStreamAsync();
                var cart = await JsonSerializer.DeserializeAsync<CartModel>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (cart == null) return;
                user.Id = cart.UserId;
                await _database.SaveUserAsync(user);
                MainPage.ItemsCount = cart.ItemsCount;
                MainPage.TotalPrice = cart.TotalPrice;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(ex.Message, "An error has occured", "OK");
            }
        }

        private async void GoodsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var good = e.Item as Good;
            var relatedGoods = MainPage.Goods.Where(x => x.Brand?.Id == good?.Brand?.Id).ToList();

            await MainPageDetail.GetFrame.Navigation.PushAsync(new Description(good, relatedGoods));
        }

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var user = _database.GetUser();
            if (e.Value)
            {
                try
                {
                    var b = sender as CheckBox;
                    var parent = b.Parent;
                    var good = (Good)b.BindingContext;

                    if (string.IsNullOrWhiteSpace(user.AuthToken))
                    {
                        b.IsChecked = false;
                        MainPage.LoginCreateAccount = true;
                        return;
                    }

                    var content = new StringContent(JsonSerializer.Serialize(good), Encoding.UTF8, "application/json");
                    await AuthorizedHttp.Client.PostAsync($"api/Goods/saved", content);
                }
                catch (HttpRequestException ex)
                {
                    await Application.Current.MainPage.DisplayAlert(ex.Message, "An error occured", "OK");
                }
            }
            else
            {
                try
                {
                    var b = sender as CheckBox;
                    var good = (Good)b.BindingContext;

                    if (string.IsNullOrWhiteSpace(user.AuthToken))
                    {
                        return;
                    }

                    await AuthorizedHttp.Client.DeleteAsync($"api/Goods/saved/{good.Id}");
                }
                catch (HttpRequestException ex)
                {
                    await Application.Current.MainPage.DisplayAlert(ex.Message, "An error occured", "OK");
                }
            }
        }
    }
}