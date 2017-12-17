
using System.Collections.Generic;

namespace foosballv2s.WebService.Models
{
    public interface ITournamentRepository
    {
        IEnumerable<Tournament> GetAll();
        Tournament Get(int id);
        Tournament Add(Tournament tournament);
        /*    Game Add(Game game);
            bool Remove(int id);
            bool Update(int id, Game game);*/
    }
}
