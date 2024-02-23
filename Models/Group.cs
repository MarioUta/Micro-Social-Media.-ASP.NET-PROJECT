using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }

        public virtual ICollection<Message>? Messages { get; set; }
    }
}
