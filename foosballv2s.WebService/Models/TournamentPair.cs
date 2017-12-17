using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Models
{
    public class TournamentPair
    {
        public int Id { get; set; }

        public int GamePairNumberInStage { get; set; }

        public int StageNumber { get; set; } = 1;

        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        public Game Game { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Tournament Tournament { get; set; } = null;
    }
}
