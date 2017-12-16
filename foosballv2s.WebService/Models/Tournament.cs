using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        public int CurrentStage { get; set; } = 1;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<TournamentGame> TournamentGames { get; set; }

        public int NumberOfStages { get; set; }
        public int NumberOfPairs { get; set; }

        public int CalculateNumberOfStages()
        {
            return (int)Math.Log(NumberOfPairs, 2) + 1;
        }
    }
}

