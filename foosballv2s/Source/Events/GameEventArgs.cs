using System;
using foosballv2s.Source.Entities;

namespace foosballv2s.Source.Activities.Events
{
    public class GameEventArgs : EventArgs
    {
        public Game EventGame { get; set; }
        public Team EventTeam { get; set; }
        
        public GameEventArgs(Game game, Team team = null)
        {
            EventGame = game;
            EventTeam = team;
        }
    }
}