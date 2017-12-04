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
using foosballv2s.Source.Entities;
using Java.Lang;
using Object = Java.Lang.Object;
using foosballv2s.Source.Activities.Helpers;

namespace foosballv2s.Source.Activities.Adapters
{
    public class ExpandableListAdapter : BaseExpandableListAdapter
    {
        private Context context;
        private List<Game> listGroup;
        private Dictionary<Game, List<Game>> listChild;

        public ExpandableListAdapter(Context context, List<Game> listGroup, Dictionary<Game, List<Game>> listChild)
        {
            this.context = context;
            this.listGroup = listGroup;
            this.listChild = listChild;
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
            var result = new List<Game>();
            listChild.TryGetValue(listGroup[groupPosition], out result);
            return result[childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            var result = new List<Game>();
            listChild.TryGetValue(listGroup[groupPosition], out result);
            return result.Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            if(convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.item_team, null);
            }
            TextView viewItemTime = convertView.FindViewById<TextView>(Resource.Id.item_time);
            TextView viewItemEvent = convertView.FindViewById<TextView>(Resource.Id.item_event);

            var eList = new List<Game>();
            listChild.TryGetValue(listGroup[groupPosition], out eList);
            Game content = eList[childPosition];

            

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
            if(convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.Games, null);
            }
            Game gameGroup = listGroup[groupPosition];
            TextView timeViewGroup = convertView.FindViewById<TextView>(Resource.Id.gameTime);
            TextView t1ViewGroup = convertView.FindViewById<TextView>(Resource.Id.statsTeam1);
            TextView t2ViewGroup = convertView.FindViewById<TextView>(Resource.Id.statsTeam2);
            TextView g1ViewGroup = convertView.FindViewById<TextView>(Resource.Id.scoreTeam1);
            TextView g2ViewGroup = convertView.FindViewById<TextView>(Resource.Id.scoreTeam2);

            t1ViewGroup.Text = gameGroup.Team1.TeamName;
            t2ViewGroup.Text = gameGroup.Team2.TeamName;
            t1ViewGroup.Text = gameGroup.Team1Score.ToString();
            t2ViewGroup.Text = gameGroup.Team2Score.ToString();
            timeViewGroup.Text = GameTimeHelper.GetTimeString(gameGroup.StartTime, gameGroup.EndTime);

            return convertView;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }