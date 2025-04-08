using System.ComponentModel.DataAnnotations;

namespace GameStoreData.Entities
{
    //this represents the points a user can earn through purchases or activity
    public class Rewards
    {
        [Key]
        public int RewardId { get; set; }

        //foreign key to link this reward to a specific user
        public int UserId { get; set; }

        //number of reward points
        public int Points { get; set; }

        //navigation property (a reward belongs to ONE user)
        public virtual User? User { get; set; }
    }
}