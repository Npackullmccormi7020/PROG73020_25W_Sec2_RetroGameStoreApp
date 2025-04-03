namespace GameStoreApp.Entities
{
    public class User
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public virtual ICollection<ShippingInfo> ShippingInfos { get; set; } = new List<ShippingInfo>();
        public virtual ICollection<Rewards> Rewards { get; set; } = new List<Rewards>();
        public int UserId { get; set; }
    }
}
