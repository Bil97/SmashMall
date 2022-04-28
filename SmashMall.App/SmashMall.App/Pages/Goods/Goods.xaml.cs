using System;
using System.Collections.Generic;
using System.Globalization;
using SmashMall.Models.Goods;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmashMall.App.Pages.Goods
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Goods : ContentView
    {
        public List<Good> AllGoods { get; set; }
        public List<Good> RandomGoods { get; set; }
        private List<Good> _goods = new();
        private List<Good> _dbGoods = new();
        private List<GoodCategory> _categories = new();

        public Goods()
        {
            InitializeComponent();
            GoodsView.Goods = AllGoods;
            GoodsView.Property = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            SearchCountTextBlock.Text = $"Search results: {AllGoods?.Count} items";

            ItemsYouMayLikeView.Goods = RandomGoods;
            ItemsYouMayLikeView.Property = DateTime.Now.ToString(CultureInfo.CurrentCulture);
        }
    }
}