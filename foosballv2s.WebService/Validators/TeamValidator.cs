using System.Text.RegularExpressions;
using foosballv2s.WebService.Models;

namespace foosballv2s.WebService.Validators
{
    /// <summary>
    /// A team validator with defined constraints
    /// </summary>
    public class TeamValidator : AbstractValidator
    {
        public string NamePattern { get; } = @"^[0\w][A-Za-z\d]{2,19}$";
        private Team team;

        public TeamValidator(Team team)
        {
            this.team = team;
            RegisterValidationFunc(ValidateName);
        }

        /// <summary>
        /// Validates a team name
        /// </summary>
        /// <returns></returns>
        private bool ValidateName()
        {
            if (!Regex.IsMatch(team.TeamName, NamePattern))
            {
                return false;
            }
            return true;
        }
    }
}