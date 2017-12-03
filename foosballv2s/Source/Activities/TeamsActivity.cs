using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using foosballv2s.Source.Activities.Adapters;
using foosballv2s.Source.Activities.Helpers;
using foosballv2s.Source.Entities;
using foosballv2s.Source.Services.FoosballWebService.Repository;
using Xamarin.Forms;
using ListView = Android.Widget.ListView;
using Android.Widget;
using foosballv2s.Source.Activities.Fragments;
using foosballv2s.Source.Services.CredentialStorage;
using foosballv2s.Source.Services.FoosballWebService;
using Fragment = Android.Support.V4.App.Fragment;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for displaying all teams
    /// </summary>
    [Activity(ParentActivity=typeof(MainActivity), 
        ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class TeamsActivity : AppCompatActivity
    {
        private TabLayout tabLayout;
        private ViewPager viewPager;
        private TeamRepository teamRepository;
        private List<Team> teamList;
        private TeamListFragment myTeamsListFragment;
        private TeamListFragment allTeamsListFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Teams);
            
            myTeamsListFragment = new TeamListFragment();
            allTeamsListFragment = new TeamListFragment();
            
            viewPager = (ViewPager) FindViewById(Resource.Id.viewpager);
            setupViewPager(viewPager);
 
            tabLayout = (TabLayout) FindViewById(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_teams);
            
            teamRepository = DependencyService.Get<TeamRepository>();
        }
        
        protected override void OnResume()
        {
            base.OnResume();
            
            FetchUserTeams();
            FetchAllTeams();
        }

        /// <summary>
        /// Fetches the teams and populates them to a list
        /// </summary>
        private async void FetchUserTeams()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.retrieving_your_teams), true);

            GameRepository gameRepository = DependencyService.Get<GameRepository>();
            var credentialStorage = DependencyService.Get<ICredentialStorage>();

            UrlParamsFormatter urlParams = new UrlParamsFormatter();
            urlParams.AddParam("userid", credentialStorage.Read().Id);
            urlParams.AddParam("sortby", "-id");
            
            Game[] games = await gameRepository.GetAll(urlParams.UrlParams);
            Team[] teams = await teamRepository.GetAll(urlParams.UrlParams);

            teams = SetTeamsWinPercentage(teams, games);
            teamList = new List<Team>(teams);
            dialog.Dismiss();
            
            TeamAdapter teamAdapter = new TeamAdapter(this, teamList);
            myTeamsListFragment.TeamListView.Adapter = teamAdapter;
            myTeamsListFragment.TeamListView.ItemClick += OnListItemClick;
        }
        
        /// <summary>
        /// Fetches all teams and populates them to a list
        /// </summary>
        private async void FetchAllTeams()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.retrieving_all_teams), true);

            UrlParamsFormatter urlParams = new UrlParamsFormatter();
            urlParams.AddParam("sortby", "-id");
            
            Team[] teams = await teamRepository.GetAll(urlParams.UrlParams);
            
            dialog.Dismiss();
            
            TeamAdapter teamAdapter = new TeamAdapter(this, new List<Team>(teams));
            allTeamsListFragment.TeamListView.Adapter = teamAdapter;
        }

        /// <summary>
        /// This method will be called when an item in the list is selected.
        /// Calculates and sets number of wins and number of games played.
        /// </summary>
        private Team[] SetTeamsWinPercentage(Team[] teams, Game[] games)
        {
            int winnerID;

            foreach (Game game in games)
            {
                if (game.Team1Score > game.Team2Score)
                    winnerID = game.Team1.id;
                else winnerID = game.Team2.id;
                foreach (Team team in teams)
                {
                    if (game.Team1.id == team.id)
                    {
                        team.GamesPlayed = ++team.GamesPlayed;
                    }
                    if (game.Team2.id == team.id)
                    {
                        team.GamesPlayed = ++team.GamesPlayed;
                    }
                    if (winnerID == team.id)
                        team.GamesWon = ++team.GamesWon;
                }
            }
            return teams;
        }

        /// <summary>
        /// This method will be called when an item in the list is selected.
        /// </summary>
        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var team = teamList[e.Position];
            Team t = new Team();
            EditText input = new EditText(this);
            input.Hint = Resources.GetString(Resource.String.change_name_hint);
            Android.App.AlertDialog.Builder infoAlert = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog.Builder changeAlert = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog.Builder deleteAlert = new Android.App.AlertDialog.Builder(this);
            
            teamRepository = DependencyService.Get<TeamRepository>();
            
            infoAlert.SetTitle(team.TeamName)
                .SetMessage(System.String.Format(Resources.GetString(Resource.String.games_won_played_perc), 
                team.GamesWon, team.GamesPlayed, team.Percentage))
                .SetCancelable(false)
                .SetNegativeButton(Resource.String.edit, delegate
                {
                    changeAlert.Show();
                })
                .SetNeutralButton(Resource.String.delete, delegate
                {
                    deleteAlert.Show();
                })
                .SetPositiveButton(Resource.String.cancel, delegate
                {
                    infoAlert.Dispose();
                });

            changeAlert.SetMessage(Resource.String.change)
                .SetCancelable(false)
                .SetView(input)
                .SetNegativeButton(Resource.String.ok, async delegate
                {
                    if (!AlreadyExists(teamList, input.Text))
                    {
                        team.TeamName = input.Text;
                        t = await teamRepository.Update(team.id, team);
                        changeAlert.Dispose();
                        infoAlert.Dispose();
                    }
                    else
                        Toast.MakeText(Android.App.Application.Context, 
                            System.String.Format(Resources.GetString(Resource.String.team_name_exists2), input.Text), 
                            ToastLength.Short).Show();
                })
                .SetPositiveButton(Resource.String.cancel, delegate
                {
                    changeAlert.Dispose();
                });
            
            deleteAlert.SetMessage(System.String.Format(Resources.GetString(Resource.String.delete2), 
                team.TeamName))
                .SetCancelable(false)
                .SetNegativeButton(Resource.String.yes, async delegate
                {
                    t = await teamRepository.Delete(team.id);
                    if (t != null)
                        Toast.MakeText(Android.App.Application.Context, System.String.Format(Resources.GetString(Resource.String.notDel), team.TeamName), ToastLength.Short).Show();
                    else
                        Toast.MakeText(Android.App.Application.Context, System.String.Format(Resources.GetString(Resource.String.isDel), team.TeamName), ToastLength.Short).Show();

                    deleteAlert.Dispose();
                    infoAlert.Dispose();
                })
                .SetPositiveButton(Resource.String.no, delegate
                {
                    deleteAlert.Dispose();
                });

            infoAlert.Show(); 
        }

        /// <summary>
        /// Checks if team name already exists
        /// </summary>
        private bool AlreadyExists(List<Team> teams, string teamName)
        {
            foreach (Team team in teams)
            {
                if (team.TeamName.Equals(teamName))
                {
                    return true;
                }
            }
            return false;
        }
        
        private void setupViewPager(ViewPager viewPager)
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            adapter.addFragment(myTeamsListFragment, Resources.GetString(Resource.String.my_teams));
            adapter.addFragment(allTeamsListFragment, Resources.GetString(Resource.String.all_teams));
            viewPager.Adapter = adapter;
        }
    }
}