using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foosballv2s
{
    class Program
    {
        static void Main(string[] args)
        {
            Team team1 = new Team();
            Team team2 = new Team();
            team1.teamName = "Komanda 1";
            team2.teamName = "Komanda 2";
            IO r = new IO();
            r.WriteTeamNames(team1, team2);
            r.ReadTeamNames(team1, team2);
            if (/*start*/true)
            {
                GameTracker tracker = new GameTracker();
                tracker.GameStart(team1, team2, r);
            }
        }
    }
}
