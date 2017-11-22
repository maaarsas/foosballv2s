using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Models
{
    /// <summary>
    /// A class for CRUD game data 
    /// </summary>
    public class TournamentRepository : ITournamentRepository
    {
        private readonly IWebServiceDbContext _context;

        public TournamentRepository(IWebServiceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all Tournaments from the storage
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tournament> GetAll()
        {
            return _context.Tournaments
                .Include(t => t.Stages)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Gets a Tournament by a id from the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tournament Get(int id)
        {
            return _context.Tournaments.Find(id);
        }

        /// <summary>
        /// Creates a Tournament in the storage
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Tournament Add(Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException("Tournament");
            }
            _context.Stages.AddRange(tournament.Stages);
            _context.Tournaments.Add(tournament);
            try
            {
                _context.SaveChanges();
            }
            catch (SqlException e) // happens when, for example, non existing teams are provided
            {
                return null;
            }
            return tournament;
        }

        /// <summary>
        /// Deletes a Tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Remove(int id)
        {
            Tournament tournament = Get(id);
            if (tournament == null)
            {
                return false;
            }
            _context.Tournaments.Remove(tournament);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Updates a Tournament by an id in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Update(int id, Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException("Tournament");
            }
            Tournament tournamentToUpdate = Get(id);

            if (tournamentToUpdate == null)
            {
                return false;
            }

            tournamentToUpdate.Name = tournament.Name;
            tournamentToUpdate.StartTime = tournament.StartTime;
            tournamentToUpdate.EndTime = tournament.EndTime;
            tournamentToUpdate.Stages = tournament.Stages;

            _context.Tournaments.Update(tournamentToUpdate);
            _context.SaveChanges();
            return true;
        }
    }
}
