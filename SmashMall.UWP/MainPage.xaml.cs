using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.Models.UserAccount;
using SmashMall.UWP.Pages;
using SmashMall.UWP.Pages.Items;
using SmashMall.Services;
using SmashMall.Services.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmashMall.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static NotifyPropertyChanged Notify = new NotifyPropertyChanged();
        private static int itemsCount = 0;
        private static decimal totalCost = 0;
        private static bool loginCreateAccount = false;
        private static bool loginCreateAccountSuccessful = false;

        public MainPage()
        {
            this.InitializeComponent();
            GetFrame = MainFrame;

            Notify = new NotifyPropertyChanged(this);
            Notify.PropertyChanged += MainPage_PropertyChanged;
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            //SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            var theme = App.RoamingDataContainer.Values["Theme"];
            if (theme != null)
            {
                if (theme.ToString().ToUpper().Contains("DEFAULT")) RequestedTheme = ElementTheme.Default;
                else if (theme.ToString().ToUpper().Contains("DARK")) RequestedTheme = ElementTheme.Dark;
                else
                    RequestedTheme = ElementTheme.Light;

            }
            else RequestedTheme = ElementTheme.Default;

            //if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            //{
            //    ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
            //}
        }

        public static Frame GetFrame { get; private set; }

        private List<GoodCategory> GoodCategories { get; set; }
        private List<TopDeal> TopDeals { get; set; }
        public static List<Good> Goods { get; set; }
        public static UserDetails User { get; set; }
        private List<Good> NewArrivals { get; set; }
        public static List<Good> RandomGoods { get; set; } = new List<Good>();

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                e.Handled = true;
                MainFrame.GoBack();
            }
            else
            {
                e.Handled = false;
            }
        }

        private void NavMenu_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        async private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.RoamingDataContainer.Values["AuthToken"] != null)
                {
                    LoginCreateAccountSuccessful = true;
                }
                else
                {
                    LoginCreateAccountSuccessful = false;
                }

                await GetUser();
                await Load();
            }
            catch (HttpRequestException ex)
            {
                MessageDialog md = new MessageDialog(ex.Message, "An error has occured");
                await md.ShowAsync();
            }
            MainFrame.Navigate(typeof(HomePage), new
            {
                TopDeals,
                NewArrivals,
            });

            refreshNavigationViewItem.Visibility = Visibility.Visible;
        }

        async private Task Load()
        {
            await GetUser();
            await GetItemsCount();
            await GetGoodCategories();
            await GetTopDeals();
            await GetGoods();
        }
        private async Task GetUser()
        {
            //try
            //{
            if (App.RoamingDataContainer.Values["AuthToken"] != null)
            {
                var result = await AuthorizedHttp.Client.GetAsync($"api/ApplicationUsers/{AuthorizedHttp.User.Identity.Name}");
                using (var stream = await result.Content.ReadAsStreamAsync())
                {
                    User = await JsonSerializer.DeserializeAsync<UserDetails>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                AccountDropDown.Content = $"Hi, {User.FirstName}";
            }
            else
            {
                AccountDropDown.Content = "Account";
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
                    using (var stream = await result.Content.ReadAsStreamAsync())
                    {
                        GoodCategories = await JsonSerializer.DeserializeAsync<List<GoodCategory>>(stream, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        NavMenu.DataContext = GoodCategories;
                    }
                }
            }
            catch (JsonException) { }
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
                    using (var stream = await result.Content.ReadAsStreamAsync())
                    {
                        TopDeals = await JsonSerializer.DeserializeAsync<List<TopDeal>>(stream, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                }

            }
            catch (JsonException) { }
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

                if (App.RoamingDataContainer.Values["AuthToken"] == null) result = await UnAuthorizedHttp.Client.GetAsync($"api/Goods");
                else result = await AuthorizedHttp.Client.GetAsync($"api/Goods");

                if (result.IsSuccessStatusCode)
                {
                    using (var stream = await result.Content.ReadAsStreamAsync())
                    {
                        Goods = await JsonSerializer.DeserializeAsync<List<Good>>(stream, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        NewArrivals = Goods?.OrderBy(x => x.DateCreated).TakeLast(12).ToList();
                        if (Goods.Count > 12)
                        {
                            var random = new Random();
                            for (int i = 0; i < 12; i++)
                            {
                                var next = random.Next(0, Goods.Count - 1);
                                RandomGoods.Add(Goods[next]);
                            }
                        }
                        else RandomGoods = Goods;
                    }
                }
            }
            catch (JsonException) { }
            //catch (Exception ex)
            //{
            //    MessageDialog message = new MessageDialog(ex.Message, "An error has occured");
            //    await message.ShowAsync();
            //}
        }

        private void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void Search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

        }

        private void NavMenu_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var category = sender.SelectedItem as GoodCategory;
            var goods = Goods?.Where(c => c.Brand.Subcategory.GoodCategory.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)).ToList();

            MainFrame.Navigate(typeof(Items), new
            {
                Goods = goods,
                RandomGoods
            });

        }

        async private void RefreshNavigationViewItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            try
            {
                await Load();

                HomePage.Goods = Goods;
                HomePage.RandomGoods = RandomGoods;
                HomePage.NewArrivals = NewArrivals;
                HomePage.TopDeals = TopDeals;
            }
            catch (HttpRequestException ex)
            {
                MessageDialog md = new MessageDialog(ex.Message, "An error has occured");
                await md.ShowAsync();
            }
        }

        private void HomeNavigationViewItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(HomePage), new
            {
                TopDeals,
                NewArrivals,
            });
        }

        public static int ItemsCount
        {
            get => itemsCount; set
            {
                itemsCount = value;
                Notify.Property = DateTime.UtcNow.ToString();
            }
        }

        public static decimal TotalPrice
        {
            get => totalCost; set
            {
                totalCost = value;
                Notify.Property = DateTime.UtcNow.ToString();
            }
        }

        public static bool LoginCreateAccount
        {
            get => loginCreateAccount;
            set
            {
                loginCreateAccount = value;
                Notify.Property = DateTime.UtcNow.ToString();
            }
        }

        public static bool LoginCreateAccountSuccessful
        {
            get => loginCreateAccountSuccessful;
            set
            {
                loginCreateAccountSuccessful = value;
                Notify.Property = DateTime.UtcNow.ToString();
            }
        }

        async private Task GetItemsCount()
        {
            //try
            //{
            var userId = App.RoamingDataContainer.Values["UserId"]?.ToString();
            var result = await UnAuthorizedHttp.Client.GetAsync($"api/cart/count/{userId}");

            if (result.IsSuccessStatusCode)
            {
                using (var stream = await result.Content.ReadAsStreamAsync())
                {
                    var cart = await JsonSerializer.DeserializeAsync<CartModel>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    App.RoamingDataContainer.Values["UserId"] = cart.UserId;
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

        private void MainPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (LoginCreateAccount)
            {
                LoginCreateAccountTeachingTip.IsOpen = true;
                LoginCreateAccount = false;
            }

            if (LoginCreateAccountSuccessful)
            {
                LogoutMenuItem.Visibility = Visibility.Visible;
                LoginMenuItem.Visibility = Visibility.Collapsed;
                RegisterMenuItem.Visibility = Visibility.Collapsed;
                AccountDropDown.Content = $"Hi, {User?.FirstName}";
            }
            else
            {
                LogoutMenuItem.Visibility = Visibility.Collapsed;
                LoginMenuItem.Visibility = Visibility.Visible;
                RegisterMenuItem.Visibility = Visibility.Visible;
                AccountDropDown.Content = "Account";
            }

            ItemsText.Text = $"{ItemsCount} items |";
            TotalPriceTextBlock.Text = new CurrencyConverter2(TotalPrice).Currency;
        }

        private void ThemeNavigationViewItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ThemeNavigationViewItem.ContextFlyout.ShowAt(ThemeNavigationViewItem);
        }

        private void SystemTheme_Click(object sender, RoutedEventArgs e)
        {
            this.RequestedTheme = ElementTheme.Default;
            App.RoamingDataContainer.Values["Theme"] = RequestedTheme.ToString();
        }

        private void LightTheme_Click(object sender, RoutedEventArgs e)
        {
            RequestedTheme = ElementTheme.Light;
            App.RoamingDataContainer.Values["Theme"] = RequestedTheme.ToString();
        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            RequestedTheme = ElementTheme.Dark;
            App.RoamingDataContainer.Values["Theme"] = RequestedTheme.ToString();
        }

        private void ShoppingCartButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Pages.ShoppingCart.Cart));
        }

        async private void LogoutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await AuthorizedHttp.Client.PostAsync($"api/ApplicationUsers/logout", null);
                App.RoamingDataContainer.Values["AuthToken"] = null;
                App.RoamingDataContainer.Values["UserId"] = null;
                LoginCreateAccountSuccessful = false;

                MainFrame.Navigate(typeof(HomePage), new
                {
                    TopDeals,
                    NewArrivals,
                });
            }
            catch (Exception ex)
            {
                MessageDialog md = new MessageDialog(ex.Message, "An error occured");
                await md.ShowAsync();
            }
        }

        private void LoginMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoginCreateAccount = true;
            LoginCreateAccountTeachingTip.Title = "";
            LoginCreateAccountTeachingTip.LoginCreateAccount();
        }

        private void RegisterMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoginCreateAccount = true;
            LoginCreateAccountTeachingTip.Title = "Login";
            LoginCreateAccountTeachingTip.LoginCreateAccount();
        }
    }
}
