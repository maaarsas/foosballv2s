using Microsoft.EntityFrameworkCore;
using foosballv2s.WebService.Models;

namespace foosballv2s.WebService.Test
{                
    public class TestWebServiceDbContext : IWebServiceDbContext 
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }
        public DbSet<User> Users { get; set; }

        public TestWebServiceDbContext()
        {
            this.Teams = new TestTeamDbSet();
            this.Games = new TestGameDbSet();
        }

        public int SaveChanges()
        {
            return 0;
        }
        public void Dispose() { }
    }
}