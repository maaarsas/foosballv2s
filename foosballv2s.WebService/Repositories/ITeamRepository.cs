using System.Collections.Generic;

namespace foosballv2s.WebService.Models
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetAll();
        Team Get(int id);
        Team Add(Team team);
        bool Remove(int id);
        bool Update(int id, Team team);
    }
}