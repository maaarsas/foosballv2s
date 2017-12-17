using foosballv2s.WebService.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Models
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly IWebServiceDbContext _context;

        public TournamentRepository(IWebServiceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all tournaments from the storage
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tournament> GetAll()
        {
            return _context.Tournaments
                .Include(t => t.TournamentGames)
                    .ThenInclude(tg => tg.Game)
                .AsNoTracking().ToList();
        }

        /// <summary>
        /// Gets a game by a id from the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tournament Get(int id)
        {
            return _context.Tournaments
                .Include(t => t.TournamentGames)
                    .ThenInclude(tg => tg.Game)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);
        }

        /// <summary>
        /// Creates a game in the storage
        /// </summary>
        /// <param name="tournament"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Tournament Add(Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException("tournament");
            }

            IValidator validator = new TournamentValidator(tournament);
            if (!validator.Validate())
            {
                return null;
            }
            _context.Tournaments.Add(tournament);
            try
            {
                _context.SaveChanges();
            }
            catch (SqlException e) // happens when, for example, non existing tournaments are provided
            {
                return null;
            }
            return tournament;
        }

    }
}
