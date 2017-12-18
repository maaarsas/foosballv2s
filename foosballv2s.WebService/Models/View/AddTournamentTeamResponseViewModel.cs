using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Models.View
{
    public class AddTournamentTeamResponseViewModel
    {
        public bool IsEnoughTeamsToStartTournament { get; set; }
        public int TeamsCount { get; set; }
        public TournamentTeam TournamentTeam { get; set; }
        
        public AddTournamentTeamResponseViewModel(bool isEnoughTeamsToStartTournament,
                                                    int teamsCount,
                                                    TournamentTeam tournamentTeam)
        {
            IsEnoughTeamsToStartTournament = isEnoughTeamsToStartTournament;
            TeamsCount = teamsCount;
            TournamentTeam = tournamentTeam;
        }
    }
}
