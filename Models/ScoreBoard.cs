using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace A23017_Cloud.Models
{
    public class ScoreBoard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int Score { get; set; }

        public string Username { get; set; }
        
        [ForeignKey("Username")]
        public User User { get; set; }
    }
}