using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace A23017_Cloud.Models
{
    public class Player : User
    {
        [DefaultValue(0)]
        public int Point { get; set; }

        public ICollection<ScoreBoard> Records { get; set; }
    }
}