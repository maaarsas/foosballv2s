using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Models
{
    public class TournamentGame
    {
        public int Id { get; set; }

        public int GamePairNumberInStage { get; set; }

        public int StageNumber { get; set; } = 1;

        public Game Game { get; set; }

    }
}
