using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foosballv2s.WebService.Models
{
    public class TournamentStage
    {
        public int Id { get; set; }

        public int StageNumber { get; set; }

        public List<Game> Games { get; set; }

        public bool AllGamesEnded
        {
            get
            {
                foreach(Game Game in Games)
                {
                    if(Game.EndTime == null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

    }
}
