using System;
using foosballv2s.WebService.Models;

namespace foosballv2s.WebService.Validators
{
    /// <summary>
    /// A game validator with defined constraints
    /// </summary>
    public class GameValidator : AbstractValidator
    {
        private Game game;

        public GameValidator(Game game)
        {
            this.game = game;
            RegisterValidationFunc(ValidateTeams);
            RegisterValidationFunc(ValidateScore);
            RegisterValidationFunc(ValidateTime);
        }

        /// <summary>
        /// Validates team names
        /// </summary>
        /// <returns></returns>
        private bool ValidateTeams()
        {
            
            if (game.Team1.Id == null || game.Team2.Id == null || game.Team1.Id == game.Team2.Id)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates game score
        /// </summary>
        /// <returns></returns>
        private bool ValidateScore()
        {
            var score1 = game.Team1Score;
            var score2 = game.Team2Score;
            
            if (score1 < 0 || score2 > Game.MAX_SCORE || score2 < 0 || score2 > Game.MAX_SCORE
                || (score1 == Game.MAX_SCORE && score2 == Game.MAX_SCORE)
                || (score1 != Game.MAX_SCORE && score2 != Game.MAX_SCORE))
            {
                return false;
            }
            return true;
        }

        
        /// <summary>
        /// Validates game time
        /// </summary>
        /// <returns></returns>
        private bool ValidateTime()
        {
            var startTime = game.StartTime;
            var endTime = game.EndTime;
            var currentTime = DateTime.Now;

            if (startTime > endTime || startTime > currentTime || endTime > currentTime)
            {
                return false;
            }
            return true;
        }
    }
}