using System;
using System.Collections.Generic;

namespace foosballv2s.WebService.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        public int CurrentStage { get; set; } = 1;

        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; }

        private int numberOfTeams;
        public int NumberOfTeams
        {
            get
            {
                return numberOfTeams;
            }
            set
            {
                numberOfTeams = value;
                NumberOfStages = CalculateNumberOfStages(value);
            }
        }
        public int NumberOfStages { get; set; }

        public ICollection<TournamentPair> Pairs { get; set; } = new List<TournamentPair>();

        public static int CalculateNumberOfStages(int numberOfPairs)
        {
            return (int)Math.Log(numberOfPairs, 2) + 1;
        }
    }
}

