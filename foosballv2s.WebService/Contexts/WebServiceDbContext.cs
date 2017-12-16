using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Models
{
    /// <summary>
    /// A class for getting/sending data to the database
    /// </summary>
    public class WebServiceDbContext : IdentityDbContext<User>, IWebServiceDbContext
    {
        public WebServiceDbContext(DbContextOptions<WebServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentGame> TournamentGames { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder
                .Entity<Game>()
                .HasIndex(g => g.Id).IsUnique(true);

            builder
                .Entity<Game>()
                .HasMany(c => c.GameEvents)
                .WithOne(e => e.Game);
            
            builder
                .Entity<Game>()
                .HasOne(c => c.User);
            
            builder
                .Entity<Team>()
                .HasIndex(t => t.Id).IsUnique(true);
            
            builder
                .Entity<Team>()
                .HasOne(c => c.User);
            
            builder
                .Entity<GameEvent>()
                .HasIndex(t => t.Id).IsUnique(true);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}