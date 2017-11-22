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
        public DbSet<TournamentStage> Stages { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Game>()
                .HasIndex(g => g.Id).IsUnique(true);
            
            builder
                .Entity<Team>()
                .HasIndex(t => t.Id).IsUnique(true);

            builder
                .Entity<TournamentStage>()
                .HasIndex(s => s.Id).IsUnique(true);

            builder
                .Entity<Tournament>()
                .HasIndex(t => t.Id).IsUnique(true);
        }

    }
}