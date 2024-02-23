using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        [Required]
        public int? PostId { get; set; }
        public virtual Post? Post { get; set; }
    }
}
