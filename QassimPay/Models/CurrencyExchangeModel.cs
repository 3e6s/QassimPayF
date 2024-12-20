namespace QassimPay.Models
{
    public class CurrencyExchangeModel
    {
        public int ID { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
