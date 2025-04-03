using System.ComponentModel.DataAnnotations;
namespace GameStoreApp.Entities
{
    public class ShippingInfo
    {
        [Key]
        public int ShippingId { get; set; }
        public int OrderId { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public bool IsShipped { get; set; }
        public virtual ICollection<Games> Games { get; set; } = new List<Games>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public DateTime ShippedDate { get; set; }
    }
    
}
