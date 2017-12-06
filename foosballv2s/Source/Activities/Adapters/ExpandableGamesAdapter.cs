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
using Object = Java.Lang.Object;
using foosballv2s.Droid.Shared.Source.Helpers;
using foosballv2s.Droid.Shared.Source.Entities;
using Android.Content.Res;

namespace foosballv2s.Source.Activities.Adapters
{
    public class ExpandableListAdapter : BaseExpandableListAdapter
    {
        private Context context;
        private List<Game> listGroup;

        public ExpandableListAdapter(Context context, List<Game> listGroup)
        {
            this.context = context;
            this.listGroup = listGroup;
        }
        public override int GroupCount
        {
            get { return listGroup.Count; }
        }

        public override bool HasStableIds
        {
            get { return false; }
        }

        public override Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return listGroup[groupPosition].GameEvents.Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.item_team, null);
            }
            TextView viewItemTime = convertView.FindViewById<TextView>(Resource.Id.item_time);
            TextView viewItemEvent = convertView.FindViewById<TextView>(Resource.Id.item_event);
            TextView viewItemDesc = convertView.FindViewById<TextView>(Resource.Id.item_desc);

            var eList = listGroup[groupPosition].GameEvents.OrderBy(e => e.EventTime);
            var eventTime = eList.ElementAt(childPosition).EventTime;
            viewItemTime.Text = GameTimeHelper.GetTimeString(listGroup[groupPosition].StartTime, eventTime);

            var team = eList.ElementAt(childPosition).Team;
            if (team != null)
            {
                viewItemDesc.Text = eList.ElementAt(childPosition).Team.TeamName;
            }
            else
            {
                viewItemDesc.Text = "";
            }

            var evt = eList.ElementAt(childPosition).EventType.ToString();
            if (evt == "GameStart")
            {
                viewItemEvent.Text = Resource.String.game_start.ToString();
            }
            else if (evt == "GameEnd")
            {
                viewItemEvent.Text = Resource.String.game_end.ToString();
            }
            else
            {
                viewItemEvent.Text = evt;
            }

            return convertView;
        }

        public override Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.StatsRow, parent, false);
            }

            Game game = listGroup[groupPosition];
            
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

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
        
        /// <summary>
        /// Sets the background of the team name to seperate winning and losing teams
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="textView"></param>
        /// <param name="teamScore"></param>
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
                textView.SetTextColor(parent.Resources.GetColor(Resource.Color.White));
            }
        }
    }
}