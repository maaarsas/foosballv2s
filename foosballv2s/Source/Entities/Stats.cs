using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace foosballv2s.Source.Entities
{
    class Stats
    {
        IO StatsIO = new IO();
        List<GameStats> statsList = new List<GameStats>();
        int victory;
        public void toStats(GameStats stats)
        {
            statsList = StatsIO.Read_Deserialize_Stats();
            statsList.Add(stats);
            StatsIO.Write_Serialize_Stats(statsList);
        }

        public int victor(int team1score, int team2score)
        {
            if (team1score > team2score)
            {
                victory = 1;
            }
            else if (team1score < team2score)
            {
                victory = 2;
            }
            else victory = 0;

            return victory;
        }
    }
}