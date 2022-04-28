using SmashMall.UWP.Controls;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmashMall.UWP.Pages.UserAccount
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public string title = "Login";
        public Login()
        {
            this.InitializeComponent();
            //LoginCreateAccountTeachingTip.Title = title;
            LoginCreateAccountTeachingTip.LoginCreateAccount();

            //var login = new LoginCreateAccountTeachingTip
            //{
            //    IsOpen = true
            //};


            //if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            //{
            //    ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
            //}

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginCreateAccountTeachingTip.IsOpen = true;
        }
    }
}
