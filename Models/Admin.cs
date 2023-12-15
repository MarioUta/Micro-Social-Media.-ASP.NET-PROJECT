﻿using System.ComponentModel.DataAnnotations;

namespace Mello.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        public int? GroupId { get; set; }
        public virtual Group? Group { get; set; }
    }
}
