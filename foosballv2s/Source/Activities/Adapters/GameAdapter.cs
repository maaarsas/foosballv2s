using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace foosballv2s.Source.Activities.Adapters
{
    class GameAdapter : ArrayAdapter<Game>
    {
        List<Game> gList;
        private Context gContext;

        public GameAdapter(Context context, List<Game> list): base(context, 0, list)
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
        
        public Game GetItem(int position)
        {
            return gList.ElementAt(position);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(gContext).Inflate(Resource.Layout.StatsRow, parent, false);
            }

            Game game = GetItem(position);
            
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
            
            SetTeamBackgroundColor(parent, t1name, game.Team1Score);
            SetTeamBackgroundColor(parent, t2name, game.Team2Score);
            return row;
        }

        private void SetTeamBackgroundColor(ViewGroup parent, TextView textView, int teamScore)
        {
            if (teamScore == Game.MAX_SCORE)
            {
                textView.SetBackgroundColor(parent.Resources.GetColor(Resource.Color.winning_background));
                textView.SetTextColor(parent.Resources.GetColor(Resource.Color.Black));
            }
            else
            {
                textView.SetBackgroundColor(parent.Resources.GetColor(Resource.Color.losing_background));
            }
        }
    }
}