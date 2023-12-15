using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateFormat Date_created { get; set; }
    }
}
