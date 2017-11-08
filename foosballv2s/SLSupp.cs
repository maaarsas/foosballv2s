using System.Collections.Generic;
using Color = Android.Graphics.Color;

namespace foosballv2s
{
    class SLSupp : IComparer
    {
        public List<string> PreSaveCheck(string team1name, string team2name, List<string> names)
        {
            bool flag1 = false;
            bool flag2 = false;
            foreach (var name in names)
            {
                flag1 = NameCheck(team1name, name);
                flag2 = NameCheck(team2name, name);
            }

            ToAdd(flag1, team1name, names);
            ToAdd(flag2, team2name, names);

            return names;
        }

        private bool NameCheck(string teamname, string listname)
        {
            if (teamname == listname) return true;
            else return false;
        }

        private List<string> ToAdd(bool flag, string teamname, List<string> names)
        {
            if (flag == false) names.Add(teamname);
            return names;
        }

        public List<string> CheckPreload(List<string> names)
        {
            if(names == null)
            {
                names = new List<string>
                    {
                        "Right Team",
                        "Left Team"
                    };
            }
            return names;
        }

        public List<Color> CheckColor(List<Color> colorlist, Color colorgiven)
        {
            bool flag = false;

            foreach (var color in colorlist)
            {
                if(color == colorgiven)
                {
                    flag = true;
                }
            }

            if (flag == false)
            {
                colorlist.Add(colorgiven);
            }

            return colorlist;
        }
    }
}