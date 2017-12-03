using System;

namespace foosballv2s.Source.Entities
{
    public class Team
    {
        public int Id { get; set; }
        
        public string TeamName { get; set; }
        
        public User User { get; set; }
        
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
