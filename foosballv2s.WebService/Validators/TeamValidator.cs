using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using foosballv2s.WebService.Models;

namespace foosballv2s.WebService.Validators
{
    class TeamValidator : IValidator
    {
        public string NamePattern { get; } = @"^[0\w][A-Za-z\d]{2,19}$";
        private Team team;
        private bool errorsExist = false;

        public TeamValidator(Team team)
        {
            this.team = team;
        }

        public bool Validate()
        {
            errorsExist = false;
            
            ValidateName();

            if (errorsExist)
            {
                return false;
            }
            return true;
        }

        private bool ValidateName()
        {
            if (!Regex.IsMatch(team.TeamName, NamePattern))
            {
                errorsExist = true;
            }
        }
    }
}