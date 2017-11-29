using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Models
{
    /// <summary>
    /// A class for getting/sending data to the database
    /// </summary>
    public class WebServiceDbContext : DbContext, IWebServiceDbContext
    {
        public WebServiceDbContext(DbContextOptions<WebServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Game>()
                .HasIndex(g => g.Id).IsUnique(true);

            builder
                .Entity<Game>()
                .HasMany(c => c.GameEvents)
                .WithOne(e => e.Game);
            
            builder
                .Entity<Team>()
                .HasIndex(t => t.Id).IsUnique(true);
            
            builder
                .Entity<GameEvent>()
                .HasIndex(t => t.Id).IsUnique(true);
        }

    }
}