using System.ComponentModel.DataAnnotations;

namespace QassimPay.Models
{
    public class TransferModel
    {
        [Key] public int Receipt_ID { get; set; }
        public decimal AmountM { get; set; }
        public int Reciver { get; set; }
        public DateOnly T_date { get; set; } 
        public int Sender_ID { get; set; }
        public WalletModel Wallet { get; set; }

    }
}
