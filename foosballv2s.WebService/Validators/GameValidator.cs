using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using foosballv2s.WebService.Models;

namespace foosballv2s.WebService.Validators
{
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

        private bool ValidateTeams()
        {
            
            if (game.Team1.Id == null || game.Team2.Id == null || game.Team1.Id == game.Team2.Id)
            {
                return false;
            }
            return true;
        }

        private bool ValidateScore()
        {
            var score1 = game.Team1Score;
            var score2 = game.Team2Score;
            
            if (score1 < 0 || score2 > Game.MAX_SCORE || score2 < 0 || score2 > Game.MAX_SCORE
                || (score1 == Game.MAX_SCORE && score2 == Game.MAX_SCORE))
            {
                return false;
            }
            return true;
        }

        private bool ValidateTime()
        {
            var startTime = game.StartTime;
            var endTime = game.EndTime;
            var currentTime = new DateTime();

            if (startTime > endTime || startTime > currentTime || endTime > currentTime)
            {
                return false;
            }
            return true;
        }
    }
}