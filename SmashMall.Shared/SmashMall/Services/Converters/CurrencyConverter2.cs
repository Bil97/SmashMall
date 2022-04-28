using System.Globalization;

namespace SmashMall.Services.Converters
{
    public class CurrencyConverter2
    {
        private decimal Amount { get; set; }
        private decimal ConvertedAmount { get; set; }
        private readonly string _culture = BaseApi.Culture;

        public CurrencyConverter2(decimal amount = 0)
        {
            Amount = amount;
        }

        public string Currency
        {
            get
            {
                Convert();
                return ConvertedAmount.ToString("C2", CultureInfo.CreateSpecificCulture(_culture));
            }
        }

        private void Convert()
        {
            // Do the conversion here and assign the value to ConvertedAmount
            ConvertedAmount = Amount;
        }
    }
}
