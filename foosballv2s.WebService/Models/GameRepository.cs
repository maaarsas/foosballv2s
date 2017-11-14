
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace foosballv2s.WebService.Models
{
    public class GameRepository : IGameRepository
    {
        private readonly IWebServiceDbContext _context;

        public GameRepository(IWebServiceDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Game> GetAll()
        {
            return _context.Games.ToList();
        }

        public Game Get(int id)
        {
            return _context.Games.Find(id);
        }

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