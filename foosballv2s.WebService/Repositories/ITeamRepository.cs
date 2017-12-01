using System.Collections.Generic;
using foosballv2s.WebService.Params;

namespace foosballv2s.WebService.Models
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetAll(TeamParams teamParams, SortParams sortParams, User user);
        Team Get(int id);
        Team Add(Team team);
        bool Remove(int id);
        bool Update(int id, Team team);
    }
}