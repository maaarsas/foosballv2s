﻿using System;

namespace foosballv2s.WebService.Models
{
    public class Game
    {
        public const int MAX_SCORE = 7;

        public int id;
        
        private int team1Score = 0;
        private int team2Score = 0;

        public int Team1Id { get; set; }
        
        public Team Team1 { get; set; } = new Team();
        
        public int Team2Id { get; set; }
        
        public Team Team2 { get; set; } = new Team();
        
        public int Team1Score
        {
            get { return team1Score; }
            set
            {
                if (HasEnded)
                {
                    return;
                }
                team1Score = value;
                CheckGameEnd();
            }
        }

        public int Team2Score
        {
            get { return team2Score; }
            set
            {
                if (HasEnded)
                {
                    return;
                }
                team2Score = value;
                CheckGameEnd();
            }
        }

        public Boolean HasEnded { get; private set; } = false;

        private void CheckGameEnd()
        {
            if (Team1Score == MAX_SCORE || Team2Score == MAX_SCORE)
            {
                HasEnded = true;
            }
        }
    }
}