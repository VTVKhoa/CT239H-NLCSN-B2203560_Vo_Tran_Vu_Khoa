using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace A23017_Cloud.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Avatar { get; set; } = "blank.jpg";

    }
}