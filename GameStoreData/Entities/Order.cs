namespace GameStoreData.Entities
{
    public class Order
    {
        public int OrderId { get; set; }

        //foreign keys
        public int UserId { get; set; }
        public int GameId { get; set; }

        //basic order info
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        //optional extra fields (still fine to keep)
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public bool IsShipped { get; set; }

        //navigation (single User/Game, not a collection)
        public virtual User? User { get; set; }
        public virtual Games? Game { get; set; }
    }
}