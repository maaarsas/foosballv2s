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
    public class ExpandableTournamentsAdapter : BaseExpandableListAdapter
    {
        private Context context;
        private List<Tournament> listGroup;

        public ExpandableTournamentsAdapter(Context context, List<Tournament> listGroup)
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
            if (listGroup[groupPosition].CurrentStage == 1)
                return listGroup[groupPosition].NumberOfTeamsRequired / 2;
            else
                return listGroup[groupPosition].Pairs.Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.item_pair, null);
            }
            TextView viewItemNumber = convertView.FindViewById<TextView>(Resource.Id.item_number);
            TextView viewItemTeam1 = convertView.FindViewById<TextView>(Resource.Id.item_team_1);
            TextView viewItemTeam2 = convertView.FindViewById<TextView>(Resource.Id.item_team_2);
            TextView viewItemStatus = convertView.FindViewById<TextView>(Resource.Id.item_status);

            //var eList = listGroup[groupPosition].GameEvents.OrderBy(e => e.EventTime);
            //var eventTime = eList.ElementAt(childPosition).EventTime;
            //viewItemTime.Text = GameTimeHelper.GetTimeString(listGroup[groupPosition].StartTime, eventTime);

            var pair = listGroup[groupPosition].Pairs.ElementAt(childPosition);
            if (pair.Team1 != null)
            {
                viewItemTeam1.Text = pair.Team1.TeamName;
            }
            else
            {
                viewItemTeam1.Text = "";
            }

            if (pair.Team2 != null)
            {
                viewItemTeam2.Text = pair.Team2.TeamName;
            }
            else
            {
                viewItemTeam2.Text = "";
            }

            viewItemNumber.Text = childPosition.ToString();

            if (pair.Game != null)
            {
                viewItemStatus.Text = "started/finished";
            }
            else
            {
                viewItemStatus.Text = "Not started";
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

            Tournament tournament = listGroup[groupPosition];
            
            TextView numberOfTeams = row.FindViewById<TextView>(Resource.Id.rowTeamsCount);
            TextView currentStage = row.FindViewById<TextView>(Resource.Id.rowCurrentStage);

            numberOfTeams.Text = tournament.Teams.Count.ToString();
            currentStage.Text = tournament.CurrentStage.ToString();
            
            return row;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}