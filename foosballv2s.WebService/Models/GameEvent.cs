using System;

namespace foosballv2s.WebService.Models
{
    public class GameEvent
    {
        public int Id { get; set; }
        public int EventType { get; set; }
        public Team Team { get; set; } = null;
        public DateTime EventTime { get; set; }
        public Game Game { get; set; }
      
    }
}