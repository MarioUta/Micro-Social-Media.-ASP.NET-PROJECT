using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        public int? Follower { get; set; }
        public virtual User? UserFollower { get; set; }
        
        public int? Followee { get; set; }
        public virtual User? UserFollowee { get; set; }

        [Required]
        public bool IsFollowing { get; set; }
    }
}
