using System;

namespace foosballv2s.Droid.Shared.Source.Entities
{
    public class Team
    {
        public int id;
        
        public string TeamName { get; set; }
        
        private int gamesPlayed = 0;
        private int gamesWon = 0;

        public int Percentage { get; private set; }

        public int GamesPlayed
        {
            get { return gamesPlayed; }
            set
            {
                gamesPlayed = value;
                countPercentage();
            }
        }

        public int GamesWon
        {
            get { return gamesWon; }
            set
            {
                gamesWon = value;
                countPercentage();
            }
        }

        private void countPercentage()
        {
            if (gamesPlayed != 0)
                Percentage = (int)Math.Round((decimal)gamesWon / gamesPlayed * 100);
        }
    }
}
