
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
    public class GameRepository : IGameRepository
    {
        private readonly IWebServiceDbContext _context;

        public GameRepository(IWebServiceDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Gets all game from the storage
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Team1)
                .Include(g => g.Team2)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Gets a game by a id from the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Game Get(int id)
        {
            return _context.Games.Find(id);
        }

        /// <summary>
        /// Creates a game in the storage
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Game Add(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }
            _context.Teams.Attach(game.Team1);
            _context.Teams.Attach(game.Team2);
            _context.Games.Add(game);
            try
            {
                _context.SaveChanges();
            }
            catch (SqlException e) // happens when, for example, non existing teams are provided
            {
                return null;
            }
            return game;
        }

        /// <summary>
        /// Deletes a game
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Remove(int id)
        {
            Game game = Get(id);
            if (game == null)
            {
                return false;
            }
            _context.Games.Remove(game);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Updates a game by an id in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Update(int id, Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }
            Game gameToUpdate = Get(id);
            
            if (gameToUpdate == null)
            {
                return false;
            }
            
            gameToUpdate.Team1 = game.Team1;
            gameToUpdate.Team2 = game.Team2;
            gameToUpdate.Team1Score = game.Team1Score;
            gameToUpdate.Team2Score = game.Team2Score;
            gameToUpdate.StartTime = game.StartTime;
            gameToUpdate.EndTime = game.EndTime;
            
            _context.Games.Update(gameToUpdate);
            _context.SaveChanges();
            return true;
        }
    }
}