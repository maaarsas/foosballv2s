using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace foosballv2s
{
    public class Foosballv2sContext : DbContext
    {
//        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=mssql4.gear.host;Initial Catalog=foosballv2s;user id=foosballv2s;password=Oq3u-!XnZi2O;Trusted_Connection=True;MultipleActiveResultSets=True;Integrated Security=False;");
        }
        
    }
}