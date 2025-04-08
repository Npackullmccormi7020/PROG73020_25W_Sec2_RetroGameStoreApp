namespace GameStoreApp.Entities
{
    public class Games
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Genre { get; set; }
        public string? Platform { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
