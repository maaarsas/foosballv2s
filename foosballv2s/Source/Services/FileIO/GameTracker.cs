﻿using System;
using System.Diagnostics;
using foosballv2s.Source.Entities;

namespace foosballv2s.Source.Services.FileIO
{
    class GameTracker
    {
        public void GameStart(Team team1, Team team2, IO r)
        {
            Stopwatch timer = new Stopwatch();
            TimeSpan ts = new TimeSpan();
            timer.Start();

            team2.TotalScore = 7;

            while (true)
            {
                if (/*team1 goal*/true)
                {
                    team1.TotalScore++;
                    r.Write(string.Format("{0:HH:mm:ss}", DateTime.Now), teamName: team1.TeamName, totalScore: team1.TotalScore, ts: ts, timer: timer);
                }
                if (/*team2 goal*/true)
                {
                    team2.TotalScore++;
                    r.Write(string.Format("{0:HH:mm:ss}", DateTime.Now), teamName: team2.TeamName, totalScore: team2.TotalScore, ts: ts, timer: timer);
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
