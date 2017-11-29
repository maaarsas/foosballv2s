using System;

namespace foosballv2s.Source.Entities
{
    public class GameEvent
    {
        public Game Game { get; set; }
        public EventTypes EventType { get; set; }
        public Team Team { get; set; } = null;
        public DateTime EventTime { get; set; }

        public GameEvent()
        {
            Team = null;
            EventTime = DateTime.Now;
        }

        public enum EventTypes
        {
            NoType,
            GameStart,
            Goal,
            GameEnd,
        }
      
    }
}