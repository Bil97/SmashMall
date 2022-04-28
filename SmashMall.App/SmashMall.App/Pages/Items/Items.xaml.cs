using System.Collections.Generic;
using SmashMall.Models.Goods;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmashMall.App.Pages.Items
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Items : ContentPage
    {
        public Items(List<Good> goods/*, List<Good> randomGoods*/)
        {
            InitializeComponent();

            Goods.AllGoods = goods;
           //Goods.RandomGoods = randomGoods;
        }

    }
}