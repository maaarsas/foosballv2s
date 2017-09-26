using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace foosballv2s
{
    class IO
    {
        public void Write(string goalTime, string teamName, int totalScore, TimeSpan ts, Stopwatch timer)
        {
            string filePath = @"C:\Users\Radvila\Documents\GitHub\foosballv2s\foosballv2s\GameInfo.txt";

            ts = timer.Elapsed;
            File.AppendAllText(filePath, goalTime + " " + teamName + " score a goal! " + " Total score is " + totalScore + " " + ts + Environment.NewLine);
            if (totalScore == 8)
            {
                timer.Reset();
                File.AppendAllText(filePath, Environment.NewLine + teamName + " Laimejo!" + Environment.NewLine);
            }
        }

        public void WriteTeamNames(Team team1, Team team2)
        {
            string filePath = @"C:\Users\Radvila\Documents\GitHub\foosballv2s\foosballv2s\GameInfo.txt";

            File.AppendAllText(filePath, team1.teamName + Environment.NewLine + "vs. " + Environment.NewLine + team2.teamName + Environment.NewLine + Environment.NewLine);
        }

        public void ReadTeamNames(Team team1, Team team2)
        {
            string filePath = @"C:\Users\Radvila\Documents\GitHub\foosballv2s\foosballv2s\GameInfo.txt";

            string teamNames = File.ReadLines(filePath).First();

            string[] entries = teamNames.Split(' ');

            team1.teamName = File.ReadLines(filePath).First();
            team2.teamName = File.ReadLines(filePath).Skip(2).Take(1).First();
        }
    }
}
