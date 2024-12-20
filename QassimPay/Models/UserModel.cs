using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Collections.Generic;

namespace QassimPay.Models
{


    public class UserModel
    {
        public int ID { get; set; }
        public string First_name { get; set; } = string.Empty;
        public string Last_name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public decimal Monthly_income { get; set; }

        public ICollection<WalletModel> Wallets { get; set; } = new List<WalletModel>();
        public ICollection<AddressModel> Adresses { get; set; } = new List<AddressModel>();


    }
}
