using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Models
{
    public class TeamContext : DbContext
    {
        public TeamContext(DbContextOptions<GameContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }

    }
}