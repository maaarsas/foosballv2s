using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<TournamentStage> Stages { get; set; }

        public bool AddStage(TournamentStage Stage)
        {
            if(Stages == null)
            {
                Stages = new List<TournamentStage>();
            }

            if (!ContainsStageOfNumber(Stage.StageNumber))
            {
                Stages.Add(Stage);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ContainsStageOfNumber(int Number)
        {
            foreach(TournamentStage Stage in Stages)
            {
                if(Stage.StageNumber == Number)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
