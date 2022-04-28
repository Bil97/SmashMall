using SmashMall.Models.Goods;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SmashMall.UWP.Pages.Goods
{
    public sealed partial class Goods : UserControl
    {
        public List<Good> AllGoods { get; set; }
        public List<Good> RandomGoods { get; set; }
        public Frame Frame { get; set; }
        private List<Good> goods = new List<Good>();
        private List<Good> dbGoods = new List<Good>();
        private List<GoodCategory> categories = new List<GoodCategory>();

        public Goods()
        {
            this.InitializeComponent();
            this.Loaded += Goods_Loaded;
        }

        private void Goods_Loaded(object sender, RoutedEventArgs e)
        {
            GoodsView.Goods = AllGoods;
            GoodsView.Property = DateTime.Now.ToString();
            GoodsView.Frame = Frame;

            searchCountTextBlock.Text = $"Search results: {AllGoods?.Count} items";

            ItemsYouMayLikeView.Goods = RandomGoods;
            ItemsYouMayLikeView.Property = DateTime.Now.ToString();
            ItemsYouMayLikeView.Frame = Frame;
        }
    }
}
