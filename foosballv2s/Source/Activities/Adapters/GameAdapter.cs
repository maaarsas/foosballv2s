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

namespace foosballv2s.Source.Activities.Adapters
{
    class GameAdapter : BaseAdapter<Game>
    {
        List<Game> gList;
        private Context gContext;

        public GameAdapter(Context context, List<Game> list)
        {
            gList = list;
            gContext = context;
        }
        public override int Count
        {
            get { return gList.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Game this[int position]
        {
            get { return gList[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(gContext).Inflate(Resource.Layout.StatsRow, null, false);
            }

            Game game = gList[position];
            
            TextView t1name = row.FindViewById<TextView>(Resource.Id.statsTeam1);
            TextView t2name = row.FindViewById<TextView>(Resource.Id.statsTeam2);
            TextView t1score = row.FindViewById<TextView>(Resource.Id.scoreTeam1);
            TextView t2score = row.FindViewById<TextView>(Resource.Id.scoreTeam2);
            TextView time = row.FindViewById<TextView>(Resource.Id.gameTime);

            t1name.Text = game.Team1.TeamName;
            t2name.Text = game.Team2.TeamName;
            t1score.Text = game.Team1Score.ToString();
            t2score.Text = game.Team2Score.ToString();
            time.Text = GameTimeHelper.GetTimeString(game.StartTime, game.EndTime);

            return row;
        }
    }
}