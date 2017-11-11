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

namespace foosballv2s
{
    class SLSupp
    {
        public List<String> PreSaveCheck(String team1name, String team2name, List<String> names)
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

        private bool NameCheck(String teamname, String listname)
        {
            if (teamname == listname) return true;
            else return false;
        }

        private List<String> ToAdd(bool flag, String teamname, List<String> names)
        {
            if (flag == false) names.Add(teamname);
            return names;
        }

        public List<String> CheckPreload(List<String> names)
        {
            if(names == null)
            {
                names = new List<String>
                    {
                        "Right Team",
                        "Left Team"
                    };
            }
            return names;
        }
    }
}