using System;
using SmashMall.App.Pages;
using SmashMall.Data;
using SmashMall.Services;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace SmashMall.App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage
    {
        private Database _database = new();
        public MainPageDetail()
        {
            InitializeComponent();

            AboutUsLabel.Text = AboutUs;
            GetFrame = MainFrame;
        }

        private const string AboutUs =
            "Smashmall is an online store where you can purchase a wide variety of items such as mobile phones, tablets, desktop computers &amp; laptops, women's fashion, men's fashion, furniture, electronics and more online and have them delivered to you.";

        public static Frame GetFrame { get; private set; }
       
        // private void MainPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        // {
        //     if (LoginCreateAccount)
        //     {
        //         LoginCreateAccountTeachingTip.IsOpen = true;
        //         LoginCreateAccount = false;
        //     }
        //
        //     if (LoginCreateAccountSuccessful)
        //     {
        //         LogoutMenuItem.Visibility = Visibility.Visible;
        //         LoginMenuItem.Visibility = Visibility.Collapsed;
        //         RegisterMenuItem.Visibility = Visibility.Collapsed;
        //         AccountDropDown.Content = $"Hi, {User?.FirstName}";
        //     }
        //     else
        //     {
        //         LogoutMenuItem.Visibility = Visibility.Collapsed;
        //         LoginMenuItem.Visibility = Visibility.Visible;
        //         RegisterMenuItem.Visibility = Visibility.Visible;
        //         AccountDropDown.Content = "Account";
        //     }
        //
        //     ItemsText.Text = $"{ItemsCount} items |";
        //     TotalPriceTextBlock.Text = new CurrencyConverter2(TotalPrice).Currency;
        // }
        //
        // private void ThemeNavigationViewItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        // {
        //     ThemeNavigationViewItem.ContextFlyout.ShowAt(ThemeNavigationViewItem);
        // }
        //
        // private void SystemTheme_Click(object sender, EventArgs e)
        // {
        //     Application.Current.UserAppTheme = OSAppTheme.Unspecified;
        //     var user = _database.GetUser();
        //     user.Theme = "DEFAULT";
        //     _database.SaveUserAsync(user);
        // }
        //
        // private void LightTheme_Click(object sender, EventArgs e)
        // {
        //     Application.Current.UserAppTheme = OSAppTheme.Light;
        //     var user = _database.GetUser();
        //     user.Theme = "LIGHT";
        //     _database.SaveUserAsync(user);
        // }
        //
        // private void DarkTheme_Click(object sender, EventArgs e)
        // {
        //     Application.Current.UserAppTheme = OSAppTheme.Dark;
        //     var user = _database.GetUser();
        //     user.Theme = "DARK";
        //     _database.SaveUserAsync(user);
        // }
        //
        // private void ShoppingCartButton_Click(object sender, RoutedEventArgs e)
        // {
        //     MainFrame.Navigation.PushAsync(new ShoppingCart.Cart());
        // }
        //
        // private async void LogoutMenuItem_Click(object sender, RoutedEventArgs e)
        // {
        //     try
        //     {
        //         await AuthorizedHttp.Client.PostAsync($"api/ApplicationUsers/logout", null);
        //         App.RoamingDataContainer.Values["AuthToken"] = null;
        //         App.RoamingDataContainer.Values["UserId"] = null;
        //         LoginCreateAccountSuccessful = false;
        //
        //         await MainFrame.Navigation.PushAsync(new HomePage());
        //     }
        //     catch (Exception ex)
        //     {
        //         await DisplayAlert(ex.Message, "An error occured","OK");
        //     }
        // }
        //
        // private void LoginMenuItem_Click(object sender, EventArgs e)
        // {
        //     LoginCreateAccount = true;
        //     LoginCreateAccountTeachingTip.Title = "";
        //     LoginCreateAccountTeachingTip.LoginCreateAccount();
        // }
        //
        // private void RegisterMenuItem_Click(object sender, EventArgs e)
        // {
        //     LoginCreateAccount = true;
        //     LoginCreateAccountTeachingTip.Title = "Login";
        //     LoginCreateAccountTeachingTip.LoginCreateAccount();
        // }
    }
}