using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QassimPay.Models
{
    public class AddressModel
    {
        [Key]
        public int ADD_ID { get; set; } // Primary key

        [Required]
        public string Street_Adress { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string Postal_Code { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        // Foreign Key for UserModel
        [ForeignKey("UserModel")]
        public int U_ID { get; set; }

        // Navigation property to UserModel
        public UserModel User { get; set; }
    }
}
