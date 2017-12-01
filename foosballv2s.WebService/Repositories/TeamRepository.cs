
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using foosballv2s.WebService.Params;
using foosballv2s.WebService.Validators;

namespace foosballv2s.WebService.Models
{
    /// <summary>
    /// A class for CRUD with team data
    /// </summary>
    public class TeamRepository : ITeamRepository
    {
        private readonly IWebServiceDbContext _context;

        public TeamRepository(IWebServiceDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Gets all teams from the storage
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Team> GetAll(TeamParams teamParams, SortParams sortParams, User user)
        {
            Microsoft.EntityFrameworkCore.DbSet<Team> teamSet = _context.Teams;
            teamSet = sortParams.ApplyTeamSortParams(teamSet);
            if (teamParams.UserId == user.Id)
            {
                return teamSet.Where(t => t.User.Id == user.Id).ToList();
            }
            else if (teamParams.UserId.Length == 0)
            {
                return teamSet.ToList();
            }
            return null;
        }

        /// <summary>
        /// Gets a team from the storage by an id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Team Get(int id)
        {
            var team = _context.Teams.Find(id);
            return team;
        }

        /// <summary>
        /// Creates a team in the storage
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Team Add(Team team)
        {
            if (team == null)
            {
                throw new ArgumentNullException("team");
            }

            IValidator validator = new TeamValidator(team);
            if (!validator.Validate())
            {
                return null;
            }
            _context.Teams.Add(team);
            _context.SaveChanges();
            return team;
        }

        /// <summary>
        /// Removes a team from the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates a team by an id in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Update(int id, Team team)
        {
            if (team == null)
            {
                throw new ArgumentNullException("team");
            }
            
            IValidator validator = new TeamValidator(team);
            if (!validator.Validate())
            {
                return false;
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