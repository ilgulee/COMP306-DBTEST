using System.ComponentModel.DataAnnotations;

namespace COMP306_DBTEST.Models
{
    public class ShippingInfo
    {
        
        [Required(ErrorMessage = "You must enter a shipping address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "You must enter a shipping postal code")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "You must choose delivery or take-out yourself")]
        public bool IsDelivery { get; set; }
         

    }
}