using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Models
{
    public class TournamentTeam
    {
        public int Id { get; set; }

        public virtual Team Team { get; set; }
        public int? TeamId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Tournament Tournament { get; set; } = null;

    }
}
