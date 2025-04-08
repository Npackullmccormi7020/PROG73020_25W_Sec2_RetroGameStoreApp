namespace GameStoreApp.Entities
{
    public class Order
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public bool IsShipped { get; set; }
        public virtual ICollection<Games> Games { get; set; } = new List<Games>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public int OrderId { get; internal set; }
        public int UserId { get; internal set; }
        public int GameId { get; internal set; }
        public int Quantity { get; internal set; }
    }
}
