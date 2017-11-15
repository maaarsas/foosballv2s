using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using foosballv2s.Source.Activities.Adapters;
using foosballv2s.Source.Activities.Helpers;
using foosballv2s.Source.Entities;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using Xamarin.Forms;
using ListView = Android.Widget.ListView;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for displaying all teams
    /// </summary>
    [Activity(ParentActivity=typeof(MainActivity))]
    public class TeamsActivity : AppCompatActivity
    {
        private TeamRepository teamRepository;
        private ListView teamListView;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Teams);
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_teams);

            teamRepository = DependencyService.Get<TeamRepository>();
            teamListView = (ListView) FindViewById(Resource.Id.team_list_view);
            
            FetchTeams();
        }

        /// <summary>
        /// Fetches the teams and populates them to a list
        /// </summary>
        private async void FetchTeams()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.retrieving_teams), true);
            
            Team[] teams = await teamRepository.GetAll();
            
            dialog.Dismiss();
            
            TeamAdapter teamAdapter = new TeamAdapter(this, new List<Team>(teams));
            teamListView.Adapter = teamAdapter;
        }
    }
}