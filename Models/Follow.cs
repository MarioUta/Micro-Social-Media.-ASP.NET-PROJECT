using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mello.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }
        public virtual User Follower { get; set; }
        public virtual User Following { get; set; }
    }
}