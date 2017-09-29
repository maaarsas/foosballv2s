using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace foosballv2s
{
    class GameTracker
    {
        public void GameStart(Team team1, Team team2, IO r)
        {
            Stopwatch timer = new Stopwatch();
            TimeSpan ts = new TimeSpan();
            timer.Start();

            team2.totalScore = 7;

            while (true)
            {
                if (/*team1 goal*/true)
                {
                    team1.totalScore++;
                    r.Write(string.Format("{0:HH:mm:ss}", DateTime.Now), teamName: team1.teamName, totalScore: team1.totalScore, ts: ts, timer: timer);
                }
                if (/*team2 goal*/true)
                {
                    team2.totalScore++;
                    r.Write(string.Format("{0:HH:mm:ss}", DateTime.Now), teamName: team2.teamName, totalScore: team2.totalScore, ts: ts, timer: timer);
                }
                if (/*stop*/ true)
                {
                    timer.Reset();
                    break;
                }
            }
        }
    }
}
