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
    [Activity(Label = "StatsActivity")]
    public class StatsActivity : Activity
    {
        private ListView statsListView;
        private List<GameStats> gamesList = new List<GameStats>();
        private IO stats = new IO();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.StatsScreen);

            statsListView = FindViewById<ListView>(Resource.Id.StatisticsList);

            //list element for testing purposes, color hardcoded to #64DD17 to test if possible to mark the victor
            gamesList.Add(new GameStats() {team1name = "scrubs", team2name = "noobs", team1score = 7, team2score = 3, victory = true, time = "7:45"});
            gamesList.Add(new GameStats() { team1name = "dar vieni", team2name = "hackerman", team1score = 7, team2score = 0, victory = true, time = "2:01" });

            StatsListAdapter statsAdapter = new StatsListAdapter(this, gamesList);
            statsListView.Adapter = statsAdapter;
        }
    }
}