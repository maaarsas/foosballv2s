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
using Android.Widget;

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
        private List<Team> teamList;

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

            GameRepository gameRepository = DependencyService.Get<GameRepository>();

            Game[] games = await gameRepository.GetAll();
            Team[] teams = await teamRepository.GetAll();

            teams = SetTeamsWinPercentage(teams, games);
            teamList = new List<Team>(teams);
            dialog.Dismiss();
            
            TeamAdapter teamAdapter = new TeamAdapter(this, teamList);
            teamListView.Adapter = teamAdapter;
            teamListView.ItemClick += OnListItemClick;
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
    }
}