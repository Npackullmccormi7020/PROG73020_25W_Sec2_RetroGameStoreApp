using System.ComponentModel.DataAnnotations;

namespace GameStoreData.Entities
{
    //this tracks shipping info for a placed order
    public class ShippingInfo
    {
        [Key]
        public int ShippingId { get; set; }

        //foreign key to the order being shipped
        public int OrderId { get; set; }

        //full shipping address info (optional)
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }

        //status of the shipping
        public bool IsShipped { get; set; }

        public DateTime ShippedDate { get; set; }

        //navigation: this shipment belongs to ONE order
        public virtual Order? Order { get; set; }
    }
}