using System.ComponentModel.DataAnnotations;

namespace QassimPay.Models
{
    public class BillingModel
    {
        [Key] public int Billing_ID { get; set; }
        public DateTime Date { get; set; }
        public string Billing_number { get; set; }
        public decimal Amount { get; set; }

        public int W_ID { get; set; }
        public WalletModel Wallet { get; set; }
    }
}
