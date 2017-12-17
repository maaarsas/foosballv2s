using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Models
{
    public interface IWebServiceDbContext
    {
        DbSet<Game> Games { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<GameEvent> GameEvents { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Tournament> Tournaments { get; set; }
        DbSet<TournamentPair> TournamentGames { get; set; }
        int SaveChanges();
        //void MarkAsModified(Team item); 
    }
}