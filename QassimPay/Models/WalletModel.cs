using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace QassimPay.Models
{
    public class WalletModel
    {

        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        public int User_ID { get; set; }
        public UserModel User { get; set; }
        public ICollection<TransferModel> Transfers { get; set; } = new List<TransferModel>();
        public ICollection<BillingModel> Billings { get; set; } = new List<BillingModel>();

    }
}
