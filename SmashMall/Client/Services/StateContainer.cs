using SmashMall.Shared.Services;
using System;

namespace SmashMall.Client.Services
{
    public class StateContainer
    {
        public int GoodsCount { get; set; } = 0;
        public string TotalPrice { get; set; } = "0";
        private CurrencyConverter currency;

        public event Action OnChange;

        public void SetCount(int value)
        {
            GoodsCount = value;
            NotifyStateChanged();
        }

        public void SetPrice(decimal value)
        {
            currency = new CurrencyConverter(value, BaseApi.Culture);

            TotalPrice = currency.Currency;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
