using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set;}
        public string Phone { get; set; }
        public string PhoneNumber { get; set; }
        public DateFormat BirthDay{ get; set; }
        public DateFormat Date_created { get; set; }

    }
}
