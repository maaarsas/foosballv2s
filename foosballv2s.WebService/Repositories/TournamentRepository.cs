using foosballv2s.WebService.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;

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
                .Include(t => t.Pairs)
                    .ThenInclude(tg => tg.Team1)
                .Include(t => t.Pairs)
                    .ThenInclude(tg => tg.Team2)
                .Include(t => t.Pairs)
                    .ThenInclude(tg => tg.Game)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Gets a tournament by a id from the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tournament Get(int id)
        {
            return _context.Tournaments
                .Include(t => t.Pairs)
                    .ThenInclude(tg => tg.Team1)
                .Include(t => t.Pairs)
                    .ThenInclude(tg => tg.Team2)
                .Include(t => t.Pairs)
                    .ThenInclude(tg => tg.Game)
                .AsNoTracking()
                .SingleOrDefault(g => g.Id == id);
        }

        /// <summary>
        /// Creates a tournament in the storage
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
                Console.WriteLine("xDDDDD");
                return null;
            }
            return tournament;
        }

        /// <summary>
        /// Deletes a tournament
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
        /// Updates a tournament by an id in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Update(int id, Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException("tournament");
            }
            Tournament tournamentToUpdate = Get(id);
            if (tournamentToUpdate == null)
            {
                return false;
            }
            /*var users = _context.Users.AsNoTracking();
            game.Team1.User = null;
            game.Team2.User = null;

            gameToUpdate.Team1 = game.Team1;
            gameToUpdate.Team2 = game.Team2;
            gameToUpdate.Team1Score = game.Team1Score;
            gameToUpdate.Team2Score = game.Team2Score;
            gameToUpdate.StartTime = game.StartTime;
            gameToUpdate.EndTime = game.EndTime;*/

            _context.Tournaments.Update(tournament);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Adds a pair to tournament in the storage
        /// </summary>
        /// <param name="tournamentPair"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TournamentPair AddPair(int tournamentId, TournamentPair tournamentPair)
        {
            if (tournamentPair == null)
            {
                throw new ArgumentNullException("tournamentPair");
            }

            Console.WriteLine("-------" + tournamentId);
            Tournament tournament = Get(tournamentId);
            tournament.Pairs.Add(tournamentPair);
            _context.Tournaments.Update(tournament);
            try
            {
                _context.SaveChanges();
            }
            catch (SqlException e) // happens when, for example, non existing tournaments are provided
            {
                Console.WriteLine("xDDDDD");
                return null;
            }
            return tournamentPair;
        }
    }
}
