using System.ComponentModel.DataAnnotations;

namespace COMP306_DBTEST.Models
{
    public class BillingInfo
    {
        
        [Required(ErrorMessage = "You must enter a billing address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "You must enter a billing postal code")]
        public string PostalCode { get; set; }
    }
}