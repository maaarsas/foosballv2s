using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Provider;
using Android.Runtime;
using Emgu.CV.Structure;
using Java.Interop;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using foosballv2s.Adapters;
using foosballv2s.Source.Services.FoosballWebService;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using Java.Interop;
using Xamarin.Forms;
using View = Android.Views.View;
using System.Collections.Generic;
using Android.Text;
using Java.Lang;

namespace foosballv2s
{
    [Activity(
        ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait
        )]
    public class MainActivity : Activity
    {
        private AutoCompleteTextView firstTeamTextView, secondTeamTextView;
        private IO instance = new IO();
        private Game game;
        private TeamRepository teamRepository;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Main);
            
            game = DependencyService.Get<Game>();
            teamRepository = DependencyService.Get<TeamRepository>();
            
            firstTeamTextView = (AutoCompleteTextView) FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            secondTeamTextView = (AutoCompleteTextView) FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);
            
            firstTeamTextView.ItemClick += AutoCompleteTextView_ItemClicked;
            secondTeamTextView.ItemClick += AutoCompleteTextView_ItemClicked;
            
            var btnP = FindViewById<Android.Widget.Button>(Resource.Id.prev);
            btnP.Click += BtnPrev_Click;
            
            //Window.SetBackgroundDrawable(Android.Resource.Id.);
            NavigationHelper.SetupNavigationListener(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            SetupTeamDropdownList();
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ColorPickerActivity));

            StartActivity(intent);
        }

        [Export("SubmitTeamNames")]
        public async void SubmitTeamNames(View view)
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.checking_teams), true);

            Team team1 = ((TeamAdapter) firstTeamTextView.Adapter).SelectedTeam;
            Team team2 = ((TeamAdapter) secondTeamTextView.Adapter).SelectedTeam;

            // first team is not selected from the list, so it is a new one, create it
            if (team1 == null)
            {
                team1 = new Team {TeamName = firstTeamTextView.Text};
                team1 = await teamRepository.Create(team1);
            }
            // second team is not selected from the list, so it is a new one, create it
            if (team2 == null)
            {
                team2 = new Team {TeamName = secondTeamTextView.Text};
                team2 = await teamRepository.Create(team2);
            }
            
            dialog.Dismiss();
            
            // if even after creation teams do not exist, means there is an error in the names
            if (team1 == null || team2 == null)
            {
                Toast.MakeText(this, Resource.String.wrong_team_names, ToastLength.Short);
                return;
            }

            game.Team1 = team1;
            game.Team2 = team2;

            Intent intent = new Intent(this, typeof(BallImageActivity));
            StartActivity(intent);
        }

        private void BtnStats_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(StatsActivity));

            StartActivity(intent);
        }

        private void SetupTeamDropdownList()
        {
            
            FetchAllTeams();
        }

        private async void FetchAllTeams()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.retrieving_teams), true);
            
            Team[] teams = await teamRepository.GetAll();
            
            dialog.Dismiss();
            
            TeamAdapter teamAdapter1 = new TeamAdapter(this, new List<Team>(teams));
            TeamAdapter teamAdapter2 = new TeamAdapter(this, new List<Team>(teams));

            firstTeamTextView.Adapter = teamAdapter1;
            secondTeamTextView.Adapter = teamAdapter2;
            
        }
        
        private void AutoCompleteTextView_ItemClicked(object sender, AdapterView<TeamAdapter>.ItemClickEventArgs e)
        {
            AutoCompleteTextView view = (AutoCompleteTextView) sender;
            TeamAdapter teamAdapter = (TeamAdapter) view.Adapter;
            var team = teamAdapter.GetItem(e.Position);
            teamAdapter.SelectedTeam = team;
            teamAdapter.IgnoreFilter = true;
            view.Text = team.TeamName;
        }
    }
    
  
}