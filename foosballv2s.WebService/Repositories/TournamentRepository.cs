using System;
using System.Data.SqlClient;

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
