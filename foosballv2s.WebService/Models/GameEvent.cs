using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace foosballv2s.WebService.Models
{
    public class GameEvent
    {
        public int Id { get; set; }
        public int EventType { get; set; }
        public Team Team { get; set; } = null;
        public DateTime EventTime { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Game Game { get; set; } = null;

    }
}