using System.Collections.Generic;
using Android.App;
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
    [Activity(ParentActivity=typeof(MainActivity))]
    public class TeamsActivity : AppCompatActivity
    {
        private TabLayout tabLayout;
        private ViewPager viewPager;
        private TeamRepository teamRepository;
        private List<Team> teamList;
        private TeamListFragment myTeamsListFragment = new TeamListFragment();
        private TeamListFragment allTeamsListFragment = new TeamListFragment();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Teams);
            
            viewPager = (ViewPager) FindViewById(Resource.Id.viewpager);
            setupViewPager(viewPager);
 
            tabLayout = (TabLayout) FindViewById(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);
            
            NavigationHelper.SetupNavigationListener(this);
            NavigationHelper.SetActionBarNavigationText(this, Resource.String.nav_teams);

            teamRepository = DependencyService.Get<TeamRepository>();

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

            foreach (Game g in games)
            {
                if (g.Team1Score > g.Team2Score)
                    winnerID = g.Team1.id;
                else winnerID = g.Team2.id;
                foreach (Team t in teams)
                {
                    if (g.Team1.id == t.id)
                    {
                        t.GamesPlayed = ++t.GamesPlayed;
                    }
                    if (g.Team2.id == t.id)
                    {
                        t.GamesPlayed = ++t.GamesPlayed;
                    }
                    if (winnerID == t.id)
                        t.GamesWon = ++t.GamesWon;
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
            input.Hint = "New team name";
            Android.App.AlertDialog.Builder infoAlert = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog.Builder changeAlert = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog.Builder deleteAlert = new Android.App.AlertDialog.Builder(this);
            
            teamRepository = DependencyService.Get<TeamRepository>();
            
            infoAlert.SetTitle(team.TeamName)
                .SetMessage(System.String.Format("Games won: {0}\nGames played: {1}\n" +
                "Win percentage: {2}%", team.GamesWon, team.GamesPlayed, team.Percentage))
                .SetCancelable(false)
                .SetNegativeButton("Edit", delegate
                {
                    changeAlert.Show();
                })
                .SetNeutralButton("Delete", delegate
                {
                    deleteAlert.Show();
                })
                .SetPositiveButton("Cancel", delegate
                {
                    infoAlert.Dispose();
                });

            changeAlert.SetMessage("Change team name")
                .SetCancelable(false)
                .SetView(input)
                .SetNegativeButton("Ok", async delegate
                {
                    if (!AlreadyExists(teamList, input.Text))
                    {
                        team.TeamName = input.Text;
                        t = await teamRepository.Update(team.id, team);
                        changeAlert.Dispose();
                        infoAlert.Dispose();
                    }
                    else
                        Toast.MakeText(Android.App.Application.Context, "The team " + input.Text + " already exists.", ToastLength.Short).Show();
                })
                .SetPositiveButton("Cancel", delegate
                {
                    changeAlert.Dispose();
                });
            
            deleteAlert.SetMessage(System.String.Format("Delete {0}?", team.TeamName))
                .SetCancelable(false)
                .SetNegativeButton("Yes", async delegate
                {
                    t = await teamRepository.Delete(team.id);
                    if (t != null)
                        Toast.MakeText(Android.App.Application.Context, team.TeamName + " is not deleted.", ToastLength.Short).Show();
                    else
                        Toast.MakeText(Android.App.Application.Context, team.TeamName + " is deleted.", ToastLength.Short).Show();

                    deleteAlert.Dispose();
                    infoAlert.Dispose();
                })
                .SetPositiveButton("No", delegate
                {
                    deleteAlert.Dispose();
                });

            infoAlert.Show(); 
        }

        /// <summary>
        /// Checks if team name already exists
        /// </summary>
        private bool AlreadyExists(List<Team> l, string s)
        {
            foreach (Team t in l)
            {
                if (t.TeamName.Equals(s))
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