using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Models.View
{
    public class AddTournamentPairResponseViewModel
    {
        public bool IsEnoughTeamsToStartTournament { get; set; }
        public int PairsCount { get; set; }
        public TournamentPair TournamentPair { get; set; }
        //public ICollection<TournamentPair> Pairs { get; set; } = new List<TournamentPair>();

        public AddTournamentPairResponseViewModel(bool isEnoughTeamsToStartTournament,
                                                    int pairsCount,
                                                    TournamentPair tournamentPair)
        {
            IsEnoughTeamsToStartTournament = isEnoughTeamsToStartTournament;
            PairsCount = pairsCount;
            TournamentPair = tournamentPair;
        }
    }
}
