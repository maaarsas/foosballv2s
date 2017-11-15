using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Android.Widget;
using Newtonsoft.Json;
using Color = Android.Graphics.Color;

namespace foosballv2s.Source.Services.FileIO
{
    /// <summary>
    /// A class for writing and reading files
    /// </summary>
    class IO
    {
        private string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/previousnames.json";
        private string path_color = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/previouscolors.json";
        private string path_stats = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/stats.json";
        private Array dataRead;
        private List<String> names = new List<String>();
        private string team1name, team2name;
        private string data, colordata;
        private SLSupp instance = new SLSupp();
        private List<Color> colorlist = new List<Color>();

        /// <summary>
        /// Logs the game events
        /// </summary>
        /// <param name="goalTime"></param>
        /// <param name="teamName"></param>
        /// <param name="totalScore"></param>
        /// <param name="ts"></param>
        /// <param name="timer"></param>
        public void Write(string goalTime, string teamName, int totalScore, TimeSpan ts, Stopwatch timer)
        {
            ts = timer.Elapsed;
            File.AppendAllText(path, goalTime + " " + teamName + " scored a goal! " + " Total score is " + totalScore + " " + ts + Environment.NewLine);
            if (totalScore == 8)
            {
                timer.Reset();
                File.AppendAllText(path, Environment.NewLine + teamName + " Laimejo!" + Environment.NewLine);
            }
        }

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

        public void Write_Serialize_Color(Color color)
        {
            instance.CheckColor(colorlist, color);

            colordata = JsonConvert.SerializeObject(colorlist);

            File.WriteAllText(path_color, colordata);

        }

        public List<Color> Read_Deserialize_Color()
        {
            if (!System.IO.File.Exists(path_color))
            {
                System.IO.FileStream fs = System.IO.File.Create(path_color);
                fs.Dispose();
            }
            else
            {
                var colordata = File.ReadAllText(path_color);
                colorlist = JsonConvert.DeserializeObject<List<Color>>(colordata);
            }

            return colorlist;
        }
    }
}
