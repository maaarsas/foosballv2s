
using foosballv2s.WebService.Models.View;
using System.Collections.Generic;

namespace foosballv2s.WebService.Models
{
    public interface ITournamentRepository
    {
        IEnumerable<Tournament> GetAll();
        Tournament Get(int id);
        Tournament Add(Tournament tournament);
        bool Remove(int id);
        bool Update(int id, Tournament tournament);

        AddTournamentPairResponseViewModel AddPair(int tournamentId, TournamentPair tournamentPair);
        bool RemovePair(int pairId);
        TournamentPair GetPair(int pairId);

        AddTournamentTeamResponseViewModel AddTeam(int teamId, TournamentTeam tournamentTeam);
        bool RemoveTeam(int teamId);
        TournamentTeam GetTeam(int teamId);
    }
}
