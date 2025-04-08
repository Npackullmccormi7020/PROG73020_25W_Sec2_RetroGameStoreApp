namespace GameStoreData.Entities
{
    //represents a user in the system
    public class User
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        //optional address info
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }

        //a user can have many rewards
        public virtual ICollection<Rewards> Rewards { get; set; } = new List<Rewards>();

        //a user can place many orders
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}