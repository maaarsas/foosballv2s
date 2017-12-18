using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace foosballv2s.WebService.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        public int CurrentStage { get; set; } = 1;

        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; }

        private int numberOfTeamsRequired;
        public int NumberOfTeamsRequired
        {
            get
            {
                return numberOfTeamsRequired;
            }
            set
            {
                numberOfTeamsRequired = value;
                NumberOfStages = CalculateNumberOfStages(value);
            }
        }
        public int NumberOfStages { get; set; }

        public bool IsEnoughTeams {
            get
            {
                return CheckIfEnoughTeamsJoined(NumberOfTeamsRequired, Pairs.Count);
            }
        } 

        public ICollection<TournamentPair> Pairs { get; set; } = new List<TournamentPair>();



        public static int CalculateNumberOfStages(int numberOfPairs)
        {
            return (int)Math.Log(numberOfPairs, 2) + 1;
        }

        public static bool CheckIfEnoughTeamsJoined(int numberOfTeamsRequired, int actualNumberOfPairs)
        {
            return (2 * actualNumberOfPairs) == numberOfTeamsRequired;
        }
    }
}

