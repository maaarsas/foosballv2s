using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using foosballv2s.Droid.Shared.Source.Entities;
using foosballv2s.Droid.Shared.Source.Services.CredentialStorage;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Helpers;
using foosballv2s.Droid.Shared.Source.Services.FoosballWebService.Repository;
using foosballv2s.Source.Activities.Adapters;
using foosballv2s.Source.Activities.Fragments;
using foosballv2s.Source.Activities.Helpers;
using Xamarin.Forms;
using Android.Widget;
using System.Linq;

namespace foosballv2s.Source.Activities
{
    /// <summary>
    /// An activity for displaying a history of games
    /// </summary>
    [Activity(ParentActivity=typeof(MainActivity), 
        ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class ExistingTournamentsActivity : AppCompatActivity
    {
        private TournamentRepository tournamentRepository;
        private ExpandableListView tournamentListView;
        private TournamentListFragment myTournamentsListFragment;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Tournaments);

            myTournamentsListFragment = new TournamentListFragment();
            var ft = FragmentManager.BeginTransaction();
            ft.Add(Resource.Id.fragment_container, myTournamentsListFragment);
            ft.Commit();

            tournamentRepository = DependencyService.Get<TournamentRepository>();
            tournamentListView = (ExpandableListView) FindViewById(Resource.Id.tournament_list_view);
        }

        protected override void OnResume()
        {
            base.OnResume();
            
            FetchAllGames();
        }

        /// <summary>
        /// Retrieves a list of games from a web service and populates a list
        /// </summary>
        private async void FetchAllGames()
        {
            ProgressDialog dialog = ProgressDialog.Show(this, "", 
                Resources.GetString(Resource.String.retrieving_all_games), true);
            
            UrlParamsFormatter urlParams = new UrlParamsFormatter();
            urlParams.AddParam("sortby", "-EndTime");
            
            Game[] games = await gameRepository.GetAll(urlParams.UrlParams);
            dialog.Dismiss();
            
            ExpandableListAdapter gameAdapter = new ExpandableListAdapter(this, new List<Game>(games));
            allGamesListFragment.GameListView.SetAdapter(gameAdapter);
        }
    }
}