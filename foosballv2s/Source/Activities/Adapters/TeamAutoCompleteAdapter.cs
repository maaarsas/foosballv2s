using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using foosballv2s.Filters;

namespace foosballv2s.Adapters
{
    public class TeamAutoCompleteAdapter : ArrayAdapter<Team>
    {
        Filter filter;
        public List<Team> teams;
        public List<Team> matchTeams;
        public Team SelectedTeam { get; set; }
        public bool IgnoreFilter { get; set; }

        public TeamAutoCompleteAdapter(Context context, List<Team> teams) : base(context, 0, teams)
        {
            this.teams = teams;
            this.matchTeams = teams;
            filter = new TeamFilter(this);
        }
        
        public override int Count {
            get {
                return matchTeams.Count;
            }
        }
        
        public Team GetItem(int position)
        {
            return matchTeams.ElementAt(position);
        }
    
        public override View GetView(int position, View convertView, ViewGroup parent) {
            
            // Get the data item for this position
            Team team = GetItem(position);    
            
            // Check if an existing view is being reused, otherwise inflate the view
            if (convertView == null) {
                convertView = LayoutInflater.From(Context).Inflate(Resource.Layout.item_team, parent, false);
            }
            
            // Lookup view for data population
            TextView tvName = (TextView) convertView.FindViewById(Resource.Id.team_item_name);
            
            // Populate the data into the template view using the data object
            tvName.Text = (position+1) + ".\t" + team.TeamName;
            
            // Return the completed view to render on screen
            return convertView;
        }

        public override Filter Filter {
            get {
                return filter;
            }
        }
    }
}