using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class Message
    {

        [Key]
        public int Id { get; set; }

        public string Content {  get; set; }

        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        public int? GroupId { get; set; }
        public virtual Group? Group { get; set; }
    }
}
