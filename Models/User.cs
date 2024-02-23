using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class User : IdentityUser
    {
        public bool? Public { get; set; } = false;
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDay{ get; set; }
        public DateTime? Date_created { get; set; } = DateTime.Now;
        public string? Link { get; set; }
    }
}
