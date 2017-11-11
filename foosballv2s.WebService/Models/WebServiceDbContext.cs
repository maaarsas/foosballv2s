using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Models
{
    public class WebServiceDbContext : DbContext, IWebServiceDbContext
    {
        public WebServiceDbContext(DbContextOptions<WebServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Game>()
                .HasIndex(g => g.Id).IsUnique(true);
            
            builder
                .Entity<Team>()
                .HasIndex(t => t.Id).IsUnique(true);
        }

    }
}