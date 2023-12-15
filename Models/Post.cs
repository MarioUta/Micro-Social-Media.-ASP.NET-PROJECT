using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime Date_created { get; set; }
    }
}
