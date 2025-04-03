using System.ComponentModel.DataAnnotations;

namespace GameStoreApp.Entities
{
    public class Rewards
    {
        [Key]
        public int RewardId { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    
    }
}
