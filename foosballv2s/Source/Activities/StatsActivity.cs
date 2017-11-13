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
        private List<GameStats> gamesList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.StatsScreen);

            statsListView = FindViewById<ListView>(Resource.Id.StatisticsList);

            StatsListAdapter statsAdapter = new StatsListAdapter(this, gamesList);

            statsListView.Adapter = statsAdapter;
        }
    }
}