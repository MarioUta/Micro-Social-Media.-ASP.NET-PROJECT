using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set;}
        [Required]
        public string Phone { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime BirthDay{ get; set; }
        [Required]
        public DateTime Date_created { get; set; }

    }
}
