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

        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = null;

        private int numberOfPairs;
        public int NumberOfPairs
        {
            get
            {
                return numberOfPairs;
            }
            set
            {
                if (isValidNumberOfPairs(value))
                {
                    numberOfPairs = value;
                    NumberOfStages = CalculateNumberOfStages(value);
                }
            }
        }
        public int NumberOfStages { get; set; }

        public ICollection<TournamentGame> TournamentGames { get; set; }

        public static int CalculateNumberOfStages(int numberOfPairs)
        {
            return (int)Math.Log(numberOfPairs, 2) + 1;
        }

        private bool isValidNumberOfPairs(int numberOfPairs)
        {
            //Is a whole number?
            return (Math.Log(numberOfPairs, 2.0) % 1) == 0;
        }
    }
}

