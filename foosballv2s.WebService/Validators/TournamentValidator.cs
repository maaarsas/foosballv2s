using foosballv2s.WebService.Models;
using System;

namespace foosballv2s.WebService.Validators
{
    public class TournamentValidator : AbstractValidator
    {
        private Tournament tournament;

        public TournamentValidator(Tournament tournament)
        {
            this.tournament = tournament;
            RegisterValidationFunc(ValidateNumberOfTeams);
        }

        /// <summary>
        /// Validates number of teams
        /// </summary>
        /// <returns></returns>
        private bool ValidateNumberOfTeams()
        {
            if ((Math.Log(tournament.NumberOfTeams, 2.0) % 1) != 0)
            {
                return false;
            }
            return true;
        }
    }
}
