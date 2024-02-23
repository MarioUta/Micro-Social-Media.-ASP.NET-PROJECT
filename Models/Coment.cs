using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class Coment
    {

        [Key]
        public int Id { get; set; }

        public string Content { get; set; }

        public String? UserId { get; set; }
        public virtual User? User { get; set; }

        public int? PostId { get; set; }
        public virtual Post? Post { get; set; }
    }
}
