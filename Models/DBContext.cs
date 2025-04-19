using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace A23017_Cloud.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("conn") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<ScoreBoard> Records { get; set; }
        public DbSet<WoD> WoDs { get; set; }
        

    }
}