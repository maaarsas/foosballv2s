using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace foosballv2s.WebService.Models
{
    public class TournamentPair
    {
        public int Id { get; set; }

        public int GamePairNumberInStage { get; set; }

        public int StageNumber { get; set; } = 1;

        public virtual Team Team1 { get; set; }
        public int? Team1Id { get; set; }
        public virtual Team Team2 { get; set; }
        public int? Team2Id { get; set; }

        public virtual Game Game { get; set; }
        public int? GameId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Tournament Tournament { get; set; } = null;
    }
}
