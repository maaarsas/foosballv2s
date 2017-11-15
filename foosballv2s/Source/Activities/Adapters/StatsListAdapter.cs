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
    class StatsListAdapter : BaseAdapter<GameStats>
    {
        List<GameStats> gList;
        private Context gContext;

        public StatsListAdapter(Context context, List<GameStats> list)
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

        public override GameStats this[int position]
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

            TextView t1name = row.FindViewById<TextView>(Resource.Id.statsTeam1);
            TextView t2name = row.FindViewById<TextView>(Resource.Id.statsTeam2);
            TextView t1score = row.FindViewById<TextView>(Resource.Id.scoreTeam1);
            TextView t2score = row.FindViewById<TextView>(Resource.Id.scoreTeam2);
            TextView time = row.FindViewById<TextView>(Resource.Id.gameTime);

            string score1string = gList[position].team1score.ToString();
            string score2string = gList[position].team2score.ToString();
            //string timeString = gList[position].time.ToString();

            t1name.Text = gList[position].team1name;
            t2name.Text = gList[position].team2name;
            t1score.Text = score1string;
            t2score.Text = score2string;
            time.Text = gList[position].time.ToString();

            return row;
        }
    }
}