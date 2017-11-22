using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Models
{
    public interface ITournamentRepository
    {
        IEnumerable<Tournament> GetAll();
        Tournament Get(int id);
        Tournament Add(Tournament tournament);
        bool Remove(int id);
        bool Update(int id, Tournament tournament);
    }
}
