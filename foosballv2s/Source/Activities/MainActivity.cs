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

namespace foosballv2s
{
    [Activity(
        ConfigurationChanges = ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait
        )]
    public class MainActivity : Activity
    {
        private AutoCompleteTextView t1, t2, team1text, team2text;
        privateIO instance = new IO();
        private ArrayAdapter<Team> teamAdapter;
        private Game game;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Main);
            
            DependencyService.Register<Game>();
            game = DependencyService.Get<Game>();
            
            t1 = (AutoCompleteTextView) FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            t2 = (AutoCompleteTextView) FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);
            
            t1.ItemClick += AutoCompleteTextView_ItemClicked;
            t2.ItemClick += AutoCompleteTextView_ItemClicked;
            
            SetupTeamDropdownList();
            var btnP = FindViewById<Android.Widget.Button>(Resource.Id.prev);
            btnP.Click += BtnPrev_Click;
            
            //Window.SetBackgroundDrawable(Android.Resource.Id.);
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ColorPickerActivity));

            StartActivity(intent);
        }

        [Export("SubmitTeamNames")]
        public void SubmitTeamNames(View view)
        {
            Validator v = new Validator();

            Intent intent = new Intent(this, typeof(BallImageActivity));

            team1text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            team2text = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);

            if (!v.Validate(team1text.Text) || !v.Validate(team2text.Text))
            {
                Toast.MakeText(this, Resource.String.wrong_team_names, ToastLength.Short);
                return;
            }

            game.Team1.TeamName = team1text.Text;
            game.Team2.TeamName = team2text.Text;

            instance.Write_Serialize(team1text, team2text);

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
            ProgressDialog dialog = ProgressDialog.Show(ApplicationContext, "", 
                Resources.GetString(Resource.String.retrieving_teams), true);
            
            TeamRepository repository = new TeamRepository(new FoosballWebServiceClient());
            Team[] teams = await repository.GetAll();
            
            dialog.Dismiss();
            
            teamAdapter = new TeamAdapter(this, new List<Team>(teams));

            t1.Adapter = teamAdapter;
            t2.Adapter = teamAdapter;
            
        }
        
        private void AutoCompleteTextView_ItemClicked(object sender, AdapterView<TeamAdapter>.ItemClickEventArgs e)
        {
            AutoCompleteTextView view = (AutoCompleteTextView) sender;
            var team = teamAdapter.GetItem(e.Position);
            view.Text = team.TeamName;
        }
    }
    
  
}