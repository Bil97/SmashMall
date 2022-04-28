using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmashMall.App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageFlyout : ContentPage
    {
        public ListView ListView;

        public static Button Refresh;
       //static MainPageFlyoutViewModel notify= new ();

        public MainPageFlyout()
        {
            InitializeComponent();

            ListView = MenuItemsListView;
            //Refresh_Button = RefreshButton;
            this.PropertyChanged += Property_Changed;
        }

        // private static bool isVisibleRefreshButton = false;

        // public static bool IsVisibleRefreshButton
        // {
        //     get { return isVisibleRefreshButton; }
        //     set
        //     {
        //         isVisibleRefreshButton = true;
        //         OnPropertyChanged(nameof(IsVisibleRefreshButton));
        //     }
        // }

        private void Property_Changed(object sender, PropertyChangedEventArgs e)
        {
        }

        private void HomeButton_OnClicked(object sender, EventArgs e)
        {
            MainPageDetail.GetFrame.Navigation.PopToRootAsync();
        }

        private void ThemeButton_OnClicked(object sender, EventArgs e)
        {
        }
    }
}