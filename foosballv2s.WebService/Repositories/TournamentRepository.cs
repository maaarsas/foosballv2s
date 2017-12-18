using foosballv2s.WebService.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using foosballv2s.WebService.Models.View;

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
        public AddTournamentPairResponseViewModel AddPair(int tournamentId, TournamentPair tournamentPair)
        {
            if (tournamentPair == null)
            {
                throw new ArgumentNullException("tournamentPair");
            }

            Tournament tournament = Get(tournamentId);
            if (!tournament.IsEnoughTeams)
            {
                tournament.Pairs.Add(tournamentPair);
                _context.Tournaments.Update(tournament);
            }
            else
            {
                return null;
            }
            try
            {
                _context.SaveChanges();
            }
            catch (SqlException e) // happens when, for example, non existing tournaments are provided
            {
                return null;
            }

            int pairsCount = tournament.Pairs.Count;
            bool isEnoughPairs = (2*pairsCount).Equals(tournament.NumberOfTeamsRequired);
            return new AddTournamentPairResponseViewModel(isEnoughPairs,
                                                        pairsCount,
                                                        tournamentPair);
        }

        /// <summary>
        /// Deletes a pair from tournament
        /// </summary>
        /// <param name="pairId"></param>
        /// <returns></returns>
        public bool RemovePair(int pairId)
        {
            TournamentPair tournamentPair = GetPair(pairId);
            if (tournamentPair == null)
            {
                return false;
            }
            _context.TournamentPairs.Remove(tournamentPair);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Gets a tournament pair by a id from the storage
        /// </summary>
        /// <param name="pairId"></param>
        /// <returns></returns>
        public TournamentPair GetPair(int pairId)
        {
            return _context.TournamentPairs
                .AsNoTracking()
                .SingleOrDefault(p => p.Id == pairId);
        }

        /// <summary>
        /// Adds a team to tournament, team is waiting for pair in the storage
        /// </summary>
        /// <param name="tournamentPair"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public AddTournamentTeamResponseViewModel AddTeam(int tournamentId, TournamentTeam tournamentTeam)
        {
            if (tournamentTeam == null)
            {
                throw new ArgumentNullException("tournamentTeam");
            }
            Tournament tournament = Get(tournamentId);
            if (!tournament.IsEnoughTeams)
            {
                tournament.Teams.Add(tournamentTeam);
                _context.Tournaments.Update(tournament);
            }
            else
            {
                return null;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (SqlException e) // happens when, for example, non existing tournaments are provided
            {
                return null;
            }

            int teamsCount = tournament.Teams.Count;
            bool isEnoughTeams = teamsCount.Equals(tournament.NumberOfTeamsRequired);
            return new AddTournamentTeamResponseViewModel(isEnoughTeams,
                                                        teamsCount,
                                                        tournamentTeam);
        }

        /// <summary>
        /// Deletes a team from tournament
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public bool RemoveTeam(int teamId)
        {
            TournamentTeam tournamentTeam = GetTeam(teamId);
            if (tournamentTeam == null)
            {
                return false;
            }
            _context.TournamentTeams.Remove(tournamentTeam);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Gets a team that would like to join tournament by a id from the storage
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public TournamentTeam GetTeam(int teamId)
        {
            return _context.TournamentTeams
                .Include(tt => tt.Team)
                .AsNoTracking()
                .SingleOrDefault(t => t.Id == teamId);
        }
    }
}
