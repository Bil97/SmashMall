using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SmashMall.App.Pages;
using SmashMall.App.Pages.Items;
using SmashMall.Data;
using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.Models.UserAccount;
using SmashMall.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmashMall.App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        private static NotifyPropertyChanged notify = new();
        private static int itemsCount = 0;
        private static decimal totalCost = 0;
        private static bool loginCreateAccount = false;
        private static bool loginCreateAccountSuccessful = false;
        private readonly Database _database = new();

        public MainPage()
        {
            InitializeComponent();
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;

            var theme = _database.GetUser().Theme;
            if (theme != null)
            {
                if (theme.ToUpper().Contains("DEFAULT")) Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                else if (theme.ToUpper().Contains("DARK")) Application.Current.UserAppTheme = OSAppTheme.Dark;
                else Application.Current.UserAppTheme = OSAppTheme.Light;
            }
            else Application.Current.UserAppTheme = OSAppTheme.Unspecified;

            this.Appearing += MainPage_Appearing;
        }

        private async void MainPage_Appearing(object sender, EventArgs e)
        {
            try
            {
                LoginCreateAccountSuccessful = string.IsNullOrEmpty(_database.GetUser().AuthToken);

            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert(ex.Message, "An error has occured", "OK");
            }

            await MainPageDetail.MainFrame.Navigation.PushAsync(new HomePage());

            MainPageFlyout.Refresh.IsVisible = true;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is not GoodCategory category)
                return;

            var goods = Goods?.Where(c =>
                    c.Brand.Subcategory.GoodCategory.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            await MainPageDetail.GetFrame.Navigation.PushAsync(new Items(goods));

            //Detail = new NavigationPage(new Items(goods));
            IsPresented = false;

            FlyoutPage.ListView.SelectedItem = null;
        }

        public static List<GoodCategory> GoodCategories { get; set; }
        public static List<TopDeal> TopDeals { get; set; }
        public static List<Good> Goods { get; set; }
        public static UserDetails User { get; set; }
        public static List<Good> NewArrivals { get; set; }
        public static List<Good> RandomGoods { get; set; } = new();

        private async Task Load()
        {
            await GetUser();
            await GetItemsCount();
            await GetGoodCategories();
            await GetTopDeals();
            await GetGoods();
        }

        private async Task GetUser()
        {
            ////try
            ////{
            if (!string.IsNullOrEmpty(_database.GetUser().AuthToken))
            {
                var result =
                    await AuthorizedHttp.Client.GetAsync($"api/ApplicationUsers/{AuthorizedHttp.User.Identity.Name}");
                using (var stream = await result.Content.ReadAsStreamAsync())
                {
                    User = await JsonSerializer.DeserializeAsync<UserDetails>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                MainPageDetail.AccountPicker.Title = User != null ? $"Hi, {User.FirstName}" : "Account";
            }
            else
            {
                MainPageDetail.AccountPicker.Title = "Account";
            }
            //}
            //catch (HttpRequestException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //catch { }
        }

        private async Task GetGoodCategories()
        {
            try
            {
                var result = await UnAuthorizedHttp.Client.GetAsync("api/GoodsCategories/category-goods");
                if (result.IsSuccessStatusCode)
                {
                    using var stream = await result.Content.ReadAsStreamAsync();
                    GoodCategories = await JsonSerializer.DeserializeAsync<List<GoodCategory>>(stream,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    FlyoutPage.MenuItemsListView.BindingContext = GoodCategories;
                }
            }
            catch (JsonException)
            {
            }
            //catch (Exception ex)
            //{
            //    MessageDialog message = new MessageDialog(ex.Message, "An error has occured");
            //    await message.ShowAsync();
            //}
        }

        private async Task GetTopDeals()
        {
            try
            {
                var result = await UnAuthorizedHttp.Client.GetAsync($"api/TopDeals");
                if (result.IsSuccessStatusCode)
                {
                    using var stream = await result.Content.ReadAsStreamAsync();
                    TopDeals = await JsonSerializer.DeserializeAsync<List<TopDeal>>(stream,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                }
            }
            catch (JsonException)
            {
            }
            //catch (Exception ex)
            //{
            //    MessageDialog message = new MessageDialog(ex.Message, "An error has occured");
            //    await message.ShowAsync();
            //}
        }

        private async Task GetGoods()
        {
            try
            {
                HttpResponseMessage result;

                if (string.IsNullOrEmpty(_database.GetUser().AuthToken))
                    result = await UnAuthorizedHttp.Client.GetAsync($"api/Goods");
                else result = await AuthorizedHttp.Client.GetAsync($"api/Goods");

                if (result.IsSuccessStatusCode)
                {
                    using var stream = await result.Content.ReadAsStreamAsync();
                    Goods = await JsonSerializer.DeserializeAsync<List<Good>>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    NewArrivals = Goods?.OrderByDescending(x => x.DateCreated).Take(12).ToList();
                    if (Goods is { Count: > 12 })
                    {
                        var random = new Random();
                        for (var i = 0; i < 12; i++)
                        {
                            var next = random.Next(0, Goods.Count - 1);
                            RandomGoods.Add(Goods[next]);
                        }
                    }
                    else RandomGoods = Goods;
                }
            }
            catch (JsonException)
            {
            }
            //catch (Exception ex)
            //{
            //    MessageDialog message = new MessageDialog(ex.Message, "An error has occured");
            //    await message.ShowAsync();
            //}
        }

        // private void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        // {
        // }
        //
        // private void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        // {
        // }
        //
        // private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        // {
        // }

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Load();

                // HomePage.Goods = Goods;
                // HomePage.RandomGoods = RandomGoods;
                // HomePage.NewArrivals = NewArrivals;
                // HomePage.TopDeals = TopDeals;
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert(ex.Message, "An error has occured", "OK");
            }
        }

        public static int ItemsCount
        {
            get => itemsCount;
            set
            {
                itemsCount = value;
                notify.Property = DateTime.UtcNow.ToString();
            }
        }

        public static decimal TotalPrice
        {
            get => totalCost;
            set
            {
                totalCost = value;
                notify.Property = DateTime.UtcNow.ToString();
            }
        }

        public static bool LoginCreateAccount
        {
            get => loginCreateAccount;
            set
            {
                loginCreateAccount = value;
                notify.Property = DateTime.UtcNow.ToString();
            }
        }

        public static bool LoginCreateAccountSuccessful
        {
            get => loginCreateAccountSuccessful;
            set
            {
                loginCreateAccountSuccessful = value;
                notify.Property = DateTime.UtcNow.ToString();
            }
        }

        private async Task GetItemsCount()
        {
            //try
            //{
            var user = _database.GetUser();
            var result = await UnAuthorizedHttp.Client.GetAsync($"api/cart/count/{user.Id}");

            if (result.IsSuccessStatusCode)
            {
                using var stream = await result.Content.ReadAsStreamAsync();
                var cart = await JsonSerializer.DeserializeAsync<CartModel>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (cart != null)
                {
                    user.Id = cart.UserId;
                    await _database.SaveUserAsync(user);
                    ItemsCount = cart.ItemsCount;
                    TotalPrice = cart.TotalPrice;
                }
            }
            else
            {
                ItemsCount = 0;
                TotalPrice = 0;
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageDialog md = new MessageDialog(ex.Message);
            //    await md.ShowAsync();
            //}
        }
    }
}