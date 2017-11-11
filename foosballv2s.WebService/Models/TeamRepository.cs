
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace foosballv2s.WebService.Models
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IWebServiceDbContext _context;

        public TeamRepository(IWebServiceDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Team> GetAll()
        {
            return _context.Teams.ToList();
        }

        public Team Get(int id)
        {
            var team = _context.Teams.Find(id);
            return team;
        }

        public Team Add(Team team)
        {
            if (team == null)
            {
                throw new ArgumentNullException("team");
            }
            _context.Teams.Add(team);
            _context.SaveChanges();
            return team;
        }

        public bool Remove(int id)
        {
            Team team = Get(id);
            if (team == null)
            {
                return false;
            }
            _context.Teams.Remove(team);
            _context.SaveChanges();
            return true;
        }

        public bool Update(int id, Team team)
        {
            if (team == null)
            {
                throw new ArgumentNullException("team");
            }
            Team teamToUpdate = Get(id);

            if (teamToUpdate == null)
            {
                return false;
            }
            
            teamToUpdate.TeamName = team.TeamName;
            
            _context.Teams.Update(teamToUpdate);
            _context.SaveChanges();
            return true;
        }
    }
}