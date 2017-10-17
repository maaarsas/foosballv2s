using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using Android.Widget;

namespace foosballv2s
{
    class IO
    {
        private String path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/previousnames.json";
        private Array dataRead;
        private List<String> names = new List<String>();
        private String team1name, team2name;
        private String data;
        SLSupp instance = new SLSupp();

        public void Write(string goalTime, string teamName, int totalScore, TimeSpan ts, Stopwatch timer)
        {
            string filePath = @"C:\Users\Radvila\Documents\GitHub\foosballv2s\foosballv2s\GameInfo.txt";

            ts = timer.Elapsed;
            File.AppendAllText(filePath, goalTime + " " + teamName + " scored a goal! " + " Total score is " + totalScore + " " + ts + Environment.NewLine);
            if (totalScore == 8)
            {
                timer.Reset();
                File.AppendAllText(filePath, Environment.NewLine + teamName + " Laimejo!" + Environment.NewLine);
            }
        }

        //Since we've decided on using JSON for saving/reading these are not required. May be used in later functionality, such as statistics

        /*public void WriteTeamNames(Team team1, Team team2)
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
        }*/

        public Array Read_Deserialize() {
            if (!System.IO.File.Exists(path))
            {
                System.IO.FileStream fs = System.IO.File.Create(path);
                fs.Dispose();
            }
            else
            {
                var data = File.ReadAllText(path);
                names = JsonConvert.DeserializeObject<List<String>>(data);

                //Need help with this IF clause (in SLSupp class). Need to recreate list if file exists and is empty. Solutions?

                names = instance.CheckPreload(names);
            }
            dataRead = names.ToArray();
            return dataRead;
        }

        public void Write_Serialize(AutoCompleteTextView team1text, AutoCompleteTextView team2text) {

            team1name = team1text.Text;
            team2name = team2text.Text;

            instance.CheckPreload(names);
            names = instance.PreSaveCheck(team1name, team2name, names);

            data = JsonConvert.SerializeObject(names);
            File.WriteAllText(path, data);
        }
    }
}
