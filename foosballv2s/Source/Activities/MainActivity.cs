using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using foosballv2s.Source.Activities.Adapters;
using foosballv2s.Source.Activities.Helpers;
using foosballv2s.Source.Entities;
using foosballv2s.Source.Services.FileIO;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using foosballv2s.Source.Services.TextToSpeech;
using Java.Interop;
using Xamarin.Forms;
using View = Android.Views.View;
using Android.Media;
using Android.Views;
using foosballv2s.Source.Services.CredentialStorage;
using foosballv2s.Source.Services.FoosballWebService;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// Main activity for choosing the teams for the game
    /// </summary>
    [Activity]
    public class MainActivity : AppCompatActivity
    {
        private AutoCompleteTextView firstTeamTextView, secondTeamTextView;
        private IO instance = new IO();
        private Game game;
        private TeamRepository teamRepository;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            VolumeControlStream = Android.Media.Stream.Music;
            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Main);

            game = DependencyService.Get<Game>();
            teamRepository = DependencyService.Get<TeamRepository>();

            firstTeamTextView = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team1Name);
            secondTeamTextView = (AutoCompleteTextView)FindViewById<AutoCompleteTextView>(Resource.Id.team2Name);

            firstTeamTextView.ItemClick += AutoCompleteTextView_ItemClicked;
            secondTeamTextView.ItemClick += AutoCompleteTextView_ItemClicked;

            var btnP = FindViewById<Android.Widget.Button>(Resource.Id.prev);
            btnP.Click += BtnPrev_Click;

            //Window.SetBackgroundDrawable(Android.Resource.Id.);
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.app_name);
        }


        protected override void OnResume()
        {
            base.OnResume();
            SetupTeamDropdownList();
        }

        /// <summary>
        /// A function that is called when the "Previous settings" button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrev_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ColorPickerActivity));

            StartActivity(intent);
        }

        /// <summary>
        /// Called when submit is clicked
        /// Checks the teams and starts the game
        /// </summary>
        /// <param name="view"></param>
        [Export("SubmitTeamNames")]
        public async void SubmitTeamNames(View view)
        {
            VolumeControlStream = Android.Media.Stream.Music;
            ProgressDialog dialog = ProgressDialog.Show(this, "",
                Resources.GetString(Resource.String.checking_teams), true);

            Team team1 = ((TeamAutoCompleteAdapter)firstTeamTextView.Adapter).SelectedTeam;
            Team team2 = ((TeamAutoCompleteAdapter)secondTeamTextView.Adapter).SelectedTeam;

            //Example how to use TextToSpeech
            DependencyService.Get<ITextToSpeech>().Speak("Welcome " + firstTeamTextView.Text + " and " + secondTeamTextView.Text);

            List<Team> l = new List<Team>(await teamRepository.GetAll());
            Func<string, bool> d = delegate (string s)
            {
                foreach (Team t in l)
                {
                    if (t.TeamName.Equals(s))
                    {
                        string msg = Resources.GetString(Resource.String.team_name_exists);
                        string msg2 = System.String.Format(msg, s);
                        Toast.MakeText(Android.App.Application.Context, msg2, ToastLength.Short).Show();
                        return false;
                    }
                }
                return true;
            };
            
            dialog.Dismiss();
            
            // first team is not selected from the list, so it is a new one, create it
            if (team1 == null)
            {
                if (d(firstTeamTextView.Text))
                {
                    team1 = new Team { TeamName = firstTeamTextView.Text };
                    team1 = await teamRepository.Create(team1);
                    if (team1 == null)
                    {
                        Toast.MakeText(
                            Android.App.Application.Context, Resource.String.wrong_first_team_name, ToastLength.Short)
                            .Show();
                        return;
                    }
                }
                else return;
            }
            // second team is not selected from the list, so it is a new one, create it
            if (team2 == null)
            {
                if (d(secondTeamTextView.Text))
                {
                    team2 = new Team { TeamName = secondTeamTextView.Text };
                    team2 = await teamRepository.Create(team2);
                    if (team2 == null)
                    {
                        await teamRepository.Delete(team1.Id);
                        Toast.MakeText(
                                Android.App.Application.Context, Resource.String.wrong_second_team_name, ToastLength.Short)
                            .Show();
                        return;
                    }
                }
                else return;
            }

            if (team1.TeamName.Equals(team2.TeamName))
            {
                Toast.MakeText(Android.App.Application.Context, Resource.String.same_team_names, ToastLength.Short).Show();
                return;
            }

            game.Team1 = team1;
            game.Team2 = team2;

            Intent intent = new Intent(this, typeof(BallImageActivity));
            StartActivity(intent);
        }

        /// <summary>
        /// Sets up a auto complete drop down list with teams from the web service
        /// </summary>
        private void SetupTeamDropdownList()
        {
            FetchUserTeams();
        }

        /// <summary>
        /// Retrievies teams and populates the dropdown list
        /// </summary>
        private async void FetchUserTeams()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "",
                Resources.GetString(Resource.String.retrieving_your_teams), true);

            var credentialStorage = DependencyService.Get<ICredentialStorage>();
            UrlParamsFormatter urlParams = new UrlParamsFormatter();
            urlParams.AddParam("userid", credentialStorage.Read().Id);
            urlParams.AddParam("sortby", "-id");
            
            Team[] teams = await teamRepository.GetAll(urlParams.UrlParams);

            dialog.Dismiss();

            TeamAutoCompleteAdapter teamAdapter1 = new TeamAutoCompleteAdapter(this, new List<Team>(teams));
            TeamAutoCompleteAdapter teamAdapter2 = new TeamAutoCompleteAdapter(this, new List<Team>(teams));

            firstTeamTextView.Adapter = teamAdapter1;
            secondTeamTextView.Adapter = teamAdapter2;

            firstTeamTextView.Text = "";
            secondTeamTextView.Text = "";

        }

        /// <summary>
        /// An event when the dropdown list team is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoCompleteTextView_ItemClicked(object sender, AdapterView<TeamAutoCompleteAdapter>.ItemClickEventArgs e)
        {
            AutoCompleteTextView view = (AutoCompleteTextView)sender;
            TeamAutoCompleteAdapter teamAdapter = (TeamAutoCompleteAdapter)view.Adapter;
            var team = teamAdapter.GetItem(e.Position);
            teamAdapter.SelectedTeam = team;
            teamAdapter.IgnoreFilter = true;
            view.Text = team.TeamName;
            view.SetSelection(view.Text.Length);
        }
    }
}