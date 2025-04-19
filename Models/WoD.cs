using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace A23017_Cloud.Models
{
    public class WoD
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Word must be 5 characters")]
        public string Word { get; set; }

        [RegularExpression(@"^/.*/$", ErrorMessage = "Phonetic should start and end with '/'")]
        [Required]
        public string Phonetic { get; set; }

        [Required]
        public string Definition { get; set; }
    }
}