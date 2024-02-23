using Mello.Models;
using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public virtual User Target { get; set; }
        public virtual User Requester { get; set; }
    }
}
