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
    class GameStats
    {
        public string team1name { get; set; }
        public string team2name { get; set; }
        public int team1score { get; set; }
        public int team2score { get; set; }
        public bool victory { get; set; }
        //team 1 victory - true, team 2 victory - false

    }
}