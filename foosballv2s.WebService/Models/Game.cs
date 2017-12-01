using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace foosballv2s.WebService.Models
{
    public class Game
    {
        public const int MAX_SCORE = 7;

        public int Id { get; set; }
        
        public Team Team1 { get; set; } = new Team();
         
        public Team Team2 { get; set; } = new Team();
        
        public int Team1Score { get; set; }

        public int Team2Score { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public ICollection<GameEvent> GameEvents { get; set; }
        
        public User User { get; set; }
    }
}
