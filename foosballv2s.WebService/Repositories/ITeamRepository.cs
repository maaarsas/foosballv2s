using System.Collections.Generic;
using foosballv2s.WebService.Params;

namespace foosballv2s.WebService.Models
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetAll(TeamParams teamParams, User user);
        Team Get(int id);
        Team Add(Team team);
        bool Remove(int id);
        bool Update(int id, Team team);
    }
}