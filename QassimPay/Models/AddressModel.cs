using System.ComponentModel.DataAnnotations;

namespace QassimPay.Models
{
    public class AddressModel
    {
        public int ID { get; set; }
        public string Street_Adress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Postal_Code { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public int U_ID { get; set; }
        public UserModel User { get; set; }
    }
}
